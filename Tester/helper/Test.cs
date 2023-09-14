using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.mkysoft.gib.tester.types;

namespace com.mkysoft.gib.tester.helper
{
    public class Test
    {
        internal static Task<string> Fatura(string tokenDevice, string tokenName, string tokenSerial, string tokenPin, string servisAdresi, int faturaSay, int zarfSay, Belge belge, types.Zarf zarf)
        {
            return Task.Run(() =>
            {
                var Zarflar = new List<types.Zarf>();
                var Veriler = new List<byte[]>();
                string sonID = belge.BelgeId;
                try
                {
                    wcf.Helper.Log("---- " + faturaSay + " Fatura " + zarfSay  + " Zarf ---- başladı");
                    wcf.Helper.ProgressMax(faturaSay * zarfSay);
                    for (int z = 0; z < zarfSay; z++)
                    {
                        Veriler.Clear();
                        var yzarf = zarf.Clone();
                        yzarf.ZarfEttn = Guid.NewGuid().ToString().ToLower();
                        for (int b = 0; b < faturaSay; b++)
                        {
                            belge.BelgeId = sonID;
                            byte[] fatura = helper.Fatura.Olustur(belge);
                            fatura = helper.Fatura.Imzala(fatura, tokenDevice, tokenName, tokenSerial, tokenPin);
                            Veriler.Add(fatura);
                            belge.ZarfEttn = yzarf.ZarfEttn;
                            wcf.Helper.BelgeEkle(types.Enums.Yon.GIDEN, types.Enums.ElmntType.INVOICE, belge);
                            //sonraki belge numarası
                            sonID = helper.Fatura.SonrakiFaturaID(sonID);
                            wcf.Helper.ProgressNext();
                        }
                        yzarf.Data = helper.Zarf.Olustur(ref yzarf, Veriler);

                        yzarf.Data = Araclar.Compress(new MemoryStream(yzarf.Data), yzarf.ZarfEttn + ".xml").ToArray();
                        Zarflar.Add(yzarf);
                    }
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    foreach (var zrf in Zarflar) 
                    {
                        string mesaj = helper.Gib.Send(servisAdresi, zrf.ZarfEttn, zrf.Data);
                        wcf.Helper.Log("Zarf Gönderim: " + zrf.ZarfEttn + " " + mesaj);
                        wcf.Helper.ZarfGitti(zrf);
                    }
                    // the code that you want to measure comes here
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds / 1000;
                    wcf.Helper.Log("---- " + faturaSay + " Fatura " + zarfSay + " Zarf Süre " + watch.ElapsedMilliseconds / 1000 + " ---- bitti");
                }
                catch (Exception ex)
                {
                    wcf.Helper.Log("[HATA] Fatura: " + ex.ToString());
                    throw new ApplicationException(ex.ToString());
                }
                return belge.BelgeId;
            });
        }

        internal static Task HataliZarf(string servisAdresi)
        {
            return Task.Run(() =>
            {
                try
                {
                    string ettn = "hatali_zarf_ettn";
                    string mesaj = helper.Gib.Send(servisAdresi, ettn, new byte[5]);
                    wcf.Helper.Log("Zarf Gönderim: " + ettn + " " + mesaj);
                }
                catch (Exception ex)
                {
                    wcf.Helper.Log(ex.ToString());
                }
            });
        }

        internal static Task<string> Istisna(string tokenDevice, string tokenName, string tokenSerial, string tokenPin, string servisAdresi, Belge belge, types.Zarf zarf)
        {
            return Task.Run(() =>
            {
                try
                {
                    belge.BelgeTipi = Enums.InvType.ISTISNA;
                    byte[] fatura = helper.Fatura.Olustur(belge);
                    fatura = helper.Fatura.Imzala(fatura, tokenDevice, tokenName, tokenSerial, tokenPin);
                    var data = helper.Zarf.Olustur(ref zarf, fatura);
                    belge.ZarfEttn = zarf.ZarfEttn;
                    wcf.Helper.BelgeEkle(types.Enums.Yon.GIDEN, types.Enums.ElmntType.INVOICE, belge);
                    data = Araclar.Compress(new MemoryStream(data), zarf.ZarfEttn + ".xml").ToArray();
                    string mesaj = helper.Gib.Send(servisAdresi, zarf.ZarfEttn, data);
                    wcf.Helper.Log("İstisna Fatura Gönderim: " + belge.BelgeEttn + " " + mesaj);
                    wcf.Helper.ZarfGitti(zarf);
                }
                catch (Exception ex)
                {
                    wcf.Helper.Log("[HATA] İstisna fatura oluşturulamadı:" + ex.ToString());
                    throw new ApplicationException(ex.ToString());
                }
                return belge.BelgeId;
            });
        }

        public static void Kullanici(bool Iptal, string ServisAdresi, string OEDevice, string OEToken, string OESerial, string OEPin, List<types.Musteri> musteriler, types.Zarf zarf)
        {
            try
            {
                var belgerler = new List<byte[]>();
                zarf.ZarfEttn = Guid.NewGuid().ToString().ToLower();
                foreach (var musteri in musteriler)
                {
                    var belge = new types.Belge(zarf);
                    byte[] data;
                    if (Iptal)
                        data = helper.Kullanici.Iptal(zarf.GonderenVKN_TCKN, musteri.VKN, musteri.Unvan, musteri.Etiketler);
                    else
                        data = helper.Kullanici.Ac(zarf.GonderenVKN_TCKN, musteri.VKN, musteri.Unvan, musteri.Etiketler);
                    data = helper.Kullanici.Imzala(data, OEDevice, OEToken, OESerial, OEPin);
                    data = helper.Kullanici.Imzala(data, musteri.TokenDevice, musteri.TokenName, musteri.TokenSerial, musteri.TokenPin);
                    belgerler.Add(data);
                    belge.ZarfEttn = zarf.ZarfEttn;
                    wcf.Helper.BelgeEkle(types.Enums.Yon.GIDEN, types.Enums.ElmntType.PROCESSUSERACCOUNT, belge);
                }
                var zdata = helper.Zarf.Olustur(ref zarf, belgerler);
                zdata = Araclar.Compress(new MemoryStream(zdata), zarf.ZarfEttn + ".xml").ToArray();
                string mesaj = helper.Gib.Send(ServisAdresi, zarf.ZarfEttn, zdata);
                wcf.Helper.Log("Kullanıcı " + (Iptal ? "Hesap İptal" : "Hesap Açma") + " Gönderim: " + zarf.ZarfEttn + " " + mesaj);
                wcf.Helper.ZarfGitti(zarf);
            }
            catch (Exception ex)
            {
                if (Iptal)
                    wcf.Helper.Log("[HATA] Kullanici İptal:" + ex.ToString());
                else
                    wcf.Helper.Log("[HATA] Kullanıcı Açma:" + ex.ToString());
                throw new ApplicationException(ex.ToString());
            }
        }

        internal static Task PostaKutusu(string tokenDevice, string tokenName, string tokenSerial, string tokenPin, string servisAdresi, types.Zarf zarf, List<types.Musteri> musteriler)
        {
            return Task.Run(() =>
            {
                var Zarflar = new List<types.Zarf>();
                var Veriler = new List<byte[]>();
                try
                {
                    wcf.Helper.ProgressMax(musteriler.Sum(x=>x.FaturaSay * x.ZarfSay));
                    wcf.Helper.Log("---- Posta Kutusu Testleri ---- başladı");
                    foreach (var musteri in musteriler)
                    {
                        zarf.GonderenVKN_TCKN = musteri.VKN;
                        zarf.GB = musteri.Etiketler[0].GB;
                        zarf.GonderenUnvan = musteri.Unvan;
                        string sonID = musteri.ID;

                        wcf.Helper.Log("---- " + musteri.FaturaSay + " Fatura " + musteri.ZarfSay + " Zarf ---- başladı");
                        for (int z = 0; z < musteri.ZarfSay; z++)
                        {
                            Veriler.Clear();
                            var yzarf = zarf.Clone();
                            yzarf.ZarfEttn = Guid.NewGuid().ToString().ToLower();
                            var belge = new types.Belge(zarf);
                            belge.BelgeProfili = types.Enums.InvProfile.TEMELFATURA;
                            belge.BelgeTipi = types.Enums.InvType.SATIS;
                            for (int b = 0; b < musteri.FaturaSay; b++)
                            {
                                belge.BelgeId = sonID;
                                byte[] fatura = helper.Fatura.Olustur(belge);
                                fatura = helper.Fatura.Imzala(fatura, tokenDevice, tokenName, tokenSerial, tokenPin);
                                Veriler.Add(fatura);
                                belge.ZarfEttn = yzarf.ZarfEttn;
                                wcf.Helper.BelgeEkle(types.Enums.Yon.GIDEN, types.Enums.ElmntType.INVOICE, belge);
                                //sonraki belge numarası
                                sonID = helper.Fatura.SonrakiFaturaID(sonID);
                                wcf.Helper.ProgressNext();
                            }
                            yzarf.Data = helper.Zarf.Olustur(ref yzarf, Veriler);

                            yzarf.Data = Araclar.Compress(new MemoryStream(yzarf.Data), yzarf.ZarfEttn + ".xml").ToArray();
                            Zarflar.Add(yzarf);
                        }
                        musteri.ID = sonID;
                    }
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    foreach (var zrf in Zarflar)
                    {
                        string mesaj = helper.Gib.Send(servisAdresi, zrf.ZarfEttn, zrf.Data);
                        wcf.Helper.Log("Zarf Gönderim: " + zrf.ZarfEttn + " " + mesaj);
                        wcf.Helper.ZarfGitti(zrf);
                    }
                    // the code that you want to measure comes here
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds / 1000;
                    wcf.Helper.Log("---- Posta Kutusu Testleri Süre " + watch.ElapsedMilliseconds / 1000 + " ---- bitti");
                }
                catch (Exception ex)
                {
                    wcf.Helper.Log("[HATA] Posta Kutusu: " + ex.ToString());
                    throw new ApplicationException(ex.ToString());
                }
            });
        }
    }
}

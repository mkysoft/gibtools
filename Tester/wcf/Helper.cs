using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace com.mkysoft.gib.tester.wcf
{
    public class Helper
    {
        private static ServiceHost _ServiceHost = null;

        public static DataTable dtZarflar = new DataTable();
        public static DataTable dtBelgeler = new DataTable();

        public static System.Windows.Forms.ListBox Logger;
        public static System.Windows.Forms.ProgressBar Progress;

        static Helper()
        {
            //Zarflar
            dtZarflar.Columns.Add("YON", typeof(types.Enums.Yon));
            dtZarflar.Columns.Add("GonderenVKN_TCKN", typeof(string));
            dtZarflar.Columns.Add("GB", typeof(string));
            dtZarflar.Columns.Add("GonderenUnvan", typeof(string));
            dtZarflar.Columns.Add("AliciVKN_TCKN", typeof(string));
            dtZarflar.Columns.Add("PK", typeof(string));
            dtZarflar.Columns.Add("AliciUnvan", typeof(string));
            dtZarflar.Columns.Add("ZTIPI", typeof(types.Enums.EnvType));
            dtZarflar.Columns.Add("ZETTN", typeof(string));
            dtZarflar.Columns.Add("ZTARIH", typeof(string));
            dtZarflar.Columns.Add("TARIH", typeof(DateTime));
            dtZarflar.Columns.Add("KOD", typeof(string));
            dtZarflar.Columns.Add("MESAJ", typeof(string));
            dtZarflar.Columns.Add("DURUM", typeof(types.Enums.Durum));

            //Belgeler
            dtBelgeler.Columns.Add("YON", typeof(types.Enums.Yon));
            dtBelgeler.Columns.Add("TIP", typeof(types.Enums.ElmntType));
            dtBelgeler.Columns.Add("GonderenVKN_TCKN", typeof(string));
            dtBelgeler.Columns.Add("GonderenUnvan", typeof(string));
            dtBelgeler.Columns.Add("AliciVKN_TCKN", typeof(string));
            dtBelgeler.Columns.Add("AliciUnvan", typeof(string));
            dtBelgeler.Columns.Add("ZETTN", typeof(string));
            dtBelgeler.Columns.Add("FETTN", typeof(string));
            dtBelgeler.Columns.Add("FID", typeof(string));
            dtBelgeler.Columns.Add("FTARIH", typeof(string));
            dtBelgeler.Columns.Add("FPROFILI", typeof(types.Enums.InvProfile));
            dtBelgeler.Columns.Add("FTIPI", typeof(types.Enums.InvType));
            dtBelgeler.Columns.Add("FCEVAP", typeof(types.Enums.Cevap));
            dtBelgeler.Columns.Add("REFERANS", typeof(string));
            dtBelgeler.Columns.Add("REFERANSKODU", typeof(string));
            dtBelgeler.Columns.Add("REFERANSMETIN", typeof(string));

            Logger = new System.Windows.Forms.ListBox();
            Progress = new System.Windows.Forms.ProgressBar();
        }

        public static void Start(System.Windows.Forms.ListBox lbLog, System.Windows.Forms.ProgressBar progress)
        {
            Logger = lbLog;
            Progress = progress;
            try
            {
                _ServiceHost = new ServiceHost(typeof(wcf.ReceiverGIB));
                _ServiceHost.Open();
                Logger.Items.Add("GİB servisi başlatıldı: http://localhost:543/GIBReceiver");
            }
            catch (Exception ex)
            {
                _ServiceHost = null;
                Logger.Items.Add("GİB servisi başlatılamadı, Hata: " + ex.Message);
            }
        }

        public static void Stop()
        {
            _ServiceHost.Close();
            _ServiceHost = null;
            Logger.Items.Add("GİB servisi durduruldu.");
        }

        internal static void ZarfGeldi(types.Zarf envelope)
        {
            ListeZarf(types.Enums.Yon.GELEN, envelope);
        }

        internal static void ZarfGitti(types.Zarf envelope)
        {
            ListeZarf(types.Enums.Yon.GIDEN, envelope);
        }

        internal static void ListeZarf(types.Enums.Yon yon, types.Zarf envelope)
        {
            var row = dtZarflar.NewRow();
            row["YON"] = yon;
            row["AliciVKN_TCKN"] = envelope.AliciVKN_TCKN;
            row["PK"] = envelope.PK;
            row["AliciUnvan"] = envelope.AliciUnvan;
            row["GonderenVKN_TCKN"] = envelope.GonderenVKN_TCKN;
            row["GB"] = envelope.GB;
            row["GonderenUnvan"] = envelope.GonderenUnvan;
            row["ZTIPI"] = envelope.EnvType;
            row["ZETTN"] = envelope.ZarfEttn;
            row["ZTARIH"] = envelope.ZarfTarih;
            row["TARIH"] = DateTime.Now;
            row["KOD"] = envelope.ZarfKodu;
            row["DURUM"] = types.Enums.Durum.YENI;
            dtZarflar.Rows.Add(row);
        }

        internal static void BelgeEkle(types.Enums.Yon yon, types.Enums.ElmntType elmntType, types.Belge belge)
        {
            var row = dtBelgeler.NewRow();
            row["YON"] = yon;
            row["TIP"] = elmntType;
            row["AliciVKN_TCKN"] = belge.AliciVKN_TCKN;
            row["AliciUnvan"] = belge.AliciUnvan;
            row["GonderenVKN_TCKN"] = belge.GonderenVKN_TCKN;
            row["GonderenUnvan"] = belge.GonderenUnvan;
            row["ZETTN"] = belge.ZarfEttn;
            row["FETTN"] = belge.BelgeEttn;
            row["FID"] = belge.BelgeId;
            row["FTARIH"] = belge.BelgeTarihi;
            row["FPROFILI"] = belge.BelgeProfili;
            row["FTIPI"] = belge.BelgeTipi;
            row["REFERANS"] = belge.Referans;
            row["REFERANSKODU"] = belge.ReferansKodu;
            row["REFERANSMETIN"] = belge.ReferansMetin;
            dtBelgeler.Rows.Add(row);
        }

        internal static void Log(string message)
        {
            if (Thread.CurrentThread.IsBackground && Logger.IsHandleCreated)
            {
                Logger.Invoke(new Action(() =>
                {
                    Logger.Items.Add(message);
                    Logger.TopIndex = Logger.Items.Count - 1;
                }));
            }
            else
            {
                Logger.Items.Add(message);
                Logger.TopIndex = Logger.Items.Count - 1;
            }
        }

        internal static void ZarfDurumGuncelle(types.Belge belge)
        {
            int.TryParse(belge.ReferansKodu, out int kod);
            var durum = types.Enums.Durum.YENI;
            if (kod > 1100 && kod < 1200)
                durum = types.Enums.Durum.HATA;
            else if (kod == 1200)
                durum = types.Enums.Durum.CEVAP;
            else if (kod == 1230)
                durum = types.Enums.Durum.HATA;
            else if (kod == 1300)
                durum = types.Enums.Durum.TAMAM;
            ZarfDurumGuncelle(belge.Referans, durum, belge.ReferansKodu, belge.ReferansMetin);
        }

        internal static void ZarfDurumGuncelle(string zarfEttn, types.Enums.Durum durum, string kod, string mesaj)
        {
            var result = dtZarflar.Select("ZETTN = '" + zarfEttn + "'");
            if (result.Length > 0)
            {
                result[0]["KOD"] = kod;
                result[0]["MESAJ"] = mesaj;
                result[0]["DURUM"] = durum;
            }
        }

        internal static void BelgeDurumGuncelle(types.Belge belge, types.Enums.Cevap cevap)
        {
            var result = dtBelgeler.Select("FETTN = '" + belge.Referans + "'");
            if (result.Length > 0)
            {
                result[0]["FCEVAP"] = cevap;
            }
        }

        internal static void ProgressMax(int maxvalue)
        {
            if (Thread.CurrentThread.IsBackground)
            {
                Progress.Invoke(new Action(() =>
                {
                    Progress.Maximum = maxvalue;
                    Progress.Value = 0;
                }));
            }
            else
            {
                Progress.Maximum = maxvalue;
                Progress.Value = 0;
            }
        }

        internal static void ProgressValue(int value)
        {
            if (Thread.CurrentThread.IsBackground)
            {
                Progress.Invoke(new Action(() =>
                {
                    Progress.Maximum = value;
                }));
            }
            else
            {
                Progress.Maximum = value;
            }
        }

        internal static void ProgressNext()
        {
            if (Thread.CurrentThread.IsBackground)
            {
                Progress.Invoke(new Action(() =>
                {
                    Progress.Value++;
                }));
            }
            else
            {
                Progress.Value++;
            }
        }
    }
}

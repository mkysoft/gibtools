using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.mkysoft.gib.tester.test
{
    [TestClass]
    public class Kullanici
    {
        [TestMethod]
        public void Ac()
        {
            var etketler = new List<types.Etiket>();
            etketler.Add(new types.Etiket()
            {
                GB = "urn:mail:defaultgb@mkysoft.com",
                PK = "urn:mail:defaultpk@mkysoft.com"
            });
            var data = helper.Kullanici.Ac("1000000001", "1234567801", "Test Kurum Bir", etketler);
            File.WriteAllBytes("userprocess.xml", data);
            data = helper.Kullanici.Imzala(data, "PFX", "test1@test.com_745418.pfx", null, "745418");
            File.WriteAllBytes("userprocess-signed.xml", data);
        }

        [TestMethod]
        public void AcCokluEtiket()
        {
            var etketler = new List<types.Etiket>();
            etketler.Add(new types.Etiket()
            {
                GB = "urn:mail:default1gb@mkysoft.com",
                PK = "urn:mail:default1pk@mkysoft.com"
            });
            etketler.Add(new types.Etiket()
            {
                GB = "urn:mail:default2gb@mkysoft.com",
                PK = "urn:mail:default2pk@mkysoft.com"
            });
            var data = helper.Kullanici.Ac("1000000001", "1234567801", "Test Kurum Bir", etketler);
            File.WriteAllBytes("userprocess.xml", data);
            data = helper.Kullanici.Imzala(data, "PFX", "test1@test.com_745418.pfx", null, "745418");
            File.WriteAllBytes("userprocess-signed.xml", data);
        }

        [TestMethod]
        public void Iptal()
        {
            var etketler = new List<types.Etiket>();
            etketler.Add(new types.Etiket() { 
                GB = "urn:mail:defaultgb@mkysoft.com",
                PK ="urn:mail:defaultpk@mkysoft.com" 
            });
            var data = helper.Kullanici.Iptal("1000000001", "11111111101", "Test Kullanıcı Bir", etketler);
            File.WriteAllBytes("usercancel.xml", data);
            //data = helper.Kullanici.Imzala(data, "PFX", "112842_testbir@test.com.tr.pfx", null, "112842");
            data = helper.Kullanici.Imzala(data, "PFX", "test1@test.com_150191.pfx", null, "681729");
            //data = helper.Kullanici.Imzala(data, "PFX", "test1@test.com_745418.pfx", null, "745418");
            File.WriteAllBytes("usercancel-signed.xml", data);
        }

        [TestMethod]
        public void IptalCokluEtiket()
        {
            var etketler = new List<types.Etiket>();
            etketler.Add(new types.Etiket()
            {
                GB = "urn:mail:defaultgb@mkysoft.com",
                PK = "urn:mail:defaultpk@mkysoft.com"
            });
            etketler.Add(new types.Etiket()
            {
                GB = "urn:mail:default2gb@mkysoft.com",
                PK = "urn:mail:default2pk@mkysoft.com"
            });
            var data = helper.Kullanici.Iptal("1000000001", "1234567801", "Test Kurum Bir", etketler);
            File.WriteAllBytes("usercancel.xml", data);
            data = helper.Kullanici.Imzala(data, "PFX", "testkurum02@test.com.tr_310673.pfx", null, "310673");
            //data = helper.Kullanici.Imzala(data, "PFX", "test1@test.com_745418.pfx", null, "745418");
            File.WriteAllBytes("usercancel-signed.xml", data);
        }

        [TestMethod]
        public void KullaniciAc()
        {
            var zarf = new types.Zarf();
            zarf.EnvType = types.Enums.EnvType.USERENVELOPE;
            zarf.ElmntType = types.Enums.ElmntType.PROCESSUSERACCOUNT;
            zarf.GB = "usergb";
            zarf.GonderenVKN_TCKN = "1000000001";
            zarf.GonderenUnvan = "mkysoft Yazılım";
            zarf.AliciVKN_TCKN = "3900383669";
            zarf.AliciUnvan = "Gelir İdaresi Başlanlığı";
            zarf.PK = "GIB";

            var musteri = new types.Musteri();
            musteri.VKN = "1234567801";
            musteri.Unvan = "Test Kurum Bir";
            musteri.TokenDevice = xades.Enums.Device.PFX;
            musteri.TokenName = "test1@test.com_745418.pfx";
            musteri.TokenPin = "745418";
            musteri.Etiketler = new List<types.Etiket>();
            musteri.Etiketler.Add(new types.Etiket()
            {
                GB = "urn:mail:defaultgb@mkysoft.com",
                PK = "urn:mail:defaultpk@mkysoft.com"
            });

            helper.Test.Kullanici(false, "http://dummy", "PFX", "test1@test.com_745418.pfx", null, "745418", new List<types.Musteri>() { musteri }, zarf);
        }

        [TestMethod]
        public void KullaniciIptal()
        {
            var zarf = new types.Zarf();
            zarf.EnvType = types.Enums.EnvType.USERENVELOPE;
            zarf.ElmntType = types.Enums.ElmntType.CANCELUSERACCOUNT;
            zarf.GB = "usergb";
            zarf.GonderenVKN_TCKN = "1000000001";
            zarf.GonderenUnvan = "mkysoft Yazılım";
            zarf.AliciVKN_TCKN = "3900383669";
            zarf.AliciUnvan = "Gelir İdaresi Başlanlığı";
            zarf.PK = "GIB";

            var musteri = new types.Musteri();
            musteri.VKN = "1234567801";
            musteri.Unvan = "Test Kurum Bir";
            musteri.TokenDevice = xades.Enums.Device.PFX;
            musteri.TokenName = "test1@test.com_745418.pfx";
            musteri.TokenPin = "745418";
            musteri.Etiketler = new List<types.Etiket>();
            musteri.Etiketler.Add(new types.Etiket()
            {
                GB = "urn:mail:defaultgb@mkysoft.com",
                PK = "urn:mail:defaultpk@mkysoft.com"
            });

            helper.Test.Kullanici(true, "http://dummy", "PFX", "testkurum02@test.com.tr_310673.pfx", null, "310673", new List<types.Musteri>() { musteri }, zarf);
        }

        [TestMethod]
        public void KullaniciIptalCoklu()
        {
            var zarf = new types.Zarf();
            zarf.EnvType = types.Enums.EnvType.USERENVELOPE;
            zarf.ElmntType = types.Enums.ElmntType.CANCELUSERACCOUNT;
            zarf.GB = "usergb";
            zarf.GonderenVKN_TCKN = "1000000001";
            zarf.GonderenUnvan = "mkysoft Yazılım";
            zarf.AliciVKN_TCKN = "3900383669";
            zarf.AliciUnvan = "Gelir İdaresi Başlanlığı";
            zarf.PK = "GIB";

            var musteriler = new List<types.Musteri>();
            var musteri = new types.Musteri()
            {
                VKN = "1234567801",
                Unvan = "Test Kurum Bir",
                TokenDevice = xades.Enums.Device.PFX,
                TokenName = "test1@test.com_745418.pfx",
                TokenPin = "745418",
                Etiketler = new List<types.Etiket>()
                {
                    new types.Etiket()
                    {
                        GB = "urn:mail:default1gb@mkysoft.com",
                        PK = "urn:mail:default1pk@mkysoft.com"
                    },
                    new types.Etiket()
                    {
                        GB = "urn:mail:default2gb@mkysoft.com",
                        PK = "urn:mail:default2pk@mkysoft.com"
                    }
                }
            };
            musteriler.Add(musteri);
            musteri = new types.Musteri()
            {
                VKN = "1234567802",
                Unvan = "Test Kurum İki",
                TokenDevice = xades.Enums.Device.PFX,
                TokenName = "testkurum02@test.com.tr_310673.pfx",
                TokenPin = "310673",
                Etiketler = new List<types.Etiket>()
                {
                    new types.Etiket()
                    {
                        GB = "urn:mail:default1gb@mkysoft.com",
                        PK = "urn:mail:default1pk@mkysoft.com"
                    },
                    new types.Etiket()
                    {
                        GB = "urn:mail:default2gb@mkysoft.com",
                        PK = "urn:mail:default2pk@mkysoft.com"
                    }
                }
            };
            musteriler.Add(musteri);

            helper.Test.Kullanici(true, "http://dummy", "PFX", "testkurum02@test.com.tr_310673.pfx", null, "310673", musteriler, zarf);
        }
    }
}

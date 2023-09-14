using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.mkysoft.gib.signer;
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
            data = helper.Kullanici.Imzala(data, "pfx", "937701-testkurum01@test.com.tr.pfx", null, "937701");
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
            data = helper.Kullanici.Imzala(data, "pfx", "937701-testkurum01@test.com.tr.pfx", null, "937701");
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
            //data = helper.Kullanici.Imzala(data, "pfx", "112842_testbir@test.com.tr.pfx", null, "112842");
            data = helper.Kullanici.Imzala(data, "pfx", "681729_NES_testbir@test.com.pfx", null, "681729");
            //data = helper.Kullanici.Imzala(data, "pfx", "937701-testkurum01@test.com.tr.pfx", null, "937701");
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
            data = helper.Kullanici.Imzala(data, "pfx", "296274-testkurum02@test.com.tr.pfx", null, "296274");
            //data = helper.Kullanici.Imzala(data, "pfx", "937701-testkurum01@test.com.tr.pfx", null, "937701");
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
            musteri.TokenDevice = signer.enums.Device.pfx;
            musteri.TokenName = "937701-testkurum01@test.com.tr.pfx";
            musteri.TokenPin = "937701";
            musteri.Etiketler = new List<types.Etiket>();
            musteri.Etiketler.Add(new types.Etiket()
            {
                GB = "urn:mail:defaultgb@mkysoft.com",
                PK = "urn:mail:defaultpk@mkysoft.com"
            });

            helper.Test.Kullanici(false, "http://dummy", "pfx", "937701-testkurum01@test.com.tr.pfx", null, "937701", new List<types.Musteri>() { musteri }, zarf);
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
            musteri.TokenDevice = signer.enums.Device.pfx;
            musteri.TokenName = "937701-testkurum01@test.com.tr.pfx";
            musteri.TokenPin = "937701";
            musteri.Etiketler = new List<types.Etiket>();
            musteri.Etiketler.Add(new types.Etiket()
            {
                GB = "urn:mail:defaultgb@mkysoft.com",
                PK = "urn:mail:defaultpk@mkysoft.com"
            });

            helper.Test.Kullanici(true, "http://dummy", "pfx", "296274-testkurum02@test.com.tr.pfx", null, "296274", new List<types.Musteri>() { musteri }, zarf);
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
                TokenDevice = signer.enums.Device.pfx,
                TokenName = "937701-testkurum01@test.com.tr.pfx",
                TokenPin = "937701",
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
                TokenDevice = signer.enums.Device.pfx,
                TokenName = "296274-testkurum02@test.com.tr.pfx",
                TokenPin = "296274",
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

            helper.Test.Kullanici(true, "http://dummy", "pfx", "296274-testkurum02@test.com.tr.pfx", null, "296274", musteriler, zarf);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.mkysoft.gib.tester.test
{
    [TestClass]
    public class Fatura
    {
        [TestMethod]
        public void Olustur()
        {
            var belge = new types.Belge();
            belge.EnvType = types.Enums.EnvType.SENDERENVELOPE;
            belge.ElmntType = types.Enums.ElmntType.INVOICE;
            belge.BelgeProfili = types.Enums.InvProfile.TEMELFATURA;
            belge.BelgeTipi = types.Enums.InvType.ISTISNA;
            belge.GonderenVKN_TCKN = "1000000001";
            belge.GonderenUnvan = "MKYSOFT ANONİM ŞİRKETİ";
            belge.GB = "defaultgb";
            belge.AliciVKN_TCKN = "1000000002";
            belge.PK = "defaultpk";
            belge.AliciUnvan = "MKYSOFT ANONİM ŞİRKETİ Test Hesabı";
            var data = helper.Fatura.Olustur(belge);
            File.WriteAllBytes(belge.BelgeEttn, data);
        }

        [TestMethod]
        public void Cevap()
        {
            var belge = new types.Belge();
            belge.BelgeEttn = "TicariKabul.xml";
            belge.BelgeId = "TicariKabul";
            belge.EnvType = types.Enums.EnvType.SENDERENVELOPE;
            belge.ElmntType = types.Enums.ElmntType.INVOICE;
            belge.BelgeTipi = types.Enums.InvType.ISTISNA;
            belge.GonderenVKN_TCKN = "1000000002";
            belge.GonderenUnvan = "MKYSOFT ANONİM ŞİRKETİ Test Hesabı";
            belge.GB = "defaultpk";
            belge.AliciVKN_TCKN = "1000000001";
            belge.PK = "defaultgb";
            belge.AliciUnvan = "MKYSOFT ANONİM ŞİRKETİ";
            var data = helper.Cevap.Fatura(belge, types.Enums.Cevap.KABUL, out string belgeEttn);
            data = helper.Fatura.Imzala(data, "937701-testkurum01@test.com.tr.pfx", null, null, "937701");
            File.WriteAllBytes(belge.BelgeEttn, data);
        }

    }
}

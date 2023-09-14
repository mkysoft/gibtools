using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.mkysoft.gib.tester.wcf;

namespace com.mkysoft.gib.tester.test
{
    [TestClass]
    public class Zarf
    {
        [TestMethod]
        public void TestOlustur()
        {
            var invoice = File.ReadAllBytes(@"resource\unsigned_invoice.xml");
            var envelope = new types.Zarf();
            envelope.EnvType = types.Enums.EnvType.SENDERENVELOPE;
            envelope.ElmntType = types.Enums.ElmntType.INVOICE;
            envelope.GonderenVKN_TCKN = "1000000001";
            envelope.GonderenUnvan = "MKYSOFT ANONİM ŞİRKETİ";
            envelope.GB = "defaultgb";
            envelope.AliciVKN_TCKN = "1000000002";
            envelope.PK = "defaultpk";
            envelope.AliciUnvan = "MKYSOFT ANONİM ŞİRKETİ Test Hesabı";
            var data = helper.Zarf.Olustur(ref envelope, new List<byte[]> { invoice, invoice });
            File.WriteAllBytes(envelope.ZarfEttn, data);
        }

        [TestMethod]
        public void GIBTestSenderEnvelope()
        {
            var request = new sendDocument();
            request.documentRequest = new documentType();
            request.documentRequest.binaryData = new base64Binary();
            request.documentRequest.binaryData.Value = File.ReadAllBytes(@"resource\GIBTestSenderEnvelope.zip");
            request.documentRequest.fileName = "4b289c44-4dd2-45b3-a0e5-d0e76f36750d.zip";
            request.documentRequest.hash = helper.Araclar.MD5(request.documentRequest.binaryData.Value);
            new ReceiverGIB().sendDocument(request);
        }

        [TestMethod]
        public void PostboxEnvelope()
        {
            var request = new sendDocument();
            request.documentRequest = new documentType();
            request.documentRequest.binaryData = new base64Binary();
            request.documentRequest.binaryData.Value = File.ReadAllBytes(@"resource\PostboxEnvelope.zip");
            request.documentRequest.fileName = "D429EB9D-11CD-4448-9D90-25E04D593824.zip";
            request.documentRequest.hash = helper.Araclar.MD5(request.documentRequest.binaryData.Value);
            new ReceiverGIB().sendDocument(request);
        }

        [TestMethod]
        public void Cevap()
        {
            var envelope = new types.Zarf();
            envelope.ZarfEttn = "AppRes.xml";
            envelope.EnvType = types.Enums.EnvType.SENDERENVELOPE;
            envelope.ElmntType = types.Enums.ElmntType.INVOICE;
            envelope.GonderenVKN_TCKN = "1000000001";
            envelope.GonderenUnvan = "MKYSOFT ANONİM ŞİRKETİ";
            envelope.GB = "defaultgb";
            envelope.AliciVKN_TCKN = "1000000002";
            envelope.PK = "defaultpk";
            envelope.AliciUnvan = "MKYSOFT ANONİM ŞİRKETİ Test Hesabı";
            envelope.ZarfKodu = "1140";
            envelope.ZarfMesaji = "DOKUMAN AYRIŞTIRILAMADI";
            var data = helper.Cevap.Zarf(envelope, out string belgeEttn);
            File.WriteAllBytes(envelope.ZarfEttn, data);
        }

        [TestMethod]
        public void PostboxEnvelopeMulti()
        {
            var request = new sendDocument();
            request.documentRequest = new documentType();
            request.documentRequest.binaryData = new base64Binary();
            request.documentRequest.binaryData.Value = File.ReadAllBytes(@"resource\PostboxEnvelopeMulti.zip");
            request.documentRequest.fileName = "8E654A9C-34D6-424A-B3C6-8B69ADB8E4B3.zip";
            request.documentRequest.hash = helper.Araclar.MD5(request.documentRequest.binaryData.Value);
            new ReceiverGIB().sendDocument(request);
        }

        [TestMethod]
        public void SenderEnvelopeMulti()
        {
            var request = new sendDocument();
            request.documentRequest = new documentType();
            request.documentRequest.binaryData = new base64Binary();
            request.documentRequest.binaryData.Value = File.ReadAllBytes(@"resource\SenderEnvelopeMulti.zip");
            request.documentRequest.fileName = "A0EC2B37-2FBC-4C91-9276-129D08E60FE0.zip";
            request.documentRequest.hash = helper.Araclar.MD5(request.documentRequest.binaryData.Value);
            new ReceiverGIB().sendDocument(request);
        }
    }
}

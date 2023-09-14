using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.mkysoft.gib.tester.test
{
    [TestClass]
    public class Gib
    {
        [TestMethod]
        public void TestReceiveSenderEnvelope()
        {
            helper.Gib.Receive("72277AEB-8A95-4740-9200-CAB611002F11", File.ReadAllBytes(@"resource\SenderEnvelope.zip"));
        }

        [TestMethod]
        public void TestReceivePostboxEnvelope()
        {
            helper.Gib.Receive("D429EB9D-11CD-4448-9D90-25E04D593824", File.ReadAllBytes(@"resource\PostboxEnvelope.zip"));
        }

        [TestMethod]
        public void TestReceiveSystemEnvelope()
        {
            helper.Gib.Receive("3d740366-b625-4a3d-b650-6a951639a537", File.ReadAllBytes(@"resource\SystemEnvelope.zip"));
        }
    }
}

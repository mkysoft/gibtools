using System;
using System.Collections.Generic;
using System.IO;
using com.mkysoft.gib.tester.helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.mkysoft.gib.tester.test
{
    [TestClass]
    public class Zip
    {
        [TestMethod]
        public void Ziple()
        {
            Araclar.Compress(new MemoryStream(new byte[10]), "test.xml");
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace com.mkysoft.gib.tester.helper
{
    public class Kullanici
    {
        public static byte[] Ac(string OEVKN, string MusteriVKN, string MusteriUnvan, List<types.Etiket> etiketler)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = false;
            xmlDoc.Load(@"resource\unsigned_processuser.xml");

            return Olustur(xmlDoc, OEVKN, MusteriVKN, MusteriUnvan, etiketler);
        }

        public static byte[] Iptal(string OEVKN, string MusteriVKN, string MusteriUnvan, List<types.Etiket> etiketler)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = false;
            xmlDoc.Load(@"resource\unsigned_canceluser.xml");

            return Olustur(xmlDoc, OEVKN, MusteriVKN, MusteriUnvan, etiketler);
        }

        private static byte[] Olustur(XmlDocument XmlDoc, string OEVKN, string MusteriVKN, string MusteriUnvan, List<types.Etiket> etiketler)
        {
            #region Namespace Manager
            var NSManager = new XmlNamespaceManager(XmlDoc.NameTable);
            NSManager.AddNamespace("ns", types.Enums.NS_Kullanici);
            NSManager.AddNamespace("oa", types.Enums.NS_oa);
            #endregion

            XmlDoc.SelectSingleNode("//oa:ApplicationArea/oa:Sender/oa:LogicalID", NSManager).InnerText = OEVKN;
            XmlDoc.SelectSingleNode("//oa:ApplicationArea/oa:CreationDateTime", NSManager).InnerText = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");

            var nodes = XmlDoc.SelectNodes("//ns:UserAccount", NSManager);
            //GB
            nodes[0].SelectSingleNode("ns:UserID", NSManager).InnerText = MusteriVKN;
            nodes[0].SelectSingleNode("ns:PersonName/ns:FormattedName", NSManager).InnerText = MusteriUnvan;
            if (MusteriVKN.Length == 11)
            {
                int index = MusteriUnvan.LastIndexOf(' ');
                nodes[0].SelectSingleNode("ns:PersonName/oa:GivenName", NSManager).InnerText = MusteriUnvan.Substring(0, index);
                nodes[0].SelectSingleNode("ns:PersonName/ns:FamilyName", NSManager).InnerText = MusteriUnvan.Substring(index + 1);
            }
            nodes[0].SelectSingleNode("ns:AuthorizedWorkScope/ns:WorkScopeCode", NSManager).InnerText = etiketler[0].GB;
            //PK
            nodes[1].SelectSingleNode("ns:UserID", NSManager).InnerText = MusteriVKN;
            nodes[1].SelectSingleNode("ns:PersonName/ns:FormattedName", NSManager).InnerText = MusteriUnvan;
            if (MusteriVKN.Length == 11)
            {
                int index = MusteriUnvan.LastIndexOf(' ');
                nodes[1].SelectSingleNode("ns:PersonName/oa:GivenName", NSManager).InnerText = MusteriUnvan.Substring(0, index);
                nodes[1].SelectSingleNode("ns:PersonName/ns:FamilyName", NSManager).InnerText = MusteriUnvan.Substring(index + 1);
            }
            nodes[1].SelectSingleNode("ns:AuthorizedWorkScope/ns:WorkScopeCode", NSManager).InnerText = etiketler[0].PK;
            for (var i = 1; i < etiketler.Count; i++)
            {
                var node = nodes[0].Clone();
                node.SelectSingleNode("ns:AuthorizedWorkScope/ns:WorkScopeCode", NSManager).InnerText = etiketler[i].GB;
                nodes[0].ParentNode.AppendChild(node);
                //XmlDoc.InsertAfter(node, nodes[0]);
                node = nodes[1].Clone();
                node.SelectSingleNode("ns:AuthorizedWorkScope/ns:WorkScopeCode", NSManager).InnerText = etiketler[i].PK;
                nodes[0].ParentNode.AppendChild(node);
                //XmlDoc.InsertAfter(node, nodes[0]);

            }

            using (var ms = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(ms))
                {
                    XmlDoc.WriteTo(writer);
                    writer.Flush();
                }
                return ms.ToArray();
            }
        }

        public static byte[] Imzala(byte[] kullanici, string device, string token, string serial, string pin)
        {
           return Imzala(kullanici, (xades.Enums.Device)Enum.Parse(typeof(xades.Enums.Device), device), token, serial, pin);
        }

        public static byte[] Imzala(byte[] kullanici, xades.Enums.Device device, string token, string serial, string pin)
        {
            int errcount = 0;
            while (errcount < 5)
            {
                errcount++;
                return xades.XmlSigner.Sign(device, token, serial, pin, kullanici, null);
            }
            throw new Exception("İmzalama hatası!");
        }
    }
}

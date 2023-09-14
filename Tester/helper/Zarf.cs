using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using com.mkysoft.gib.tester.types;

namespace com.mkysoft.gib.tester.helper
{
    public class Zarf
    {
        public static byte[] Olustur(ref types.Zarf envelope, byte[] Fatura)
        {
            return Olustur(ref envelope, new List<byte[]> { Fatura });
        }

        public static byte[] Olustur(ref types.Zarf envelope, List<byte[]> Faturalar)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(@"resource\empty_envelope.xml");

            #region Namespace Manager
            var NSManager = new XmlNamespaceManager(xmlDoc.NameTable);
            NSManager.AddNamespace("sh", types.Enums.NS_sh);
            NSManager.AddNamespace("ef", types.Enums.NS_ef);
            #endregion

            #region sender 
            xmlDoc.SelectSingleNode("//sh:Sender/sh:Identifier", NSManager).InnerText = envelope.GB;
            xmlDoc.SelectSingleNode("//sh:Sender/sh:ContactInformation[sh:ContactTypeIdentifier = 'VKN_TCKN']/sh:Contact", NSManager).InnerText = envelope.GonderenVKN_TCKN;
            #endregion
            #region receiver 
            xmlDoc.SelectSingleNode("//sh:Receiver/sh:Identifier", NSManager).InnerText = envelope.PK;
            xmlDoc.SelectSingleNode("//sh:Receiver/sh:ContactInformation[sh:ContactTypeIdentifier = 'VKN_TCKN']/sh:Contact", NSManager).InnerText = envelope.AliciVKN_TCKN;
            #endregion
            #region DocumentIdentification 
            #region zarfEttn
            if (String.IsNullOrEmpty(envelope.ZarfEttn))
                envelope.ZarfEttn = Guid.NewGuid().ToString().ToLower();
            xmlDoc.SelectSingleNode("//sh:InstanceIdentifier", NSManager).InnerText = envelope.ZarfEttn;
            #endregion
            //Type
            xmlDoc.SelectSingleNode("//sh:DocumentIdentification/sh:Type", NSManager).InnerText = envelope.EnvType.ToString();
            //CreationDateAndTime
            envelope.ZarfTarih = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss");
            xmlDoc.SelectSingleNode("//sh:DocumentIdentification/sh:CreationDateAndTime", NSManager).InnerText = envelope.ZarfTarih;
            #endregion
            #region elmntType 
            xmlDoc.SelectSingleNode("//ef:Package/Elements/ElementType", NSManager).InnerText = envelope.ElmntType.ToString();
            xmlDoc.SelectSingleNode("//ef:Package/Elements/ElementCount", NSManager).InnerText = Faturalar.Count.ToString();
            #endregion

            var elements = xmlDoc.SelectSingleNode("//ef:Package/Elements/ElementList", NSManager);
            foreach (var Fatura in Faturalar)
            {
                var fatDoc = new XmlDocument();
                fatDoc.PreserveWhitespace = true;
                using (var ms = new MemoryStream(Fatura))
                    fatDoc.Load(ms);
                elements.AppendChild(elements.OwnerDocument.ImportNode(fatDoc.DocumentElement, true));
            }
            using (var ms = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(ms))
                {
                    xmlDoc.WriteTo(writer);
                    writer.Flush();
                }
                return ms.ToArray();
            }
        }

        internal static void GelenZarfiCevapla(types.Zarf envelope, string endpoint)
        {
            var cevap = Cevap.Zarf(envelope, out string BelgeEttn);
            var yeni = new types.Zarf();
            yeni.EnvType = types.Enums.EnvType.SYSTEMENVELOPE;
            yeni.ElmntType = types.Enums.ElmntType.APPLICATIONRESPONSE;

            yeni.GB = envelope.PK;
            yeni.GonderenVKN_TCKN = envelope.AliciVKN_TCKN;
            yeni.GonderenUnvan = envelope.AliciUnvan;

            yeni.PK = envelope.GB;
            yeni.AliciVKN_TCKN = envelope.GonderenVKN_TCKN;
            yeni.AliciUnvan = envelope.GonderenUnvan;

            var data = Zarf.Olustur(ref yeni, cevap);
            data = Araclar.Compress(data, yeni.ZarfEttn + ".xml");

            var belge = new types.Belge(yeni);
            belge.BelgeEttn = BelgeEttn;
            Gib.Send(endpoint, yeni.ZarfEttn, data);
            wcf.Helper.ZarfGitti(belge);
            wcf.Helper.ZarfDurumGuncelle(new Belge() { Referans = envelope.ZarfEttn, ReferansKodu = envelope.ZarfKodu, ReferansMetin = envelope.ZarfMesaji });
        }

        internal static void ZarfiSorgula(types.Zarf envelope, string endpoint)
        {
            try
            {
                var appres = Gib.Request(endpoint, envelope.ZarfEttn);
                if (!String.IsNullOrEmpty(appres))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(appres);
                    var env = new types.Zarf();
                    env.EnvType = Enums.EnvType.SYSTEMENVELOPE;
                    Gib.ReceiverApplicationResponse(env, xmlDoc);
                }
            }
            catch (Exception ex)
            {
                wcf.Helper.Log("ZarfiSorgula: " + ex.ToString());
            }
        }

        public static void RowToClass(DataRow row, types.Zarf zarf)
        {
            if (zarf == null)
                zarf = new types.Zarf();
            zarf.EnvType = (types.Enums.EnvType)(row["ZTIPI"]);
            zarf.ZarfEttn = (string)(row["ZETTN"]);
            zarf.ZarfTarih = (string)(row["ZTARIH"]);

            zarf.GB = (string)(row["GB"]);
            zarf.GonderenVKN_TCKN = (string)(row["GonderenVKN_TCKN"]);
            if (row["GonderenUnvan"] != DBNull.Value)
                zarf.GonderenUnvan = (string)(row["GonderenUnvan"]);

            zarf.PK = (string)(row["PK"]);
            zarf.AliciVKN_TCKN = (string)(row["AliciVKN_TCKN"]);
            if (row["AliciUnvan"] != DBNull.Value)
                zarf.AliciUnvan = (string)(row["AliciUnvan"]);
            if (row["KOD"] != DBNull.Value)
                zarf.ZarfKodu = (string)(row["KOD"]);
            if (row["MESAJ"] != DBNull.Value)
                zarf.ZarfMesaji = (string)(row["MESAJ"]);
        }

        public static types.Zarf RowToClass(DataRow row)
        {
            var zarf = new types.Zarf();
            RowToClass(row, zarf);
            return zarf;
        }
    }
}

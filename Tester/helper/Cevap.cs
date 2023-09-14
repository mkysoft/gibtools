using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace com.mkysoft.gib.tester.helper
{
    public class Cevap
    {
        public static byte[] Zarf(types.Zarf envelope, out string belgeEttn)
        {

            XmlDocument xmlDoc = DoldurAppRes(@"resource\empty_appres.xml", envelope, out XmlNamespaceManager NSManager, out belgeEttn);


            //Gelen Zarf
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:ID", NSManager).InnerText = envelope.ZarfEttn;
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:IssueDate", NSManager).InnerText = envelope.ZarfTarih;
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:DocumentTypeCode", NSManager).InnerText = envelope.EnvType.ToString();
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:DocumentType", NSManager).InnerText = envelope.EnvType.ToString();

            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:LineReference/cac:DocumentReference/cbc:ID", NSManager).InnerText = envelope.ZarfEttn;
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:LineReference/cac:DocumentReference/cbc:IssueDate", NSManager).InnerText = envelope.ZarfTarih;

            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:ReferenceID", NSManager).InnerText = Guid.NewGuid().ToString().ToLower();

            if (String.IsNullOrEmpty(envelope.ZarfKodu))
                envelope.ZarfKodu = "1200";
            if (String.IsNullOrEmpty(envelope.ZarfMesaji))
                envelope.ZarfMesaji = "BASARI ILE ISLENDI";
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:ResponseCode", NSManager).InnerText = envelope.ZarfKodu;
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:Description", NSManager).InnerText = envelope.ZarfMesaji;

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

        public static XmlDocument DoldurAppRes(string template, types.Zarf envelope, out XmlNamespaceManager NSManager, out string belgeEttn)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = false;
            xmlDoc.Load(template);

            #region Namespace Manager
            NSManager = new XmlNamespaceManager(xmlDoc.NameTable);
            NSManager.AddNamespace("cbc", types.Enums.NS_cbc);
            NSManager.AddNamespace("cac", types.Enums.NS_cac);
            NSManager.AddNamespace("ext", types.Enums.NS_ext);
            #endregion

            xmlDoc.SelectSingleNode("//cbc:UUID", NSManager).InnerText = belgeEttn = Guid.NewGuid().ToString().ToLower();
            xmlDoc.SelectSingleNode("//cbc:IssueDate", NSManager).InnerText = DateTime.Now.ToString("yyyy-MM-dd");
            xmlDoc.SelectSingleNode("//cbc:IssueTime", NSManager).InnerText = DateTime.Now.ToString("hh:mm:ss");
            xmlDoc.SelectSingleNode("//cbc:ID", NSManager).InnerText = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            xmlDoc.SelectSingleNode("//cbc:IssueDate", NSManager).InnerText = DateTime.UtcNow.ToString("yyyy-MM-dd");
            xmlDoc.SelectSingleNode("//cbc:IssueTime", NSManager).InnerText = DateTime.UtcNow.ToString("hh:mm:ss");
            #region sender 
            xmlDoc.SelectSingleNode("//cac:SenderParty/cac:PartyName/cbc:Name", NSManager).InnerText = envelope.AliciUnvan;
            xmlDoc.SelectSingleNode("//cac:SenderParty/cac:PartyIdentification/cbc:ID[@schemeID = 'VKN']", NSManager).InnerText = envelope.AliciVKN_TCKN;
            #endregion
            #region receiver 
            xmlDoc.SelectSingleNode("//cac:ReceiverParty/cac:PartyName/cbc:Name", NSManager).InnerText = envelope.GonderenUnvan;
            xmlDoc.SelectSingleNode("//cac:ReceiverParty/cac:PartyIdentification/cbc:ID[@schemeID = 'VKN']", NSManager).InnerText = envelope.GonderenVKN_TCKN;
            #endregion
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:Response/cbc:ReferenceID", NSManager).InnerText = Guid.NewGuid().ToString().ToLower();

            return xmlDoc;
        }

        public static byte[] Fatura(types.Belge belge, types.Enums.Cevap cevap, out string belgeEttn)
        {

            XmlDocument xmlDoc = DoldurAppRes(@"resource\empty_commres.xml", belge, out XmlNamespaceManager NSManager, out belgeEttn);

            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:Response/cbc:ReferenceID", NSManager).InnerText = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:Response/cbc:ResponseCode", NSManager).InnerText = cevap.ToString();
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:Response/cbc:Description", NSManager).InnerText = "FATURAKABUL";

            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:ID", NSManager).InnerText = belge.BelgeEttn;
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:IssueDate", NSManager).InnerText = belge.BelgeTarihi;
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:DocumentTypeCode", NSManager).InnerText = "FATURA";
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:DocumentType", NSManager).InnerText = "FATURA";

            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:ReferenceID", NSManager).InnerText = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:ResponseCode", NSManager).InnerText = cevap.ToString();
            if (cevap == types.Enums.Cevap.KABUL)
                xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:Description", NSManager).InnerText = "Kabul edilmiştir.";
            else
                xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:Description", NSManager).InnerText = "Red edilmiştir.";

            #region imzalayan
            xmlDoc.SelectSingleNode("//cac:Signature/cbc:ID", NSManager).InnerText = belge.AliciVKN_TCKN;
            xmlDoc.SelectSingleNode("//cac:Signature/cac:SignatoryParty/cac:PartyIdentification/cbc:ID", NSManager).InnerText = belge.AliciVKN_TCKN;
            xmlDoc.SelectSingleNode("//cac:Signature/cac:DigitalSignatureAttachment/cac:ExternalReference/cbc:URI", NSManager).InnerText = "#Signature_" + xmlDoc.SelectSingleNode("//cbc:ID", NSManager).InnerText;
            #endregion

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
    }
}
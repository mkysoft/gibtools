using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using com.mkysoft.gib.signer;
using com.mkysoft.gib.tester.types;

namespace com.mkysoft.gib.tester.helper
{
    public class Fatura
    {
        public static byte[] Olustur(types.Belge invoice)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = false;
            xmlDoc.Load(@"resource\unsigned_invoice.xml");

            #region Namespace Manager
            var NSManager = new XmlNamespaceManager(xmlDoc.NameTable);
            NSManager.AddNamespace("cbc", types.Enums.NS_cbc);
            NSManager.AddNamespace("cac", types.Enums.NS_cac);
            #endregion

            invoice.BelgeEttn = Guid.NewGuid().ToString().ToLower();
            xmlDoc.SelectSingleNode("//cbc:UUID", NSManager).InnerText = invoice.BelgeEttn;
            xmlDoc.SelectSingleNode("//cbc:IssueDate", NSManager).InnerText = DateTime.Now.ToString("yyyy-MM-dd");
            xmlDoc.SelectSingleNode("//cbc:IssueTime", NSManager).InnerText = DateTime.Now.ToString("hh:mm:ss");
            xmlDoc.SelectSingleNode("//cbc:ID", NSManager).InnerText = invoice.BelgeId;
            xmlDoc.SelectSingleNode("//cbc:ProfileID", NSManager).InnerText = invoice.BelgeProfili.ToString();
            xmlDoc.SelectSingleNode("//cbc:InvoiceTypeCode", NSManager).InnerText = invoice.BelgeTipi.ToString();
            #region sender 
            xmlDoc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party/cac:PartyName/cbc:Name", NSManager).InnerText = invoice.GonderenUnvan;
            xmlDoc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party/cac:PartyIdentification/cbc:ID[@schemeID = 'VKN']", NSManager).InnerText = invoice.GonderenVKN_TCKN;
            #endregion
            #region receiver 
            xmlDoc.SelectSingleNode("//cac:AccountingCustomerParty/cac:Party/cac:PartyName/cbc:Name", NSManager).InnerText = invoice.AliciUnvan;
            xmlDoc.SelectSingleNode("//cac:AccountingCustomerParty/cac:Party/cac:PartyIdentification/cbc:ID[@schemeID = 'VKN']", NSManager).InnerText = invoice.AliciVKN_TCKN;
            #endregion
            #region imzalayan
            xmlDoc.SelectSingleNode("//cac:Signature/cbc:ID", NSManager).InnerText = invoice.GonderenVKN_TCKN;
            xmlDoc.SelectSingleNode("//cac:Signature/cac:SignatoryParty/cac:PartyIdentification/cbc:ID", NSManager).InnerText = invoice.GonderenVKN_TCKN;
            xmlDoc.SelectSingleNode("//cac:Signature/cac:DigitalSignatureAttachment/cac:ExternalReference/cbc:URI", NSManager).InnerText = "#Signature_" + invoice.BelgeId;
            #endregion

            #region görsel
            xmlDoc.SelectSingleNode("//cac:AdditionalDocumentReference/cbc:ID", NSManager).InnerText = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            xmlDoc.SelectSingleNode("//cac:AdditionalDocumentReference/cbc:IssueDate", NSManager).InnerText = DateTime.Now.ToString("yyyy-MM-dd");
            xmlDoc.SelectSingleNode("//cac:AdditionalDocumentReference/cac:Attachment/cbc:EmbeddedDocumentBinaryObject/@filename", NSManager).InnerText = invoice.BelgeId + ".xslt";
            #endregion

            #region istisna
            if (invoice.BelgeTipi == Enums.InvType.ISTISNA)
            {
                xmlDoc.SelectSingleNode("//cac:InvoiceLine", NSManager).RemoveChild(xmlDoc.SelectSingleNode("//cac:InvoiceLine/cac:TaxTotal", NSManager));
                xmlDoc.SelectSingleNode("//cac:TaxTotal/cbc:TaxAmount", NSManager).InnerText = "0";
                xmlDoc.SelectSingleNode("//cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount", NSManager).InnerText = "0";
                var node = xmlDoc.SelectSingleNode("//cac:TaxTotal/cac:TaxSubtotal", NSManager);
                node.RemoveChild(node.SelectSingleNode("cbc:TaxableAmount", NSManager));
                node.RemoveChild(node.SelectSingleNode("cbc:Percent", NSManager));
                var taxscheme = xmlDoc.SelectSingleNode("//cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme", NSManager);
                node = xmlDoc.CreateElement("cbc:TaxExemptionReasonCode", types.Enums.NS_cbc);
                node.InnerText = "302";
                xmlDoc.SelectSingleNode("//cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory", NSManager).InsertBefore(node, taxscheme);
                node = xmlDoc.CreateElement("cbc:TaxExemptionReason", types.Enums.NS_cbc);
                node.InnerText = "11/1-a Hizmet İhracatı";
                xmlDoc.SelectSingleNode("//cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory", NSManager).InsertBefore(node, taxscheme);
                xmlDoc.SelectSingleNode("//cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount", NSManager).InnerText = "300";
                xmlDoc.SelectSingleNode("//cac:LegalMonetaryTotal/cbc:PayableAmount", NSManager).InnerText = "300";
                var note = xmlDoc.SelectSingleNode("//cbc:Note", NSManager).InnerText = "Yalnız #üçyüz# TL İş bu fatura 3065 sayılı KDV 11/1-a Hizmet İhracatı müstesna olarak düzenlenmiştir.";
            }
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

        public static byte[] Imzala(byte[] fatura, string device, string token, string serial, string pin)
        {
            int errcount = 0;
            while (errcount < 5)
            {
                try
                {
                    var request = new SignRequest();
                    request.Device = (signer.enums.Device)Enum.Parse(typeof(signer.enums.Device), device);
                    request.SignType = signer.enums.SignType.xades;
                    request.Token = new Token() { Name = token, Serial = serial };
                    request.Pin = pin;
                    var signInvoice = new SignInvoice(request, fatura);
                    signInvoice.Sign();
                    if (!signInvoice.IsSigned)
                    {
                        wcf.Helper.Log("[HATA] İmzalama: " + signInvoice.ErrorMsg);
                        errcount++;
                        Thread.Sleep(5000);
                    }
                    else
                        return signInvoice.XmlData;
                } 
                catch (Exception ex)
                {
                    wcf.Helper.Log("[HATA] İmzalama: " + ex.Message);
                    errcount++;
                }
            }
            throw new Exception("İmzalama hatası!");
        }

        internal static string SonrakiFaturaID(string id)
        {
            var counter = int.Parse(id.Substring(8));
            counter++;
            return id.Substring(0, 7) + counter.ToString("D9");
        }

        internal static Task Cevapla(string tokenDevice, string tokenName, string tokenSerial, string tokenPin, string endpoint, Enums.Cevap cevap)
        {
            return Task.Run(() =>
            {
                try
                {
                    var query = "YON = " + (int)types.Enums.Yon.GELEN;
                    query += " AND TIP = " + (int)types.Enums.ElmntType.INVOICE;
                    query += " AND FPROFILI = " + (int)types.Enums.InvProfile.TICARIFATURA;
                    query += " AND FCEVAP IS NULL";
                    var faturalar = wcf.Helper.dtBelgeler.Select(query);
                    if (faturalar.Length == 0)
                        throw new ApplicationException("Uygun ticari fatura bulunamadı!");

                    Belge belge = new Belge();
                    Fatura.RowToClass(faturalar[0], belge);

                    query = "YON = " + (int)types.Enums.Yon.GELEN;
                    query += " AND ZETTN = '" + belge.ZarfEttn + "'";
                    var zarflar = wcf.Helper.dtZarflar.Select(query);
                    if (zarflar.Length == 0)
                        throw new ApplicationException("Fatura ait zarf bulunamadı!");

                    wcf.Helper.Log("---- Ticari " + cevap.ToString() + " gönderim ---- başladı");

                    Zarf.RowToClass(zarflar[0], belge);

                    var data = Cevap.Fatura(belge, cevap, out string belgeEtttn);
                    data = Imzala(data, tokenDevice, tokenName, tokenSerial, tokenPin);

                    var yenizarf = new types.Zarf();
                    yenizarf.EnvType = types.Enums.EnvType.POSTBOXENVELOPE;
                    yenizarf.ElmntType = types.Enums.ElmntType.APPLICATIONRESPONSE;

                    yenizarf.GB = belge.PK;
                    yenizarf.GonderenVKN_TCKN = belge.AliciVKN_TCKN;
                    yenizarf.GonderenUnvan = belge.AliciUnvan;

                    yenizarf.PK = belge.GB;
                    yenizarf.AliciVKN_TCKN = belge.GonderenVKN_TCKN;
                    yenizarf.AliciUnvan = belge.GonderenUnvan;

                    data = Zarf.Olustur(ref yenizarf, data);
                    data = Araclar.Compress(data, yenizarf.ZarfEttn + ".xml");

                    var yenibelge = new types.Belge(yenizarf);
                    yenibelge.BelgeEttn = belgeEtttn;
                    var mesaj = Gib.Send(endpoint, yenizarf.ZarfEttn, data);
                    wcf.Helper.Log("Zarf Gönderim: " + yenizarf.ZarfEttn + " " + mesaj);
                    wcf.Helper.BelgeEkle(Enums.Yon.GIDEN, Enums.ElmntType.APPLICATIONRESPONSE, yenibelge);
                    wcf.Helper.ZarfGitti(yenibelge);
                    wcf.Helper.BelgeDurumGuncelle(new Belge() { Referans = belge.BelgeEttn }, cevap);
                }
                catch(Exception ex)
                {
                    wcf.Helper.Log("[HATA] Ticari cevap: " + ex.ToString());
                    throw new ApplicationException(ex.ToString());
                }
            });
        }

        public static void RowToClass(DataRow row, types.Belge belge)
        {
            if (belge == null)
                belge = new types.Belge();

            belge.AliciVKN_TCKN = (string)row["AliciVKN_TCKN"];
            belge.AliciUnvan = (string)row["AliciUnvan"];
            belge.GonderenVKN_TCKN = (string)row["GonderenVKN_TCKN"];
            belge.GonderenUnvan = (string)row["GonderenUnvan"];
            belge.ZarfEttn = (string)row["ZETTN"];
            belge.BelgeEttn = (string)row["FETTN"];
            belge.BelgeId = (string)row["FID"];
            belge.BelgeTarihi = (string)row["FTARIH"];
            belge.BelgeProfili = (Enums.InvProfile)row["FPROFILI"];
            belge.BelgeTipi = (Enums.InvType)row["FTIPI"];
            if (row["REFERANS"] != DBNull.Value)
                belge.Referans = (string)row["REFERANS"];
            if (row["REFERANSKODU"] != DBNull.Value)
                belge.ReferansKodu = (string)row["REFERANSKODU"];
            if (row["REFERANSMETIN"] != DBNull.Value)
                belge.ReferansMetin = (string)row["REFERANSMETIN"];
        }

        public static types.Belge RowToClass(DataRow row)
        {
            var belge = new types.Belge();
            RowToClass(row, belge);
            return belge;
        }
    }
}

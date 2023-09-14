using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using com.mkysoft.gib.tester.types;

namespace com.mkysoft.gib.tester.helper
{
    public class Gib
    {
        public static string Send(string endpoint, string guid, byte[] envelope)
        {
            int counter = 0;
            do
            {
                GIBService.documentReturnType sendRequest;

                CustomBinding customBinding = new CustomBinding();
                customBinding.SendTimeout = new TimeSpan(0, 15, 0);
                customBinding.OpenTimeout = new TimeSpan(0, 5, 0);

                MtomMessageEncodingBindingElement el1 = new MtomMessageEncodingBindingElement();
                el1.MaxReadPoolSize = 64;
                el1.MaxWritePoolSize = 16;
                el1.MessageVersion = MessageVersion.Soap12;
                el1.MaxBufferSize = 2147483647;
                el1.WriteEncoding = Encoding.UTF8;

                customBinding.Elements.Add(el1);

                if (endpoint.Contains("http://"))
                {
                    HttpTransportBindingElement el2 = new HttpTransportBindingElement
                    {
                        ManualAddressing = false,
                        MaxBufferPoolSize = 2147483647,
                        MaxReceivedMessageSize = 2147483647,
                        AllowCookies = false,
                        AuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous,
                        BypassProxyOnLocal = true,
                        HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                        KeepAliveEnabled = true,
                        MaxBufferSize = 2147483647,
                        ProxyAuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous,
                        Realm = "",
                        TransferMode = System.ServiceModel.TransferMode.Buffered,
                        UnsafeConnectionNtlmAuthentication = false,
                        UseDefaultWebProxy = true
                    };
                    customBinding.Elements.Add(el2);
                }
                else
                {
                    HttpsTransportBindingElement el2 = new HttpsTransportBindingElement
                    {
                        ManualAddressing = false,
                        MaxBufferPoolSize = 2147483647,
                        MaxReceivedMessageSize = 2147483647,
                        AllowCookies = false,
                        AuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous,
                        BypassProxyOnLocal = true,
                        HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                        KeepAliveEnabled = true,
                        MaxBufferSize = 2147483647,
                        ProxyAuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous,
                        Realm = "",
                        TransferMode = TransferMode.Buffered,
                        UnsafeConnectionNtlmAuthentication = false,
                        RequireClientCertificate = false,
                        UseDefaultWebProxy = true
                    };
                    customBinding.Elements.Add(el2);
                }
                var endpointAddress = new EndpointAddress(endpoint);

                var docRequest = new GIBService.documentType();
                docRequest.fileName = guid + ".zip";
                docRequest.binaryData = new GIBService.base64Binary();
                docRequest.binaryData.Value = envelope;
                docRequest.hash = Araclar.MD5(docRequest.binaryData.Value);
                docRequest.binaryData.contentType = "application/x-zip-compressed";
                try
                {
                    var tempDir = new DirectoryInfo("envelope-out");
                    if (!tempDir.Exists)
                        tempDir.Create();
                    File.WriteAllBytes(Path.Combine(tempDir.FullName, docRequest.fileName), docRequest.binaryData.Value);
                    sendRequest = new GIBService.EFaturaPortTypeClient(customBinding, endpointAddress).sendDocument(docRequest);
                    return sendRequest.msg;
                }
                catch (FaultException<GIBService.EFaturaFaultType> ex)
                {
                    throw new Exception(ex.Detail.code + " : " + ex.Detail.msg);
                }
                catch (Exception ex)
                {
                    wcf.Helper.Log("Gönderim sırasında hata oluştu: " + guid);
                    wcf.Helper.Log(ex.ToString());
                    Thread.Sleep(5000);
                    counter++;
                    if (counter > 60 )
                        throw ex;
                }
            } while (true);
        }

        internal static string Request(string endpoint, string zarfEttn)
        {
            var client = CreateClient(endpoint);
            var request = new GIBService.getAppRespRequestType();
            request.instanceIdentifier = zarfEttn;
            try
            {
                var response = client.getApplicationResponse(request);
                return response.applicationResponse;
            }
            catch (Exception ex)
            {
                wcf.Helper.Log("Sorgulama sırasında hata oluştu: " + zarfEttn);
                wcf.Helper.Log(ex.ToString());
                //Thread.Sleep(1000);
                return null;
            }
        }

        private static GIBService.EFaturaPortTypeClient CreateClient(string endpoint)
        {
            CustomBinding customBinding = new CustomBinding();
            customBinding.SendTimeout = new TimeSpan(0, 15, 0);
            customBinding.OpenTimeout = new TimeSpan(0, 5, 0);

            MtomMessageEncodingBindingElement el1 = new MtomMessageEncodingBindingElement();
            el1.MaxReadPoolSize = 64;
            el1.MaxWritePoolSize = 16;
            el1.MessageVersion = MessageVersion.Soap12;
            el1.MaxBufferSize = 2147483647;
            el1.WriteEncoding = Encoding.UTF8;

            customBinding.Elements.Add(el1);

            if (endpoint.Contains("http://"))
            {
                HttpTransportBindingElement el2 = new HttpTransportBindingElement
                {
                    ManualAddressing = false,
                    MaxBufferPoolSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647,
                    AllowCookies = false,
                    AuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous,
                    BypassProxyOnLocal = true,
                    HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                    KeepAliveEnabled = true,
                    MaxBufferSize = 2147483647,
                    ProxyAuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous,
                    Realm = "",
                    TransferMode = System.ServiceModel.TransferMode.Buffered,
                    UnsafeConnectionNtlmAuthentication = false,
                    UseDefaultWebProxy = true
                };
                customBinding.Elements.Add(el2);
            }
            else
            {
                HttpsTransportBindingElement el2 = new HttpsTransportBindingElement
                {
                    ManualAddressing = false,
                    MaxBufferPoolSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647,
                    AllowCookies = false,
                    AuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous,
                    BypassProxyOnLocal = true,
                    HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                    KeepAliveEnabled = true,
                    MaxBufferSize = 2147483647,
                    ProxyAuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous,
                    Realm = "",
                    TransferMode = TransferMode.Buffered,
                    UnsafeConnectionNtlmAuthentication = false,
                    RequireClientCertificate = false,
                    UseDefaultWebProxy = true
                };
                customBinding.Elements.Add(el2);
            }
            var endpointAddress = new EndpointAddress(endpoint);
            return new GIBService.EFaturaPortTypeClient(customBinding, endpointAddress);
        }

        public static void Receive(string dosyaAdi, byte[] envelope)
        {
            byte[] zarf = Araclar.Decompress(envelope, out string xmlAdi);
            xmlAdi = xmlAdi.Substring(0, 36);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new MemoryStream(zarf));

            #region Namespace Manager
            var NSManager = new XmlNamespaceManager(xmlDoc.NameTable);
            NSManager.AddNamespace("sh", types.Enums.NS_sh);
            #endregion
            var envp = new types.Zarf();
            #region sender 
            var node = xmlDoc.SelectSingleNode("//sh:Sender/sh:Identifier", NSManager);
            envp.GB = node.InnerText;
            envp.GonderenVKN_TCKN = xmlDoc.SelectSingleNode("//sh:Sender/sh:ContactInformation[sh:ContactTypeIdentifier = 'VKN_TCKN']/sh:Contact", NSManager).InnerText;
            node = xmlDoc.SelectSingleNode("//sh:Sender/sh:ContactInformation[sh:ContactTypeIdentifier = 'UNVAN']/sh:Contact", NSManager);
            if (node != null)
                envp.GonderenUnvan = node.InnerText;
            #endregion
            #region receiver 
            node = xmlDoc.SelectSingleNode("//sh:Receiver/sh:Identifier", NSManager);
            envp.PK = node.InnerText;
            envp.AliciVKN_TCKN = xmlDoc.SelectSingleNode("//sh:Receiver/sh:ContactInformation[sh:ContactTypeIdentifier = 'VKN_TCKN']/sh:Contact", NSManager).InnerText;
            node = xmlDoc.SelectSingleNode("//sh:Receiver/sh:ContactInformation[sh:ContactTypeIdentifier = 'UNVAN']/sh:Contact", NSManager);
            if (node != null)
                envp.AliciUnvan = node.InnerText;
            #endregion
            #region zarfEttn ve tarih
            var nodes = xmlDoc.GetElementsByTagName("InstanceIdentifier", types.Enums.NS_sh);
            if (nodes.Count != 1)
                throw new Exception("Zarf numarası bulunamadı!");
            envp.ZarfEttn = nodes.Item(0).InnerText;
            node = xmlDoc.SelectSingleNode("//sh:DocumentIdentification/sh:CreationDateAndTime", NSManager);
            envp.ZarfTarih = node.InnerText.Substring(0, 10);
            #endregion
            #region envType 
            nodes = xmlDoc.GetElementsByTagName("Type", types.Enums.NS_sh);
            if (nodes.Count != 1)
                throw new Exception("Zarf tipi bulunamadı!");
            Enum.TryParse<types.Enums.EnvType>(nodes.Item(0).InnerText, out types.Enums.EnvType envType);
            envp.EnvType = envType;
            #endregion
            #region elmntType 
            nodes = xmlDoc.GetElementsByTagName("ElementType");
            if (nodes.Count != 1)
                throw new Exception("Belge tipi bulunamadı!");
            Enum.TryParse<types.Enums.ElmntType>(nodes.Item(0).InnerText, out types.Enums.ElmntType elmntType);
            envp.ElmntType = elmntType;
            wcf.Helper.ZarfGeldi(envp);
            #endregion
            switch (envType)
            {
                case types.Enums.EnvType.SENDERENVELOPE:
                    ReceiverSenderEnvelope(envp, xmlDoc);
                    break;
                case types.Enums.EnvType.SYSTEMENVELOPE:
                    ReceiverSystemEnvelope(envp, xmlDoc);
                    break;
                case types.Enums.EnvType.POSTBOXENVELOPE:
                    ReceiverPostboxEnvelope(envp, xmlDoc);
                    break;
                default:
                    throw new NotImplementedException(envType.ToString()); 
            }
        }

        private static void ReceiverPostboxEnvelope(types.Zarf envelope, XmlDocument xmlDoc)
        {
            try
            {
                switch (envelope.ElmntType)
                {
                    case types.Enums.ElmntType.APPLICATIONRESPONSE:
                        ReceiverCommRes(envelope, xmlDoc);
                        break;
                    default:
                        throw new NotImplementedException(envelope.ElmntType.ToString());
                }
            }
            catch (Exception)
            {
                wcf.Helper.ZarfDurumGuncelle(envelope.ZarfEttn, Enums.Durum.YENI, "1150", "SCHEMATRON KONTROL SONUCU HATALI");
                wcf.Helper.Log("GİB Test Zarfı Geldi: " + envelope.ZarfEttn);
            }
        }

        private static void ReceiverCommRes(types.Zarf envelope, XmlDocument xmlDoc)
        {
            var docs = xmlDoc.GetElementsByTagName("ElementList");
            if (docs.Count < 1)
                throw new Exception("Belge bulunamadı!");
            foreach (XmlElement doc in docs[0].ChildNodes)
            {
                XmlDocument appresXml = new XmlDocument();
                appresXml.LoadXml(doc.OuterXml);
                #region Namespace Manager
                var NSManager = new XmlNamespaceManager(xmlDoc.NameTable);
                NSManager.AddNamespace("cbc", types.Enums.NS_cbc);
                NSManager.AddNamespace("cac", types.Enums.NS_cac);
                #endregion
                var nodes = appresXml.GetElementsByTagName("UUID", types.Enums.NS_cbc);
                var appres = new types.Belge(envelope);
                appres.BelgeEttn = nodes.Item(0).InnerText;
                nodes = appresXml.GetElementsByTagName("ID", types.Enums.NS_cbc);
                appres.BelgeId = nodes.Item(0).InnerText;
                appres.BelgeTarihi = xmlDoc.SelectSingleNode("//cbc:IssueDate", NSManager).InnerText;
                appres.BelgeProfili = Enums.InvProfile.YOK;
                appres.BelgeTipi = Enums.InvType.YOK;
                appres.Referans = xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:ID", NSManager).InnerText;
                appres.ReferansKodu = xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:ResponseCode", NSManager).InnerText;
                appres.ReferansMetin = xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:Description", NSManager).InnerText;
                wcf.Helper.BelgeEkle(Enums.Yon.GELEN, Enums.ElmntType.APPLICATIONRESPONSE, appres);
                var cevap = (Enums.Cevap)Enum.Parse(typeof(Enums.Cevap), appres.ReferansKodu);
                wcf.Helper.BelgeDurumGuncelle(appres, cevap);
            }
        }

        private static void ReceiverSystemEnvelope(types.Zarf envelope, XmlDocument xmlDoc)
        {
            switch (envelope.ElmntType)
            {
                case types.Enums.ElmntType.APPLICATIONRESPONSE:
                    ReceiverApplicationResponse(envelope, xmlDoc);
                    break;
                default:
                    throw new NotImplementedException(envelope.ElmntType.ToString());
            }
        }

        public static void ReceiverApplicationResponse(types.Zarf envelope, XmlDocument xmlDoc)
        {
            var docs = xmlDoc.GetElementsByTagName("ElementList");
            if (docs.Count < 1)
                throw new Exception("Belge bulunamadı!");
            foreach (XmlElement doc in docs[0].ChildNodes)
            {
                XmlDocument appresXml = new XmlDocument();
                appresXml.LoadXml(doc.OuterXml);
                #region Namespace Manager
                var NSManager = new XmlNamespaceManager(xmlDoc.NameTable);
                NSManager.AddNamespace("cbc", types.Enums.NS_cbc);
                NSManager.AddNamespace("cac", types.Enums.NS_cac);
                #endregion
                var nodes = appresXml.GetElementsByTagName("UUID", types.Enums.NS_cbc);
                var appres = new types.Belge(envelope);
                appres.BelgeEttn = nodes.Item(0).InnerText;
                nodes = appresXml.GetElementsByTagName("ID", types.Enums.NS_cbc);
                appres.BelgeId = nodes.Item(0).InnerText;
                appres.BelgeTarihi = xmlDoc.SelectSingleNode("//cbc:IssueDate", NSManager).InnerText;
                appres.BelgeProfili = Enums.InvProfile.YOK;
                appres.BelgeTipi = Enums.InvType.YOK;
                appres.Referans = xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:DocumentReference/cbc:ID", NSManager).InnerText;
                appres.ReferansKodu = xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:ResponseCode", NSManager).InnerText;
                appres.ReferansMetin = xmlDoc.SelectSingleNode("//cac:DocumentResponse/cac:LineResponse/cac:Response/cbc:Description", NSManager).InnerText;
                wcf.Helper.BelgeEkle(Enums.Yon.GELEN, Enums.ElmntType.APPLICATIONRESPONSE, appres);
                wcf.Helper.ZarfDurumGuncelle(appres);
            }
        }

        private static void ReceiverSenderEnvelope(types.Zarf envelope, XmlDocument xmlDoc)
        {
            try
            {
                switch (envelope.ElmntType)
                {
                    case types.Enums.ElmntType.INVOICE:
                        ReceiverInvoice(envelope, xmlDoc);
                        break;
                    default:
                        throw new NotImplementedException(envelope.ElmntType.ToString());
                }
            }
            catch (Exception)
            {
                wcf.Helper.ZarfDurumGuncelle(envelope.ZarfEttn, Enums.Durum.YENI, "1150", "SCHEMATRON KONTROL SONUCU HATALI");
                wcf.Helper.Log("GİB Test Zarfı Geldi: " + envelope.ZarfEttn);
            }
        }

        private static void ReceiverInvoice(types.Zarf envelope, XmlDocument xmlDoc)
        {
            var invs = xmlDoc.GetElementsByTagName("ElementList");
            if (invs.Count < 1)
                throw new Exception("Belge bulunamadı!");
            var node = xmlDoc.GetElementsByTagName("ElementCount");
            if (node.Count != 1)
                throw new Exception("Belge sayısı uygun değil!");
            int.TryParse(node.Item(0).InnerText, out int i);
            if (invs[0].ChildNodes.Count != i)
                throw new Exception("Belge sayısı eşit değil!");
            foreach (XmlElement inv in invs[0].ChildNodes)
            {
                XmlDocument ftrXml = new XmlDocument();
                ftrXml.LoadXml(inv.OuterXml);
                #region Namespace Manager
                var NSManager = new XmlNamespaceManager(xmlDoc.NameTable);
                NSManager.AddNamespace("cbc", types.Enums.NS_cbc);
                NSManager.AddNamespace("cac", types.Enums.NS_cac);
                #endregion
                var nodes = ftrXml.GetElementsByTagName("UUID", types.Enums.NS_cbc);
                var invoice = new types.Belge(envelope);
                invoice.BelgeEttn = nodes.Item(0).InnerText;
                nodes = ftrXml.GetElementsByTagName("ID", types.Enums.NS_cbc);
                invoice.BelgeId = nodes.Item(0).InnerText;
                invoice.BelgeTarihi = xmlDoc.SelectSingleNode("//cbc:IssueDate", NSManager).InnerText;
                Enum.TryParse<types.Enums.InvType>(ftrXml.GetElementsByTagName("InvoiceTypeCode", types.Enums.NS_cbc).Item(0).InnerText, out types.Enums.InvType invType);
                invoice.BelgeTipi = invType;
                Enum.TryParse<types.Enums.InvProfile>(ftrXml.GetElementsByTagName("ProfileID", types.Enums.NS_cbc).Item(0).InnerText, out types.Enums.InvProfile invProfile);
                invoice.BelgeProfili = invProfile;
                wcf.Helper.BelgeEkle(Enums.Yon.GELEN, Enums.ElmntType.INVOICE, invoice);
            }
        }
    }
}

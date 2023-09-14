using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace com.mkysoft.gib.tester.wcf
{
    [ServiceBehavior]
    public class ReceiverGIB : IReceiverGIB
    {
        private class GibException : Exception
        {
            private int _code;
            public int code { get { return _code; } set { _code = value; } }
            private string _msg;
            public string msg { get { return _msg; } set { _msg = value; } }
            public GibException(int code, string msg)
            {
                _code = code;
                _msg = msg;
            }
        }

        private EFaturaFaultType Err2005 = new EFaturaFaultType() { code = 2005, codeSpecified = true, msg = "SISTEM HATASI" };

        public sendDocumentResponse sendDocument(sendDocument request)
        {
            try
            {
                var tempDir = new DirectoryInfo("envelope-in");
                if (!tempDir.Exists)
                    tempDir.Create();
                File.WriteAllBytes(Path.Combine(tempDir.FullName, request.documentRequest.fileName), request.documentRequest.binaryData.Value);
                if (request.documentRequest.fileName.Length != 40)
                    throw new GibException(2006, "GECERSIZ ZARF ADI");
                if (!helper.Araclar.MD5(request.documentRequest.binaryData.Value).Equals(request.documentRequest.hash, StringComparison.OrdinalIgnoreCase))
                    throw new GibException(2000, "OZET DEGERLER ESIT DEGIL");
                if (!Guid.TryParse(request.documentRequest.fileName.Substring(0, 36), out Guid temp) || request.documentRequest.fileName.Substring(36, 4).ToLower(new System.Globalization.CultureInfo("en-US")) != ".zip")
                    throw new GibException(2006, "GECERSIZ ZARF ADI");

                helper.Gib.Receive(request.documentRequest.fileName, request.documentRequest.binaryData.Value);

                sendDocumentResponse documentResponse = new sendDocumentResponse();
                documentResponse.documentResponse = new documentReturnType();

                documentResponse.documentResponse.msg = "ZARF BAŞARI İLE ALINDI";
                documentResponse.documentResponse.hash = helper.Araclar.MD5(new UTF8Encoding().GetBytes(documentResponse.documentResponse.msg));
                return documentResponse;
            }
            catch (GibException ex)
            {
                wcf.Helper.Log(ex.ToString());
                EFaturaFaultType Err = new EFaturaFaultType();
                Err.code = ex.code;
                Err.codeSpecified = true;
                Err.msg = ex.msg;
                throw new FaultException<EFaturaFaultType>(Err);
            }
            catch (Exception ex)
            {
                wcf.Helper.Log(ex.ToString());
                throw new FaultException<EFaturaFaultType>(Err2005);
            }
        }

        public getApplicationResponseResponse getApplicationResponse(getApplicationResponse request)
        {
            throw new FaultException<EFaturaFaultType>(Err2005);
        }
    }
}

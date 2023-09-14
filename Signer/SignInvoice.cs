using com.mkysoft.helper.extensions;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace com.mkysoft.gib.signer
{
    /// <summary>
    /// Sign invoice with restapi
    /// </summary>
    //public class SignInvoice : ISignature
    // TODO:
    public class SignInvoice
    {
        /// <summary>
        /// XML to be signed or signed XML
        /// </summary>
        public byte[] XmlData { get; private set; }

        /// <summary>
        /// true if the Xml in XmlData is signed
        /// </summary>
        public bool IsSigned { get; private set; }

        /// <summary>
        /// Error message that returns from the rest api call
        /// </summary>
        public string ErrorMsg { get; private set; }

        /// <summary>
        /// Webapi bağlantısını sağlamak için HttpRequestMessageObjesi
        /// </summary>
        protected HttpRequestMessage requestMessage;

        /// <summary>
        /// Webapi çağrısı için HttpClient objesi
        /// </summary>
        protected HttpClient httpClient;

        public SignInvoice(SignRequest request, string _XmlData)
        {
            XmlData = _XmlData.ToByteArray();
            // TODO:
            SignRequest = request;
            request.XmlData = XmlData;
        }
        public SignInvoice(SignRequest request, byte[] _XmlData)
        {
            XmlData = _XmlData;
            SignRequest = request;
            request.XmlData = XmlData;
        }
        public SignInvoice(SignRequest request)
        {
            SignRequest = request;
            XmlData = request.XmlData;
        }
        private SignRequest SignRequest { get; set; }

        /// <summary>
        /// signs the invoice and returns result
        /// </summary>
        /// <returns>Returns true if signed</returns>
        public bool Sign()
        {
            bool isSigned = false;

            Task signAsync = SignAsync();
            signAsync.Wait();

            return isSigned;
        }

        /// <summary>
        /// Sign the invoice Async
        /// </summary>
        /// <returns>nothing. Sets the fields</returns>
        private async Task SignAsync()
        {
            // TODO:
            //httpClient = ESignConnection.GetClient();
            //requestMessage = new HttpRequestMessage(HttpMethod.Post, new ESignConfiguration().ESignAPIUrl + "api/signer/signinvoice");
            //string JSONString = JsonConvert.SerializeObject(SignRequest);

            //requestMessage.Content = new StringContent(JSONString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string encodedData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                XmlData = System.Convert.FromBase64String(encodedData.Trim(new char[] { '\"' }));
                ErrorMsg = "";
                IsSigned = true;
                //httpClient.Dispose();
            }
            else
            {
                ErrorMsg = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                IsSigned = false;
                //httpClient.Dispose();
            }

        }
    }
}

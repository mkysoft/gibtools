using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.mkysoft.gib.tool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnGonder_Click(object sender, EventArgs e)
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

            if (txtGIBServisAdresi.Text.Contains("http://"))
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
            var endpointAddress = new EndpointAddress(txtGIBServisAdresi.Text);

            var docRequest = new GIBService.documentType();
                
            if (rbZarf.Checked)
            {
                Guid guid = new Guid();
                if (Path.GetFileName(fdOpen.FileName).Length == 40 && Guid.TryParse(Path.GetFileName(fdOpen.FileName).Substring(0, 36), out guid) && Path.GetFileName(fdOpen.FileName).Substring(36, 4).ToLower(new System.Globalization.CultureInfo("en-US")) == ".zip")
                    docRequest.fileName = Path.GetFileName(fdOpen.FileName);
                else
                {
                    guid = Guid.NewGuid();
                    docRequest.fileName = guid + ".zip";
                }
                docRequest.binaryData = new GIBService.base64Binary();
                docRequest.binaryData.Value = File.ReadAllBytes(fdOpen.FileName);
                docRequest.hash = Araclar.MD5(docRequest.binaryData.Value);
            }
            else if (rbHataliZarfID.Checked)
            {
                docRequest.fileName = "GİB Tester";
                docRequest.binaryData = new GIBService.base64Binary();
                docRequest.binaryData.Value = new UTF8Encoding().GetBytes("GİB Tester");
                docRequest.hash = null;
            }
            else if (rbHataliOzetDeger.Checked)
            {
                docRequest.fileName = Guid.NewGuid().ToString() + ".zip";
                docRequest.binaryData = new GIBService.base64Binary();
                docRequest.binaryData.Value = new UTF8Encoding().GetBytes("GİB Tester");
                docRequest.hash = Araclar.MD5(new UTF8Encoding().GetBytes("GİB Tester Hatalı"));
            }
            docRequest.binaryData.contentType = "application/x-zip-compressed";
            try
            {
                sendRequest = new GIBService.EFaturaPortTypeClient(customBinding, endpointAddress).sendDocument(docRequest);
                lbLog.Items.Add(sendRequest.msg);
            }
            catch (FaultException<GIBService.EFaturaFaultType> ex)
            {
                lbLog.Items.Add(ex.Detail.code + " : " + ex.Detail.msg);
            }
        }

        private void BtnGozat_Click(object sender, EventArgs e)
        {
            fdOpen.ShowDialog();
        }

        private void RbZarf_CheckedChanged(object sender, EventArgs e)
        {
            btnGozat.Visible = rbZarf.Checked;
        }
    }
}

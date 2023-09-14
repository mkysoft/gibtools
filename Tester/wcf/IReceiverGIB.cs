using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace com.mkysoft.gib.tester.wcf
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(Namespace = "http://gib.gov.tr/vedop3/eFatura")]
    public interface IReceiverGIB
    {

        [OperationContract(Action = "sendDocument")]
        [System.ServiceModel.FaultContractAttribute(typeof(EFaturaFaultType), Action = "sendDocument", Name = "EFaturaFault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        sendDocumentResponse sendDocument(sendDocument request);

        [OperationContract(Action = "getApplicationResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(EFaturaFaultType), Action = "getApplicationResponse", Name = "EFaturaFault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        getApplicationResponseResponse getApplicationResponse(getApplicationResponse request);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura")]
    public partial class EFaturaFaultType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private int codeField;

        private bool codeFieldSpecified;

        private string msgField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public int code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
                this.RaisePropertyChanged("code");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool codeSpecified
        {
            get
            {
                return this.codeFieldSpecified;
            }
            set
            {
                this.codeFieldSpecified = value;
                this.RaisePropertyChanged("codeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string msg
        {
            get
            {
                return this.msgField;
            }
            set
            {
                this.msgField = value;
                this.RaisePropertyChanged("msg");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura")]
    public partial class getAppRespResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string applicationResponseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string applicationResponse
        {
            get
            {
                return this.applicationResponseField;
            }
            set
            {
                this.applicationResponseField = value;
                this.RaisePropertyChanged("applicationResponse");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura")]
    public partial class getAppRespRequestType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string instanceIdentifierField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string instanceIdentifier
        {
            get
            {
                return this.instanceIdentifierField;
            }
            set
            {
                this.instanceIdentifierField = value;
                this.RaisePropertyChanged("instanceIdentifier");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura")]
    public interface EFaturaPortType
    {

        // CODEGEN: Generating message contract since the operation getApplicationResponse is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "getApplicationResponse", ReplyAction = "*")]
        [System.ServiceModel.FaultContractAttribute(typeof(EFaturaFaultType), Action = "getApplicationResponse", Name = "EFaturaFault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        getApplicationResponseResponse getApplicationResponse(getApplicationResponse request);

        // CODEGEN: Generating message contract since the operation sendDocument is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "sendDocument", ReplyAction = "*")]
        [System.ServiceModel.FaultContractAttribute(typeof(EFaturaFaultType), Action = "sendDocument", Name = "EFaturaFault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        sendDocumentResponse sendDocument(sendDocument request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class getApplicationResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura", Order = 0)]
        public getAppRespRequestType getAppRespRequest;

        public getApplicationResponse()
        {
        }

        public getApplicationResponse(getAppRespRequestType getAppRespRequest)
        {
            this.getAppRespRequest = getAppRespRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class getApplicationResponseResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura", Order = 0)]
        public getAppRespResponseType getAppRespResponse;

        public getApplicationResponseResponse()
        {
        }

        public getApplicationResponseResponse(getAppRespResponseType getAppRespResponse)
        {
            this.getAppRespResponse = getAppRespResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura")]
    public partial class documentType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string fileNameField;

        private base64Binary binaryDataField;

        private string hashField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string fileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
                this.RaisePropertyChanged("fileName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public base64Binary binaryData
        {
            get
            {
                return this.binaryDataField;
            }
            set
            {
                this.binaryDataField = value;
                this.RaisePropertyChanged("binaryData");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string hash
        {
            get
            {
                return this.hashField;
            }
            set
            {
                this.hashField = value;
                this.RaisePropertyChanged("hash");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2005/05/xmlmime")]
    public partial class base64Binary : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string contentTypeField;

        private byte[] valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string contentType
        {
            get
            {
                return this.contentTypeField;
            }
            set
            {
                this.contentTypeField = value;
                this.RaisePropertyChanged("contentType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "base64Binary")]
        public byte[] Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura")]
    public partial class documentReturnType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string msgField;

        private string hashField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string msg
        {
            get
            {
                return this.msgField;
            }
            set
            {
                this.msgField = value;
                this.RaisePropertyChanged("msg");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string hash
        {
            get
            {
                return this.hashField;
            }
            set
            {
                this.hashField = value;
                this.RaisePropertyChanged("hash");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class sendDocument
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura", Order = 0)]
        public documentType documentRequest;

        public sendDocument()
        {
        }

        public sendDocument(documentType documentRequest)
        {
            this.documentRequest = documentRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class sendDocumentResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://gib.gov.tr/vedop3/eFatura", Order = 0)]
        public documentReturnType documentResponse;

        public sendDocumentResponse()
        {
        }

        public sendDocumentResponse(documentReturnType documentResponse)
        {
            this.documentResponse = documentResponse;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface EFaturaPortTypeChannel : EFaturaPortType, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EFaturaPortTypeClient : System.ServiceModel.ClientBase<EFaturaPortType>, EFaturaPortType
    {

        public EFaturaPortTypeClient()
        {
        }

        public EFaturaPortTypeClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public EFaturaPortTypeClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public EFaturaPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public EFaturaPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        getApplicationResponseResponse EFaturaPortType.getApplicationResponse(getApplicationResponse request)
        {
            return base.Channel.getApplicationResponse(request);
        }

        public getAppRespResponseType getApplicationResponse(getAppRespRequestType getAppRespRequest)
        {
            getApplicationResponse inValue = new getApplicationResponse();
            inValue.getAppRespRequest = getAppRespRequest;
            getApplicationResponseResponse retVal = ((EFaturaPortType)(this)).getApplicationResponse(inValue);
            return retVal.getAppRespResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        sendDocumentResponse EFaturaPortType.sendDocument(sendDocument request)
        {
            return base.Channel.sendDocument(request);
        }

        public documentReturnType sendDocument(documentType documentRequest)
        {
            sendDocument inValue = new sendDocument();
            inValue.documentRequest = documentRequest;
            sendDocumentResponse retVal = ((EFaturaPortType)(this)).sendDocument(inValue);
            return retVal.documentResponse;
        }
    }
}

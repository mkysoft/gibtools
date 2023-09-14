using System;
using System.Collections.Generic;
using System.Text;

namespace com.mkysoft.gib.signer
{
    /// <summary>
    /// Data to send for sign request
    /// </summary>
    public class SignRequest
    {
        public enums.Device Device { get; set; }
        public enums.SignType SignType { get; set; }
        public Token Token { get; set; }
        public string Pin { get; set; }
        public byte[] XmlData { get; set; }
    }
}

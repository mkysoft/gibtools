using System;
using System.Collections.Generic;
using System.Text;

namespace com.mkysoft.gib.signer.enums
{
    /// <summary>
    /// XML node to use in signing
    /// </summary>
    public enum SignatureElement
    {
        /// <summary>
        /// XML node ds:Signature
        /// </summary>
        dsSignature = 0,
        /// <summary>
        /// XML node ext:ExtensionContent
        /// </summary>
        Invoice = 1,
        /// <summary>
        /// XML node earsiv:baslik
        /// </summary>
        ArchiveReport = 2,
        /// <summary>
        /// XML node edefter:defter
        /// </summary>
        Ledger = 3,
        /// <summary>
        /// XML node edefter:berat
        /// </summary>
        Pattern = 4,
        /// <summary>
        /// XML node oa:Signature
        /// </summary>
        User = 5
    }
}

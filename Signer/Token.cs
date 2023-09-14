using System;
using System.Collections.Generic;
using System.Text;

namespace com.mkysoft.gib.signer
{
    /// <summary>
    /// Token Definition
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Token Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Token serial
        /// </summary>
        public string Serial { get; set; }

        /// <summary>
        /// Slot token is plugged in 
        /// </summary>
        public long Slot { get; set; }

        /// <summary>
        /// Tax Number of token owner
        /// </summary>
        public string TaxNumber { get; set; }
    }
}

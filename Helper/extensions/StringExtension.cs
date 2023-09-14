using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace com.mkysoft.helper.extensions
{
    /// <summary>
    /// Extension methods for string
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Convert string to byte array ın UTF8 mode
        /// </summary>
        /// <param name="Value">string to convert</param>
        /// <returns>byte array</returns>
        public static byte[] ToByteArray(this string Value)
        {
            return Encoding.UTF8.GetBytes(Value);
        }
    }
}

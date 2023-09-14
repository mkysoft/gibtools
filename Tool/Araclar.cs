using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace com.mkysoft.gib.tool
{
    public class Araclar
    {
        public static string MD5(byte[] data)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] hash = md5.ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        public static MemoryStream Compress(MemoryStream datams, string file)
        {
            using (MemoryStream zipms = new MemoryStream())
            {
                ZipStorer zs = ZipStorer.Create(zipms, null);
                zs.AddStream(ZipStorer.Compression.Deflate, file, datams, DateTime.Now, null);
                zs.Close();
                return zipms;
            }
        }
    }
}
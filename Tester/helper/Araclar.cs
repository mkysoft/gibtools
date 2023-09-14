using com.mkysoft.helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace com.mkysoft.gib.tester.helper
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
                ZipStorer zs = ZipStorer.Create(zipms, String.Empty);
                zs.AddStream(ZipStorer.Compression.Deflate, file, datams, DateTime.Now, String.Empty);
                zs.Close();
                return zipms;
            }
        }

        public static byte[] Compress(byte[] data, string file)
        {
            using(var ms = new MemoryStream(data))
            {
                return Compress(ms, file).ToArray();
            }
        }

        internal static byte[] Decompress(byte[] envelope, out string dosyaAdi)
        {
            using (MemoryStream zipms = new MemoryStream(envelope))
            {
                var zs = ZipStorer.Open(zipms, FileAccess.Read);
                var files = zs.ReadCentralDir();
                foreach(var file in files)
                {
                    dosyaAdi = file.FilenameInZip;
                    zs.ExtractFile(file, out byte[] data);
                    return data;
                }
                throw new Exception("Zip içerisinde dosya bulunamadı!");
            }
        }
    }
}
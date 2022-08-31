using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SCG.Cifrado
{
    public class Cifra
    {
        public static string DesEncripta(string cadena, string llave, string vector)
        {
            byte[] data = Convert.FromBase64String(cadena);
            byte[] key = Convert.FromBase64String(llave);
            byte[] iVector = Convert.FromBase64String(vector);

            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            ICryptoTransform transform = rijndaelManaged.CreateDecryptor(key, iVector);
            MemoryStream memoryStream = new MemoryStream(data);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            byte[] bytes = new byte[data.Length];
            cryptoStream.Read(bytes, 0, data.Length);
            cryptoStream.Close();
            memoryStream.Close();

            return Encoding.ASCII.GetString(bytes).TrimEnd('\0');
        }

        public static string Encripta(string cadena, string llave, string vector)
        {
            byte[] data = Encoding.ASCII.GetBytes(cadena);
            byte[] key = Convert.FromBase64String(llave);
            byte[] iVector = Convert.FromBase64String(vector);

            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            ICryptoTransform transform = rijndaelManaged.CreateEncryptor(key, iVector);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return Convert.ToBase64String(bytes);
        }


    }
}

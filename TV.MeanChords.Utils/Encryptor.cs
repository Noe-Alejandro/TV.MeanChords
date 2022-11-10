using System;
using System.Security.Cryptography;
using System.Text;

namespace TV.MeanChords.Utils
{
    public static class Encryptor
    {
        /// <summary>
        /// Clave de encriptación.
        /// </summary>
        private const string _keyEncript = "2SvkNgAOsuiViSXT";

        /// <summary>
        /// Método encargado de desencriptar un texto.
        /// </summary>
        /// <param name="text">Texto a tratar.</param>
        /// <returns>Retorna cadena desencriptada.</returns>
        public static string DecryptString(this string text)
        {
            try
            {
                var cryptoTransform = GetTransform(text, true);
                return Encoding.UTF8.GetString(cryptoTransform);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Método encargado de encriptar un texto.
        /// </summary>
        /// <param name="text">Texto a tratar.</param>
        /// <returns>Retorna cadena encriptada.</returns>
        public static string EncryptString(this string text)
        {
            try
            {
                var cryptoTransform = GetTransform(text);
                return Convert.ToBase64String(cryptoTransform, 0, cryptoTransform.Length);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Método encargado de gestionar la encriptación de un texto.
        /// </summary>
        /// <param name="text">Texto a tratar.</param>
        /// <param name="isDencrypt">Indicador de des-encriptación.</param>
        /// <returns>Retorna el arreglo el bytes del texto.</returns>
        private static byte[] GetTransform(string text, bool isDencrypt = false)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(_keyEncript);
            byte[] textBytes = isDencrypt ? Convert.FromBase64String(text) : Encoding.UTF8.GetBytes(text);

            var tdescProvider = new TripleDESCryptoServiceProvider();
            tdescProvider.Key = keyArray;
            tdescProvider.Mode = CipherMode.ECB;
            tdescProvider.Padding = PaddingMode.PKCS7;

            ICryptoTransform cryptoTransform = isDencrypt ? tdescProvider.CreateDecryptor() : tdescProvider.CreateEncryptor();
            tdescProvider.Clear();
            return cryptoTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace CryptographySymmetrical_H2
{
    public enum Encrypters
    {
        DES,
        TRIPLEDES,
        AES
    }

    class EncryptData
    {
        public static Stopwatch watch;

        public static void EncryptFolder(Encrypters encrypter)
        {

            byte[] key = null;
            byte[] iv = null;

            switch (encrypter)
            {
                case Encrypters.DES:
                    key = GenerateNumbers(8);
                    iv = GenerateNumbers(8);
                    break;
                case Encrypters.TRIPLEDES:
                    key = GenerateNumbers(16);
                    iv = GenerateNumbers(8);
                    break;
                case Encrypters.AES:
                    key = GenerateNumbers(32);
                    iv = GenerateNumbers(16);
                    break;
            }

            EncryptFolder(encrypter, key, iv);
        }

        /// <summary>
        /// Method for encrypting txt files in current directory
        /// </summary>
        /// <param name="encrypter">Method for encryption</param>
        /// <param name="key">The key used to encrypt with</param>
        /// <param name="iv">The iv used to encrypt with</param>
        public static void EncryptFolder(Encrypters encrypter, byte[] key, byte[] iv)
        {
            watch = new Stopwatch();
            watch.Start();
            SymmetricAlgorithm algorithm = null;

            switch (encrypter)
            {
                case Encrypters.DES:
                    algorithm = DESCryptoServiceProvider.Create();
                    break;
                case Encrypters.TRIPLEDES:
                    algorithm = TripleDESCryptoServiceProvider.Create();
                    break;
                case Encrypters.AES:
                    algorithm = AesCryptoServiceProvider.Create();
                    break;
            }

            algorithm.Mode = CipherMode.CBC;
            algorithm.Padding = PaddingMode.PKCS7;

            algorithm.Key = key;
            algorithm.IV = iv;

            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, algorithm.CreateEncryptor(),
                    CryptoStreamMode.Write);

                foreach (string file in Directory.GetFiles(Environment.CurrentDirectory, "*.txt"))
                {
                    byte[] fileText = File.ReadAllBytes(file);

                    cryptoStream.Write(fileText, 0, fileText.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(file, memoryStream.ToArray());

                }

                watch.Stop();
                Console.WriteLine("Stopwatch took: " + watch.ElapsedMilliseconds + "ms to complete \n" +
                    "Remember to save the key and IV somewhere");
                Console.WriteLine("Key:" + Convert.ToBase64String(algorithm.Key) + "\n" +
                    "IV: " + Convert.ToBase64String(algorithm.IV));
            }
        }

        /// <summary>
        /// Method for decrypting data
        /// </summary>
        /// <param name="encrypter">What encryption is used</param>
        /// <param name="message">The message to decrypt</param>
        /// <param name="key">The encryption key</param>
        /// <param name="iv">the encryption iv</param>
        /// <returns>Returns a decrypted byte[]</returns>
        public static void DecryptFolder(Encrypters encrypter, byte[] key, byte[] iv)
        {
            watch = new Stopwatch();
            watch.Start();
            SymmetricAlgorithm algorithm = null;

            switch (encrypter)
            {
                case Encrypters.DES:
                    algorithm = DESCryptoServiceProvider.Create();
                    break;
                case Encrypters.TRIPLEDES:
                    algorithm = TripleDESCryptoServiceProvider.Create();
                    break;
                case Encrypters.AES:
                    algorithm = AesCryptoServiceProvider.Create();
                    break;
            }

            algorithm.Mode = CipherMode.CBC;
            algorithm.Padding = PaddingMode.PKCS7;
            try
            {
                algorithm.Key = key;
                algorithm.IV = iv;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Make sure to use the correct encryption method");
                throw e;
            }

            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, algorithm.CreateDecryptor(),
                    CryptoStreamMode.Write);

                foreach (string file in Directory.GetFiles(Environment.CurrentDirectory, "*.txt"))
                {
                    byte[] fileText = File.ReadAllBytes(file);
                    cryptoStream.Write(fileText, 0, fileText.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(file, memoryStream.ToArray());

                }

                watch.Stop();
                Console.WriteLine("Stopwatch took " + watch.ElapsedMilliseconds + "ms to decrypt");
            }
        }

        public static byte[] GenerateNumbers(int length)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] numbers = new byte[length];
                rng.GetBytes(numbers);

                return numbers;
            }
        }
    }
}

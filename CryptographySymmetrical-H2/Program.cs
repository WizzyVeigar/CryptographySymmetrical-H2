using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographySymmetrical_H2
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //1.krypter eller ikke krypter
                Console.WriteLine("Would you like to encrypt or decrypt?");
                string encOrDec = Console.ReadLine().ToLower();

                //2.Angiv nøgle, ikke angiv nøgle og iv?
                string key = string.Empty;
                string iv = string.Empty;
                Console.WriteLine("Give key and iv yourself? y/n");
                string keyIvInput = Console.ReadLine().ToLower(); ;
                if (keyIvInput == "y")
                {
                    Console.WriteLine("Insert key");
                    key = Console.ReadLine();
                    Console.WriteLine("Insert iv");
                    iv = Console.ReadLine();
                }

                //3.krypteringsform
                for (int i = 0; i < Enum.GetNames(typeof(Encrypters)).Length; i++)
                {
                    Console.WriteLine(i + ". " + Enum.GetNames(typeof(Encrypters))[i]);
                }
                int encryptType = Convert.ToInt32(Console.ReadLine());

                //5.Krypter og udskriv tid

                if (encOrDec == "encrypt" || encOrDec == "1")
                {
                    if (keyIvInput == "y")
                    {
                        EncryptData.EncryptFolder((Encrypters)encryptType, Encoding.UTF8.GetBytes(key),Encoding.UTF8.GetBytes(iv));

                    }
                    else
                    {
                        EncryptData.EncryptFolder((Encrypters)encryptType);
                    }
                }
                else if (encOrDec == "decrypt" || encOrDec == "2")
                {
                    if (keyIvInput == "y")
                    {
                        EncryptData.DecryptFolder((Encrypters)encryptType, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));
                    }
                    else
                    {
                        Console.WriteLine("You cannot decrypt without giving a key and IV");
                    }
                }

                Console.ReadKey();
            }
        }
    }
}

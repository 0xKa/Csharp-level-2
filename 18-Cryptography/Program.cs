using System;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

public class clsHashing
{
    public static string ComputeSHA256(string input)
    {
        // SHA is Secure Hash Algorithm.
        // Create an instance of the SHA-256 algorithm
        using (SHA256 sha256 = SHA256.Create())
        {
            // Compute the hash value from the UTF-8 encoded input string
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Convert the byte array to a lowercase hexadecimal string
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

    }
    public static string ComputeSHA256UsingStringBuilder(string input)
    {
        // using StringBuilder is faster
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

    }

}

public class clsSymmetric
{
    private static byte[] GetValidKey128bit(string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        Array.Resize(ref keyBytes, 16); // Trim or pad to 16 bytes for AES-128
        return keyBytes;
    }
    private static byte[] GetValidKey256bit(string key)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(key)); // Ensure 256-bit key
        }
    }
    public static string Encrypt(string plainText, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GetValidKey256bit(key);
            aesAlg.GenerateIV(); // Generate a random IV

            using (var msEncrypt = new MemoryStream())
            {
                // Write the IV at the beginning of the stream
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (var csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }
    public static string Decrypt(string cipherText, string key)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GetValidKey256bit(key);

            // Extract IV from the beginning of the cipherText
            byte[] iv = new byte[aesAlg.BlockSize / 8];
            Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
            aesAlg.IV = iv;

            using (var msDecrypt = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length))
            using (var csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }


    // using a Fixed-size IV (Encrypt result never changes) // not recommended cause it's weak
    public static string Encrypt_w(string plainText, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            // Set the key and IV for AES encryption
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[aesAlg.BlockSize / 8];


            // Create an encryptor
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


            // Encrypt the data
            using (var msEncrypt = new System.IO.MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }


                // Return the encrypted data as a Base64-encoded string
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }
    public static string Decrypt_w(string cipherText, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            // Set the key and IV for AES decryption
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[aesAlg.BlockSize / 8];


            // Create a decryptor
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);


            // Decrypt the data
            using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(cipherText)))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
            {
                // Read the decrypted data from the StreamReader
                return srDecrypt.ReadToEnd();
            }
        }
    }
}



internal class Program
{
    static void Main(string[] args)
    {

        string text = "Reda Bassam Hilal";
        Console.WriteLine($"Encrypting the text: {text}");



        Console.WriteLine("\nComputing SHA-256:");
        Console.WriteLine(clsHashing.ComputeSHA256(text));
        

        Console.WriteLine("\nComputing SHA-256 using StringBuilder (faster):");
        Console.WriteLine(clsHashing.ComputeSHA256UsingStringBuilder(text));

        string key = "1234567890123456";
        string EncryptedText = clsSymmetric.Encrypt(text, key);
        string DecryptedText = clsSymmetric.Decrypt(EncryptedText, key);

        Console.WriteLine($"\nSymmetric Encryption of the text [{text}]:");
        Console.WriteLine(EncryptedText);
        Console.WriteLine($"\nSymmetric Decryption of the text [{EncryptedText}]:");
        Console.WriteLine(DecryptedText);
        

        Console.ReadLine();
    }
}

/* Hashing [SHA] 
 * The SHA (Secure Hash Algorithm) is a cryptographic hash function that converts input data 
 * into a fixed-size hash value. It is one-way, meaning it cannot be reversed, and collision-resistant,
 * meaning it minimizes the chance of two different inputs producing the same output.
 * 
 * Common Types:
 * SHA-1	--> 160 bits --> 40 hex chars (160/4)
 * SHA-256	--> 256 bits --> 64 hex chars (256/4)
 * SHA-512	--> 512 bits --> 128 hex chars (512/4)
 * 
 */

/* Symmetric Encryption 
 * Symmetric encryption is a type of encryption where the same key is used 
 * for both encrypting and decrypting the data. 
 * This means that the sender and the receiver must both have access 
 * to the same secret key in order to securely communicate.
 * 
 * Common Symmetric Encryption Algorithms:
 * AES (Advanced Encryption Standard) → Most widely used today.
 * DES (Data Encryption Standard) → Older and now considered weak.
 * 3DES (Triple DES) → More secure than DES but slower than AES.
 * Blowfish & Twofish → Alternative algorithms to AES.
 * 
 * AES supports three key sizes: 128-bit, 192-bit, and 256-bit.
 * 
 * 128-bit Key:
 *      The key is 128 bits long, which means it has 2^128 possible combinations.
 *      This is considered strong encryption and is widely used for many secure applications.
 * 192-bit Key:
 *      The key is 192 bits long, providing a higher level of security than 128-bit keys.
 *      While it offers increased security, 192-bit keys are less commonly used than 128-bit and 256-bit keys.
 * 256-bit Key:
 *      The key is 256 bits long, providing the highest level of security among the three key sizes.
 *      AES with a 256-bit key is often used in situations where maximum security is required.
 */

using System;
using System.IO;
using System.Security.Cryptography;
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

    public static void EncryptFile(string filePath, string key)
    {
        string encryptedFilePath = filePath + ".enc"; // Output file

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GetValidKey256bit(key);  // Ensure the key is 32 bytes for AES-256
            aesAlg.GenerateIV();  // Generate a random IV

            using (FileStream fsOut = new FileStream(encryptedFilePath, FileMode.Create))
            {
                // Write IV to the beginning of the file
                fsOut.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (CryptoStream csEncrypt = new CryptoStream(fsOut, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                using (FileStream fsIn = new FileStream(filePath, FileMode.Open))
                {
                    fsIn.CopyTo(csEncrypt); // Encrypt and write to the new file
                }
            }
        }
        Console.WriteLine($"File encrypted: {encryptedFilePath}");
    }
    public static void DecryptFile(string encryptedFilePath, string key)
    {
        string decryptedFilePath = GetDecryptedFilePath(encryptedFilePath);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = GetValidKey256bit(key);

            using (FileStream fsIn = new FileStream(encryptedFilePath, FileMode.Open))
            {
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                fsIn.Read(iv, 0, iv.Length); // Read the IV from the file

                aesAlg.IV = iv;

                using (CryptoStream csDecrypt = new CryptoStream(fsIn, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                using (FileStream fsOut = new FileStream(decryptedFilePath, FileMode.Create))
                {
                    csDecrypt.CopyTo(fsOut); // Decrypt and write to the output file
                }
            }
        }
        Console.WriteLine($"File decrypted: {decryptedFilePath}");
    }
    private static string GetDecryptedFilePath(string encryptedFilePath)
    {
        string originalFilePath = encryptedFilePath.Replace(".enc", ""); // Remove .enc
        string directory = Path.GetDirectoryName(originalFilePath);
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFilePath);
        string extension = Path.GetExtension(originalFilePath);

        return Path.Combine(directory, $"{fileNameWithoutExt}.dec{extension}"); // Append .dec before extension
    }

    // using a Fixed-size IV (Encrypt result never changes) // not recommended cause it's weak
    public static string Encrypt_(string plainText, string key)
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
    public static string Decrypt_(string cipherText, string key)
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

public class clsAsymmetric
{
    public static string Encrypt(string plainText, string publicKey)
    {
        try
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);


                byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
                return Convert.ToBase64String(encryptedData);
            }
        }
        catch (CryptographicException ex)
        {
            Console.WriteLine($"Encryption error: {ex.Message}");
            throw; // Rethrow the exception to be caught in the Main method
        }
    }
    public static string Decrypt(string cipherText, string privateKey)
    {
        try
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);


                byte[] encryptedData = Convert.FromBase64String(cipherText);
                byte[] decryptedData = rsa.Decrypt(encryptedData, false);


                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        catch (CryptographicException ex)
        {
            Console.WriteLine($"Decryption error: {ex.Message}");
            throw; // Rethrow the exception to be caught in the Main method
        }
    }

}

internal class Program
{
    private static string text = "Reda Bassam Hilal";
   
    private static void HashingEncryption()
    {
        Console.WriteLine($"\n---------------------------------Hashing-----------------------------------------------");
        Console.WriteLine($"\nText: [{text}]:");

        Console.WriteLine("\nComputing SHA-256:");
        Console.WriteLine(clsHashing.ComputeSHA256(text));

        Console.WriteLine("\nComputing SHA-256 using StringBuilder (faster):");
        Console.WriteLine(clsHashing.ComputeSHA256UsingStringBuilder(text));

    }
    private static void SymmetricEncryption()
    {
        string key = "1234567890123456";
        string EncryptedText = clsSymmetric.Encrypt(text, key);
        string DecryptedText = clsSymmetric.Decrypt(EncryptedText, key);

        Console.WriteLine($"\n---------------------------------Symmetric Encryption----------------------------------");
        Console.WriteLine($"\nText: [{text}]:");
        Console.WriteLine($"\nEncrypted Text: [{EncryptedText}]:");
        Console.WriteLine($"\nDecrypted Text: [{DecryptedText}]:");
        Console.WriteLine($"\nKey: [{key}]:");
    }
    private static void AsymmetricEncryption()
    {
        string publicKey = null;
        string privateKey = null;
        try
        {
            // Generate public and private key pair
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                /*
                 When exporting the public key, ToXmlString(false) is used with the argument set 
                 to false to indicate that only the public parameters should be included in the XML string.
                 */
                publicKey = rsa.ToXmlString(includePrivateParameters: false);
                privateKey = rsa.ToXmlString(includePrivateParameters: true);

            }
        }
        catch (CryptographicException ex)
        {
            Console.WriteLine($"Encryption/Decryption error: {ex.Message}");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.ReadKey();
        }


        string EncryptedText = clsAsymmetric.Encrypt(text, publicKey);
        string DecryptedText = clsAsymmetric.Decrypt(EncryptedText, privateKey);


        Console.WriteLine($"\n---------------------------------Asymmetric Encryption---------------------------------");
        Console.WriteLine($"\nText: [{text}]:");
        Console.WriteLine($"\nEncrypted Text: [{EncryptedText}]:");
        Console.WriteLine($"\nDecrypted Text: [{DecryptedText}]:");
        Console.WriteLine($"\nPublic Key: [{publicKey}]:");
        Console.WriteLine($"\nPrivate Key: [{privateKey}]:");


    }
    private static void FilesEncryption()
    {
        Console.WriteLine($"\n---------------------------------File Encryption---------------------------------");
        string key = "030291";
        string filepath = @"..\..\MyImage.jpg";


        clsSymmetric.EncryptFile(filepath, key);
        clsSymmetric.DecryptFile(filepath + ".enc", key);
    }

    static void Main(string[] args)
    {
        Console.WriteLine($"Encrypting the text: {text}");

        HashingEncryption();

        SymmetricEncryption();

        AsymmetricEncryption();

        FilesEncryption();

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

/* Asymmetric Encryption 
 * (also known as public-key cryptography) is a type of encryption that uses two different keys:

 * 1 - Public Key – Used for encryption (can be shared openly).
 * 2 - Private Key – Used for decryption (must be kept secret).
 * This is different from symmetric encryption (e.g., AES), which uses the same key for both encryption and decryption.
 * 
 *  Common Asymmetric Encryption Algorithms
 * 1 - RSA – Most widely used for secure data transmission.
 * 2 - ECC (Elliptic Curve Cryptography) – Faster and more secure at smaller key sizes.
 * 3 - Diffie-Hellman – Used for key exchange rather than direct encryption.
 * 4 - DSA (Digital Signature Algorithm) – Used for signing messages.
 * 
 * 🔹 Key Features of Asymmetric Encryption
 * ✅ Secure Communication – Even if the public key is shared, only the private key can decrypt.
 * ✅ No Need to Share Secret Keys – Unlike symmetric encryption, no need to exchange private keys.
 * ✅ Used in SSL/TLS, Digital Signatures, and Cryptocurrencies – Ensures secure transactions and authentication.

 * 🔴 Downsides
 * ❌ Slower than Symmetric Encryption – Asymmetric encryption requires more computation.
 * ❌ Longer Key Sizes Needed for Security – RSA commonly uses 2048-bit or 4096-bit keys for strong security.
 * 
 */

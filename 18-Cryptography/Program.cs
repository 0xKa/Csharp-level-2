using System;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

public class clsHashing
{
    public static string ComputeSHA256(string input)
    {
        // SHA is Secutred Hash Algorithm.
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
internal class Program
{
    static void Main(string[] args)
    {

        string text = "Reda";
        Console.WriteLine($"Encrypting the text: {text}");



        Console.WriteLine("\nComputing SHA-256:");
        Console.WriteLine(clsHashing.ComputeSHA256(text));
        

        Console.WriteLine("\nComputing SHA-256 using StringBuilder (faster):");
        Console.WriteLine(clsHashing.ComputeSHA256UsingStringBuilder(text));


        
        Console.ReadLine();
    }
}

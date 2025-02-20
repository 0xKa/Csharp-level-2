using System;
using screen = System.Console;
using static System.Console;
using System.IO;


// [using] keyword is used for:
// 1- Include/Import Library => using System; 
// 2- Create an Alias => using screen = System.Console;
// 3- Static Directive => using static System.Console;
// 4- Resource Management -> used for closing database, file stream, etc... connections

namespace _8_Using_Statement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //-1
            Console.WriteLine("Hello");

            //-2
            screen.WriteLine("Hello");


            //-3
            WriteLine("Hello");

            //-4
            using (StreamWriter writer = new StreamWriter("example.txt"))
            {
                writer.WriteLine("Hello, C#!");
                writer.Flush(); // Ensure data is written before reading
            }
            using (StreamReader reader = new StreamReader("example.txt"))
            {
                string content = reader.ReadToEnd();
                Console.WriteLine($"File Content: {content}");
            } 
            // Both writer and reader are disposed ✅

            System.Console.ReadKey();
        }
    }
}

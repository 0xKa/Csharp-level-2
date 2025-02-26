using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;


namespace _14_Windows_Registry
{
    public class WinRegistry
    { 
        public static void WritetoRegistry(string keyPath, string valueName, string valueData)
        {

            try
            {
                Registry.SetValue(keyPath, valueName, valueData, RegistryValueKind.String);

                Console.WriteLine($"Value {valueName} successfully written to the Registry.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        public static string ReadFromRegistry(string keyPath, string valueName) 
        {

            string result = null;

            try
            {
                result = Registry.GetValue(keyPath, valueName, null) as string;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return result; 

        }

    }


    internal class Program
    {
        static void Main(string[] args)
        {

            // Specify the Registry key and path
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\zSampleKey";
            string valueName = "Sample Name 2 :]";
            string valueData = "Sample Data :)";
            
            WinRegistry.WritetoRegistry(keyPath, valueName, valueData);

            Console.WriteLine( WinRegistry.ReadFromRegistry(keyPath, valueName) );

            Console.ReadLine();
        }
    }
}

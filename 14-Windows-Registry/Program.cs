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

        public static void DeleteFromRegistry(string keyPath, string valueName)
        {
            try
            {
                // Ensure we're using a relative path under HKCU
                string relativeKeyPath = keyPath.Replace(@"HKEY_CURRENT_USER\", "");

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(relativeKeyPath, writable: true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(valueName, throwOnMissingValue: false);
                        Console.WriteLine($"Value '{valueName}' deleted successfully from '{keyPath}'.");
                    }
                    else
                    {
                        Console.WriteLine($"Registry key '{keyPath}' not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {


            // Specify the Registry key and path
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\zSampleKey";
            string valueName = "sample___name";
            string valueData = "sample___date";


            WinRegistry.WritetoRegistry(keyPath, valueName,valueData);
            Console.WriteLine( WinRegistry.ReadFromRegistry(keyPath, valueName) );
            WinRegistry.DeleteFromRegistry(keyPath, valueName);

            // on Local Machine //
            //string keyPath_local = @"HKEY_LOCAL_MACHINE\SOFTWARE\zSampleKeyLocal"; // on local machine, windows will save it in [WOW6432Node] folder
            //string valueName_local = "MyName12341234";
            //string valueData_local = "Sample Data for local machine:)";
            
            //WinRegistry.WritetoRegistry(keyPath_local, valueName_local, valueData_local);
            //Console.WriteLine( WinRegistry.ReadFromRegistry(keyPath_local, valueName_local) );


            // note: to write on local machine, the application needs admin permissions
            // you can achiave that by running visual studio as admin,
            // or by adding app.manifest file to the project to ask for permissions

            Console.ReadLine();
        }
    }
}

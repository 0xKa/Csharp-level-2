using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace _16_App.config
{
    internal class Program
    {
        // app.config is an xml file
        // in the App.config file we can any configurations of the app, like Connection string

        //note: after building the app, a new file will be created to replace app.config, in the bin directory

        static void Main(string[] args)
        {

            string ConnectionString1 = ConfigurationManager.AppSettings["ConnectionString"];
            string LogLevel = ConfigurationManager.AppSettings["LogLevel"];
            string koko = ConfigurationManager.AppSettings["koko"];
            string ConnectionString2 = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

            Console.WriteLine(ConnectionString1);
            Console.WriteLine(ConnectionString2);
            Console.WriteLine(LogLevel);
            Console.WriteLine(koko);
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Program
{
    public class Logger
    {
        public delegate void LogAction(string message);
        private LogAction _LogAction;

        public Logger(LogAction action) 
        {
            _LogAction = action;
        }

        public void Log(string message)
        {
            Console.WriteLine($"\n\nThis massage will be logged -> [{message}]");
            _LogAction?.Invoke(message);
        }
    }

    public static void LogToConsoleScreen(string message)
    {
        Console.WriteLine(message);
    }

    public static void LogToFile(string message)
    {
        using (StreamWriter writer = new StreamWriter("LogExample.txt", append: true))
        {
            writer.WriteLine(message);
        }
    }

    public static void Main(string[] args)
    {
        Logger ScreenLogger = new Logger(LogToConsoleScreen);
        Logger FileLogger = new Logger(LogToFile);

        ScreenLogger.Log("Screen Test Massage");
        FileLogger.Log("File Text Massage");


        Console.Read();
    }
}


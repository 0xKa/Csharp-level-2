using System;
using System.Diagnostics;

namespace _15_Event_Viewer_Logging
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Log to Event Log //

            // Specify the source name for the event log
            string SourceName = "MyApplication2";

            // Create the event source if it does not exist
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
                Console.WriteLine("Event source created.");
            }

            EventLog.WriteEntry(SourceName, "This is Information Log :)", EventLogEntryType.Information);
            EventLog.WriteEntry(SourceName, "This is Warning Log :)", EventLogEntryType.Warning);
            EventLog.WriteEntry(SourceName, "This is Error Log :)", EventLogEntryType.Error);
            EventLog.WriteEntry(SourceName, "This is Failure Audit Log :)", EventLogEntryType.FailureAudit);
            EventLog.WriteEntry(SourceName, "This is Success Audit Log :)", EventLogEntryType.SuccessAudit);
             

            // Read Event Log //
            //EventLog eventLog = new EventLog("Application");

            //foreach (EventLogEntry entry in eventLog.Entries)
            //{
            //    Console.WriteLine($"[{entry.EntryType}] {entry.TimeGenerated}: {entry.Message}");
            //}

            Console.Read();
        }
    }
}

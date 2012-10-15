using System;
using System.Diagnostics;

namespace DistributedDeployment
{
    internal interface ILog
    {
        void Debug(string msg);

        void Error(string msg, Exception ex);
    }

    public class ConsoleLog : ILog 
    {
        public void Debug(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Error(string msg, Exception ex)
        {
            Console.WriteLine(msg);
            Console.WriteLine(ex.Message);
        }
    }

    public class EventLogger : ILog
    {
        public void Debug(string msg)
        {
            WriteEntry(msg, EventLogEntryType.Information);
        }

        public void Error(string msg, Exception ex)
        {
            WriteEntry(msg + "\n" + ex.StackTrace, EventLogEntryType.Error);
        }

        private static void WriteEntry(string msg, EventLogEntryType type)
        {
            var cs = "DistributedDeployment";
            if (!EventLog.SourceExists(cs))
                EventLog.CreateEventSource(cs, "Application");

            EventLog.WriteEntry(cs, msg, type);
        }
    }
}

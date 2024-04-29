using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Tek_Coding_Exercise
{
    class Logger : ILogger
    {
        private static string logFile = ConfigurationManager.AppSettings["LogFile"];

        public void LogError(string message)
        {
            File.AppendAllLines(logFile, new[] { "Error : " + message });
        }

        public void LogInformation(string message)
        {
            File.AppendAllLines(logFile, new[] { "Information : " + message });
        }
    }
}

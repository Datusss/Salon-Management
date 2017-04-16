using SM.Utilities.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Utilities
{
    public enum LogFile
    {
        NSW,
        ClassifierService,
        ScraperService,
        UnauthorizedActions,
        GoogleBot
    }

    public class LogWrapper
    {
        private string logPath = string.Empty;

        public LogWrapper(string logFolder, LogFile logfile)
        {
            logPath = System.IO.Path.Combine(logFolder, logfile.ToString() + ".log");
        }

        public bool Log(Exception ex, params object[] parameters)
        {
            return Logger.Log(logPath, ex, parameters);
        }

        public bool Log(string msg, params object[] parameters)
        {
            return Logger.Log(logPath, msg, parameters);
        }
    }
}

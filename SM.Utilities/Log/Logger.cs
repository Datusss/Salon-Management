using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 

namespace SM.Utilities.Log
{
    /// <summary>
    /// Logger is used for creating a customized error log files.
    /// </summary>
    public class Logger
    {
        #region Members
        private static StreamWriter sw = null;
        #endregion

        #region Methods
        #region Public methods
        /// <summary>
        /// Write the log entry to customized text-based text file.
        /// </summary>
        /// <returns>false if problem persists.</returns>
        public static bool Log(string strLogFilePath, Exception objException, params object[] debugParams)
        {
            if (!InitStream(strLogFilePath))
                return false;

            // Write the error log to that text file
            return WriteErrorLog(strLogFilePath, objException, debugParams);
        }

        public static bool Log(string strLogFilePath, string message, params object[] debugParams)
        {
            if (!InitStream(strLogFilePath))
                return false;

            // Write the error log to that text file
            return WriteLog(strLogFilePath, message, debugParams);
        }
        #endregion

        #region Private methods
        private static bool InitStream(string strLogFilePath)
        {
            if (string.IsNullOrEmpty(strLogFilePath))
            {
                throw new System.IO.IOException("Empty file path is not allowed.");
            }

            if (!File.Exists(strLogFilePath))
            {
                if (!CheckDirectory(strLogFilePath))
                    return false;

                // Create log file if it doesnot exist.
                FileStream fs = new FileStream(strLogFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Close();
            }

            return true;
        }

        /// <summary>
        /// Write information to the text file.
        /// </summary>
        /// <returns>false if problem persists</returns>
        private static bool WriteErrorLog(string strPathName, Exception objException, params object[] debugParams)
        {
            try
            {
                sw = new StreamWriter(strPathName, true);
                lock (sw)
                {
                    try
                    {
                        sw.WriteLine("Source      : " + objException.Source.ToString().Trim());
                    }
                    catch { }
                    try
                    {
                        sw.WriteLine("Method      : " + objException.TargetSite.Name.ToString());
                    }
                    catch { }
                    sw.WriteLine("Date        : " + DateTime.Now.ToShortDateString());
                    sw.WriteLine("Time        : " + DateTime.Now.ToLongTimeString());
                    //sw.WriteLine("Computer    : " + Dns.GetHostName().ToString());
                    sw.WriteLine("Error       : " + objException.Message.ToString().Trim());
                    sw.WriteLine("Stack Trace : " + objException.StackTrace.ToString().Trim());

                    if ((debugParams != null) && (debugParams.Length > 0))
                    {
                        sw.WriteLine("Debug Info  : ");
                        foreach (object obj in debugParams)
                        {
                            sw.WriteLine("      - " + obj.ToString());
                        }
                    }

                    sw.WriteLine("^^-------------------------------------------------------------------^^");
                    sw.Flush();
                    sw.Close();
                }

                return true;
            }
            catch (System.IO.IOException ex)
            {
                // File could be in used. Retry to write log into a temporary log file.
                FileInfo logFile = new FileInfo(strPathName);
                string uniqueTempLogFile = Path.Combine(logFile.DirectoryName, UniqueValueGenerator.GetUniqueString() + logFile.Name);
                return WriteErrorLog(uniqueTempLogFile, objException, debugParams);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Write information to the text file.
        /// </summary>
        /// <returns>false if problem persists</returns>
        private static bool WriteLog(string strPathName, string strMessage, params object[] debugParams)
        {
            try
            {
                sw = new StreamWriter(strPathName, true);
                lock (sw)
                {
                    sw.WriteLine("Date        : " + DateTime.Now.ToShortDateString());
                    sw.WriteLine("Time        : " + DateTime.Now.ToLongTimeString());
                    sw.WriteLine("Message     : " + strMessage);

                    if ((debugParams != null) && (debugParams.Length > 0))
                    {
                        sw.WriteLine("Debug Info  : ");
                        foreach (object obj in debugParams)
                        {
                            sw.WriteLine("      - " + obj.ToString());
                        }
                    }

                    sw.WriteLine("^^-------------------------------------------------------------------^^");
                    sw.Flush();
                    sw.Close();
                }

                return true;
            }
            catch (IOException)
            {
                // File could be in used. Retry to write log into a temporary log file.
                FileInfo logFile = new FileInfo(strPathName);
                string uniqueTempLogFile = Path.Combine(logFile.DirectoryName, UniqueValueGenerator.GetUniqueString() + logFile.Name);
                return WriteLog(uniqueTempLogFile, strMessage, debugParams);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Create a directory if not exists.
        /// </summary>
        /// <param name="strLogPath"></param>
        /// <returns></returns>
        private static bool CheckDirectory(string strLogPath)
        {
            try
            {
                int nFindSlashPos = strLogPath.Trim().LastIndexOf("\\");
                string strDirectoryname = strLogPath.Trim().Substring(0, nFindSlashPos);

                if (!Directory.Exists(strDirectoryname))
                    Directory.CreateDirectory(strDirectoryname);

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Utilities
{
    public class IOHelper
    {
        #region Get methods
        /// <summary>
        /// Get text content of the specified file.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Text content of the specified file.</returns>
        public static string GetFileContent(string filePath)
        {
            StreamReader sReader = null;
            string tmpHTML = string.Empty;

            try
            {
                if (File.Exists(filePath))
                {
                    sReader = new StreamReader(filePath, Encoding.GetEncoding("windows-1252"));
                    tmpHTML = sReader.ReadToEnd();
                }

                return tmpHTML;
            }
            finally
            {
                if (sReader != null)
                {
                    sReader.Close();
                    sReader.Dispose();
                }
            }
        }
        #endregion

        #region Overwrite methods
        /// <summary>
        /// Overwrite a file with specified content.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="content">Content for overwriting.</param>
        /// <returns>Boolean value indicates the function implemented successfully or not</returns>
        public static void OverwriteFile(string filePath, string content)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(filePath, false, Encoding.GetEncoding("windows-1252"));
                sw.Write(content);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// Overwrite a file with content from a template file.
        /// </summary>
        /// <param name="overwritenFile">File to be overwritten.</param>
        /// <param name="templateFile">Template file.</param>
        public static void OverwriteTemplateFile(string overwritenFile, string templateFile)
        {
            OverwriteTemplateFile(overwritenFile, templateFile, null);
        }

        /// <summary>
        /// Overwrite a file with content from a template file then replace string values specified in keyValuePairs.
        /// </summary>
        /// <param name="overwritenFile">File to be overwritten.</param>
        /// <param name="templateFile">Template file.</param>
        /// <param name="keyValuePairs">Contains pairs of key/value for replacing.</param>
        public static void OverwriteTemplateFile(string overwritenFile, string templateFile, IDictionary<string, string> keyValuePairs)
        {
            StringBuilder tmpHTML = new StringBuilder(GetFileContent(templateFile));
            if (keyValuePairs != null)
            {
                foreach (KeyValuePair<string, string> kvp in keyValuePairs)
                {
                    tmpHTML.Replace(kvp.Key, kvp.Value);
                }
            }

            OverwriteFile(overwritenFile, tmpHTML.ToString());
        }

        /// <summary>
        /// Create or Overwrite a file with specified content.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="inBuff">Binary content for overwriting.</param>
        /// <returns>Boolean value indicates the function implemented successfully or not</returns>
        public static bool OverwriteBinaryFile(string filePath, byte[] inBuff)
        {
            FileStream fStream = null;
            BinaryWriter bWriter = null;

            try
            {
                RemoveFile(filePath);
                fStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                bWriter = new BinaryWriter(fStream);
                bWriter.Write(inBuff);
                bWriter.Flush();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (bWriter != null)
                {
                    bWriter.Close();
                }

                if (fStream != null)
                {
                    fStream.Close();
                    fStream.Dispose();
                }
            }
        }
        #endregion

        #region Remove methods
        /// <summary>
        /// Delete the specified file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="deleteReadOnly">Indicates the file should be delete even if it is set as read-only.</param>
        /// <returns>false if problem persists</returns>
        public static bool RemoveFile(string path, bool deleteReadOnly)
        {
            try
            {
                if (File.Exists(path))
                {
                    FileInfo file = new FileInfo(path);

                    if (deleteReadOnly)
                    {
                        file.Attributes = FileAttributes.Normal;
                    }

                    file.Delete();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete the specified file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>false if problem persists</returns>
        public static bool RemoveFile(string path)
        {
            return RemoveFile(path, false);
        }
        #endregion

        public static bool CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            try
            {
                if (!destination.Exists)
                {
                    destination.Create();
                }

                FileInfo[] files = source.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.CopyTo(Path.Combine(destination.FullName, file.Name));
                }

                DirectoryInfo[] dirs = source.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    string destinationDir = Path.Combine(destination.FullName, dir.Name);
                    CopyDirectory(dir, new DirectoryInfo(destinationDir));
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static long CalculateDirectorySize(DirectoryInfo directory, bool includeSubdirectories)
        {
            long totalSize = 0;

            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }

            if (includeSubdirectories)
            {
                DirectoryInfo[] dirs = directory.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    totalSize += CalculateDirectorySize(dir, true);
                }
            }

            return totalSize;
        }
    }
}

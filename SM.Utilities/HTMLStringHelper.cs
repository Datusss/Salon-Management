using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SM.Utilities
{
    public class HTMLStringHelper
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public HTMLStringHelper()
        {
        }

        /// <summary>
        /// Constructs a HTMLStringHelper with specific content.
        /// </summary>
        /// <param name="strContent">The parser's content.</param>
        public HTMLStringHelper
          (string strContent)
        {
            Content = strContent;
        }
        #endregion

        #region Properties
        /// <summary>Gets and sets the content to be parsed.</summary>
        public string Content
        {
            get
            {
                return m_strContent;
            }
            set
            {
                m_strContent = value;
                m_strContentLC = m_strContent.ToLower();
                ResetPosition();
            }
        }

        /// <summary>Gets the parser's current position.</summary>
        public int Position
        {
            get
            {
                return m_nIndex;
            }
        }
        #endregion

        #region Static methods
        public static string Sanitize(string html)
        {
            html = html.Replace("_moz_dirty=\"\"", "");
            if (String.IsNullOrEmpty(html)) return html;

            string tagname;
            Match tag;

            // match every HTML tag in the input
            MatchCollection tags = _tags.Matches(html);
            for (int i = tags.Count - 1; i > -1; i--)
            {
                tag = tags[i];
                tagname = tag.Value.ToLowerInvariant();

                if (!(_whitelist.IsMatch(tagname) || _whitelist_a.IsMatch(tagname) || _whitelist_img.IsMatch(tagname)

                    ))
                {
                    // if (!_whitelist_customTags.IsMatch(tagname.Replace("\"","'")))
                    if (!
                        (
                        Regex.IsMatch(tagname.Replace("\"", "'"), @"^<(p|ul|ol|li|span) style=('|"")(.)*('|"")>")
                        ||
                        Regex.IsMatch(tagname.Replace("\"", "'"), @"^</span>")
                        ))
                    {
                        html = html.Remove(tag.Index, tag.Length);
                    }
                }
            }

            return StringHelper.BalanceTags(html);
        }
        /// <summary>
        /// Retrieves the collection of HTML links in a string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <param name="strRootUrl">Root url (may be null).</param>
        /// <param name="documents">Collection of document link strings.</param>
        /// <param name="images">Collection of image link strings.</param>
        public static void GetLinks(string strString, string strRootUrl, ref ArrayList documents, ref ArrayList images)
        {
            // Remove comments and JavaScript and fix links
            strString = HTMLStringHelper.RemoveComments(strString);
            strString = HTMLStringHelper.RemoveScripts(strString);
            HTMLStringHelper parser = new HTMLStringHelper(strString);
            parser.ReplaceEvery("\'", "\"");

            // Set root url
            string rootUrl = "";
            if (strRootUrl != null)
                rootUrl = strRootUrl.Trim();
            if ((rootUrl.Length > 0) && !rootUrl.EndsWith("/"))
                rootUrl += "/";

            // Extract HREF targets
            string strUrl = "";
            parser.ResetPosition();
            while (parser.SkipToEndOfNoCase("href=\""))
            {
                if (parser.ExtractTo("\"", ref strUrl))
                {
                    strUrl = strUrl.Trim();
                    if (strUrl.Length > 0)
                    {
                        if (strUrl.IndexOf("mailto:") == -1)
                        {

                            // Get fully qualified url (best guess)
                            if (!strUrl.StartsWith("http://") && !strUrl.StartsWith("ftp://"))
                            {
                                try
                                {
                                    UriBuilder uriBuilder = new UriBuilder(rootUrl);
                                    uriBuilder.Path = strUrl;
                                    strUrl = uriBuilder.Uri.ToString();
                                }
                                catch (Exception)
                                {
                                    strUrl = "http://" + strUrl;
                                }
                            }

                            // Add url to document list if not already present
                            if (!documents.Contains(strUrl))
                                documents.Add(strUrl);
                        }
                    }
                }
            }

            // Extract SRC targets
            parser.ResetPosition();
            while (parser.SkipToEndOfNoCase("src=\""))
            {
                if (parser.ExtractTo("\"", ref strUrl))
                {
                    strUrl = strUrl.Trim();
                    if (strUrl.Length > 0)
                    {

                        // Get fully qualified url (best guess)
                        if (!strUrl.StartsWith("http://") && !strUrl.StartsWith("ftp://"))
                        {
                            try
                            {
                                UriBuilder uriBuilder = new UriBuilder(rootUrl);
                                uriBuilder.Path = strUrl;
                                strUrl = uriBuilder.Uri.ToString();
                            }
                            catch (Exception)
                            {
                                strUrl = "http://" + strUrl;
                            }
                        }

                        // Add url to images list if not already present
                        if (!images.Contains(strUrl))
                            images.Add(strUrl);
                    }
                }
            }
        }

        #region Overload GetLinks
        //public static void GetLinks(string strString, string strRootUrl,string urlPrefix, ref ArrayList documents,string pattern,string regular)
        //{
        //    // Remove comments and JavaScript and fix links
        //    strString = HTMLStringHelper.RemoveComments(strString);
        //    strString = HTMLStringHelper.RemoveScripts(strString);


        //    // Set root url
        //    string rootUrl = string.Empty;
        //    if (strRootUrl != null)
        //        rootUrl = strRootUrl.Trim();
        //    if ((rootUrl.Length > 0) && !rootUrl.EndsWith("/"))
        //        rootUrl += "/";


        //    // Extract HREF targets
        //    Regex regex = new Regex("href=[\"']?(?<group>[\\w\\d._=&?/;#-]+)[\"']?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //    // get all the matches depending upon the regular expression
        //    MatchCollection mcl = regex.Matches(strString);
        //    string strUrl = string.Empty;
        //    foreach (Match ml in mcl)
        //    {
        //        if (ml.Groups.Count > 1)
        //        {
        //            strUrl = ml.Groups[1].Value.Replace("&amp;", "&");

        //            // Get fully qualified url (best guess)
        //            if (!strUrl.StartsWith("http://"))
        //            {
        //                try
        //                {
        //                    UriBuilder uriBuilder = new UriBuilder(rootUrl);
        //                    uriBuilder.Path = strUrl;
        //                    strUrl = uriBuilder.Uri.ToString();
        //                }
        //                catch (Exception)
        //                {
        //                    strUrl = strUrl.Replace("../", string.Empty);
        //                    if (strUrl.StartsWith("..http"))
        //                        strUrl = strUrl.Substring(2);
        //                    if (!strUrl.StartsWith("http"))
        //                        strUrl = urlPrefix + strUrl;
        //                }
        //            }

        //            // Add url to document list if not already present
        //            if (strUrl.IndexOf(pattern) != -1)
        //            {
        //                if (regular != null && regular.Length > 0)
        //                {
        //                    if (ValidateURL(strUrl, regular))
        //                        if (!documents.Contains(strUrl))
        //                            documents.Add(strUrl);
        //                }
        //                else
        //                    if (!documents.Contains(strUrl))
        //                        documents.Add(strUrl);
        //            }
        //        }
        //    }       
        //}











        #region Overload GetLinkss
        public static void GetLinks(string strString, string strRootUrl, string urlPrefix, ref ArrayList documents, string pattern, string regular)
        {
            // Remove comments and JavaScript and fix links
            strString = HTMLStringHelper.RemoveComments(strString);
            strString = HTMLStringHelper.RemoveScripts(strString);


            // Set root url
            string rootUrl = string.Empty;
            if (strRootUrl != null)
                rootUrl = strRootUrl.Trim();
            if ((rootUrl.Length > 0) && !rootUrl.EndsWith("/"))
                rootUrl += "/";


            // Extract HREF targets
            //Regex regex = new Regex("href=[\"']?(?<group>[\\w\\d._=&?/;#-]+)[\"']?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);            

            //Todo:
            Regex regex = new Regex("href\\s*=\\s*(?<s>['\"]?)(?<group>[^ >]+)\\k<s>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //Regex regex = new Regex("href\\s*=\\s*(?<s>['\"])(?<group>[^>]+?)\\k<s>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // get all the matches depending upon the regular expression
            MatchCollection mcl = regex.Matches(strString);
            string strUrl = string.Empty;

            foreach (Match ml in mcl)
            {
                if (ml.Groups.Count > 1)
                {
                    strUrl = ml.Groups["group"].Value.Replace("&amp;", "&").Replace("\"", "");

                    // Get fully qualified url (best guess)
                    if (!strUrl.StartsWith("http://"))
                    {
                        try
                        {
                            UriBuilder uriBuilder = new UriBuilder(rootUrl);
                            uriBuilder.Path = strUrl;
                            strUrl = uriBuilder.Uri.ToString();
                        }
                        catch (Exception)
                        {
                            strUrl = strUrl.Replace("../", string.Empty);
                            if (strUrl.StartsWith("..http"))
                                strUrl = strUrl.Substring(2);
                            if (!strUrl.StartsWith("http"))
                                strUrl = urlPrefix + strUrl;
                        }
                    }

                    // Add url to document list if not already present
                    if (pattern.Length > 0)
                    {
                        if (strUrl.IndexOf(pattern) != -1)
                        {
                            if (regular != null && regular.Length > 0)
                            {
                                if (ValidateURL(strUrl, regular))
                                    if (!documents.Contains(strUrl))
                                    {
                                        documents.Add(strUrl);
                                    }
                            }
                            else
                                if (!documents.Contains(strUrl))
                                {
                                    documents.Add(strUrl);
                                }
                        }
                    }
                    else
                        if (!documents.Contains(strUrl))
                        {
                            documents.Add(strUrl);
                        }
                }
            }
        }





        #endregion










        /////////////////
        // Static methods
        // validateURL
        public static Boolean ValidateURL(string url, string regular)
        {
            Regex myRegex = new Regex(@regular);
            return myRegex.IsMatch(url);

        }
        #endregion

        /// <summary>
        /// Removes all HTML comments from a string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns>Comment-free version of string.</returns>
        public static string RemoveComments(string strString)
        {
            // Return comment-free version of string
            string strCommentFreeString = "";
            string strSegment = "";
            HTMLStringHelper parser = new HTMLStringHelper(strString);

            while (parser.ExtractTo("<!--", ref strSegment))
            {
                strCommentFreeString += strSegment;
                if (!parser.SkipToEndOf("-->"))
                    return strString;
            }

            parser.ExtractToEnd(ref strSegment);
            strCommentFreeString += strSegment;
            return strCommentFreeString;
        }

        /// <summary>
        /// Returns an unanchored version of a string, i.e. without the enclosing
        /// leftmost &lt;a...&gt; and rightmost &lt;/a&gt; tags.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns>Unanchored version of string.</returns>
        public static string RemoveEnclosingAnchorTag(string strString)
        {
            string strStringLC = strString.ToLower();
            int nStart = strStringLC.IndexOf("<a");
            if (nStart != -1)
            {
                nStart++;
                nStart = strStringLC.IndexOf(">", nStart);
                if (nStart != -1)
                {
                    nStart++;
                    int nEnd = strStringLC.LastIndexOf("</a>");
                    if (nEnd != -1)
                    {
                        string strRet = strString.Substring(nStart, nEnd - nStart);
                        return strRet;
                    }
                }
            }
            return strString;
        }

        /// <summary>
        /// Returns an unquoted version of a string, i.e. without the enclosing
        /// leftmost and rightmost double " characters.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns>Unquoted version of string.</returns>
        public static string RemoveEnclosingQuotes(string strString)
        {
            int nStart = strString.IndexOf("\"");
            if (nStart != -1)
            {
                int nEnd = strString.LastIndexOf("\"");
                if (nEnd > nStart)
                    return strString.Substring(nStart, nEnd - nStart - 1);
            }
            return strString;
        }

        /// <summary>
        /// Returns a version of a string without any HTML tags.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns>Version of string without HTML tags.</returns>
        public static string RemoveHtml(string strString)
        {
            // Do some common case-sensitive replacements
            Hashtable replacements = new Hashtable();
            replacements.Add("&nbsp;", " ");
            replacements.Add("&amp;", "&");
            replacements.Add("&aring;", "");
            replacements.Add("&auml;", "");
            replacements.Add("&eacute;", "");
            replacements.Add("&iacute;", "");
            replacements.Add("&igrave;", "");
            replacements.Add("&ograve;", "");
            replacements.Add("&ouml;", "");
            replacements.Add("&quot;", "\"");
            replacements.Add("&szlig;", "");
            HTMLStringHelper parser = new HTMLStringHelper(strString);
            foreach (string key in replacements.Keys)
            {
                string val = replacements[key] as string;
                if (strString.IndexOf(key) != -1)
                    parser.ReplaceEveryExact(key, val);
            }

            // Do some sequential replacements
            parser.ReplaceEveryExact("&#0", "&#");
            parser.ReplaceEveryExact("&#39;", "'");
            parser.ReplaceEveryExact("</", " <~/");
            parser.ReplaceEveryExact("<~/", "</");

            // Case-insensitive replacements
            replacements.Clear();
            replacements.Add("<br>", " ");
            replacements.Add("<p>", " ");
            foreach (string key in replacements.Keys)
            {
                string val = replacements[key] as string;
                if (strString.IndexOf(key) != -1)
                    parser.ReplaceEvery(key, val);
            }
            strString = parser.Content;

            // Remove all tags
            string strClean = "";
            int nIndex = 0;
            int nStartTag = 0;
            while ((nStartTag = strString.IndexOf("<", nIndex)) != -1)
            {

                // Extract to start of tag
                string strSubstring = strString.Substring(nIndex, (nStartTag - nIndex));
                strClean += strSubstring;
                nIndex = nStartTag + 1;

                // Skip over tag
                int nEndTag = strString.IndexOf(">", nIndex);
                if (nEndTag == (-1))
                    break;
                nIndex = nEndTag + 1;
            }

            // Gather remaining text
            if (nIndex < strString.Length)
                strClean += strString.Substring(nIndex, strString.Length - nIndex);
            strString = strClean;
            strClean = "";

            // Finally, reduce spaces
            parser.Content = strString;
            parser.ReplaceEveryExact("  ", " ");
            strString = parser.Content.Trim();

            // Return the de-HTMLized string
            return strString;
        }

        /// <summary>
        /// Removes all scripts from a string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns>Version of string without any scripts.</returns>
        public static string RemoveScripts(string strString)
        {
            // Get script-free version of content
            string strStringSansScripts = "";
            string strSegment = "";
            HTMLStringHelper parser = new HTMLStringHelper(strString);

            while (parser.ExtractToNoCase("<script", ref strSegment))
            {
                strStringSansScripts += strSegment;
                if (!parser.SkipToEndOfNoCase("</script>"))
                {
                    parser.Content = strStringSansScripts;
                    return strString;
                }
            }

            parser.ExtractToEnd(ref strSegment);
            strStringSansScripts += strSegment;
            return (strStringSansScripts);
        }
       
        #endregion

        #region Operations

        /// <summary>
        /// Checks if the parser is case-sensitively positioned at the start
        /// of a string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns>
        /// true if the parser is positioned at the start of the string, false
        /// otherwise.
        /// </returns>
        public bool At(string strString)
        {
            if (m_strContent.IndexOf(strString, Position) == Position)
                return (true);
            return (false);
        }

        /// <summary>
        /// Checks if the parser is case-insensitively positioned at the start
        /// of a string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns>
        /// true if the parser is positioned at the start of the string, false
        /// otherwise.
        /// </returns>
        public bool AtNoCase(string strString)
        {
            strString = strString.ToLower();
            if (m_strContentLC.IndexOf(strString, Position) == Position)
                return (true);
            return (false);
        }

        /// <summary>
        /// Extracts the text from the parser's current position to the case-
        /// sensitive start of a string and advances the parser just after the
        /// string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <param name="strExtract">The extracted text.</param>
        /// <returns>true if the parser was advanced, false otherwise.</returns>
        public bool ExtractTo(string strString, ref string strExtract)
        {
            int nPos = m_strContent.IndexOf(strString, Position);
            if (nPos != -1)
            {
                strExtract = m_strContent.Substring(m_nIndex, nPos - m_nIndex);
                m_nIndex = nPos + strString.Length;
                return (true);
            }
            return (false);
        }

        /// <summary>
        /// Extracts the text from the parser's current position to the case-
        /// insensitive start of a string and advances the parser just after the
        /// string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <param name="strExtract">The extracted text.</param>
        /// <returns>true if the parser was advanced, false otherwise.</returns>
        public bool ExtractToNoCase(string strString, ref string strExtract)
        {
            strString = strString.ToLower();
            int nPos = m_strContentLC.IndexOf(strString, Position);
            if (nPos != -1)
            {
                strExtract = m_strContent.Substring(m_nIndex, nPos - m_nIndex);
                m_nIndex = nPos + strString.Length;
                return (true);
            }
            return (false);
        }

        /// <summary>
        /// Extracts the text from the parser's current position to the case-
        /// sensitive start of a string and position's the parser at the start
        /// of the string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <param name="strExtract">The extracted text.</param>
        /// <returns>true if the parser was advanced, false otherwise.</returns>
        public bool ExtractUntil(string strString, ref string strExtract)
        {
            int nPos = m_strContent.IndexOf(strString, Position);
            if (nPos != -1)
            {
                strExtract = m_strContent.Substring(m_nIndex, nPos - m_nIndex);
                m_nIndex = nPos;
                return (true);
            }
            return (false);
        }

        /// <summary>
        /// Extracts the text from the parser's current position to the case-
        /// insensitive start of a string and position's the parser at the start
        /// of the string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <param name="strExtract">The extracted text.</param>
        /// <returns>true if the parser was advanced, false otherwise.</returns>
        public bool ExtractUntilNoCase(string strString, ref string strExtract)
        {
            strString = strString.ToLower();
            int nPos = m_strContentLC.IndexOf(strString, Position);
            if (nPos != -1)
            {
                strExtract = m_strContent.Substring(m_nIndex, nPos - m_nIndex);
                m_nIndex = nPos;
                return (true);
            }
            return (false);
        }

        /// <summary>
        /// Extracts the text from the parser's current position to the end
        /// of its content and does not advance the parser's position.
        /// </summary>
        /// <param name="strExtract">The extracted text.</param>
        public void ExtractToEnd(ref string strExtract)
        {
            strExtract = "";
            if (Position < m_strContent.Length)
            {
                int nRemainLen = m_strContent.Length - Position;
                strExtract = m_strContent.Substring(Position, nRemainLen);
            }
        }

        /// <summary>
        /// Case-insensitively replaces every occurence of a string in the
        /// parser's content with another.
        /// </summary>
        /// <param name="strOccurrence">The occurrence.</param>
        /// <param name="strReplacement">The replacement string.</param>
        /// <returns>The number of occurences replaced.</returns>
        public int ReplaceEvery(string strOccurrence, string strReplacement)
        {
            // Initialize replacement process
            int nReplacements = 0;
            strOccurrence = strOccurrence.ToLower();

            // For every occurence...
            int nOccurrence = m_strContentLC.IndexOf(strOccurrence);
            while (nOccurrence != -1)
            {

                // Create replaced substring
                string strReplacedString = m_strContent.Substring(0, nOccurrence) + strReplacement;

                // Add remaining substring (if any)
                int nStartOfRemainingSubstring = nOccurrence + strOccurrence.Length;
                if (nStartOfRemainingSubstring < m_strContent.Length)
                {
                    string strSecondPart = m_strContent.Substring(nStartOfRemainingSubstring, m_strContent.Length - nStartOfRemainingSubstring);
                    strReplacedString += strSecondPart;
                }

                // Update the original string
                m_strContent = strReplacedString;
                m_strContentLC = m_strContent.ToLower();
                nReplacements++;

                // Find the next occurence
                nOccurrence = m_strContentLC.IndexOf(strOccurrence);
            }
            return (nReplacements);
        }

        /// <summary>
        /// Case sensitive version of replaceEvery()
        /// </summary>
        /// <param name="strOccurrence">The occurrence.</param>
        /// <param name="strReplacement">The replacement string.</param>
        /// <returns>The number of occurences replaced.</returns>
        public int ReplaceEveryExact(string strOccurrence, string strReplacement)
        {
            int nReplacements = 0;
            while (m_strContent.IndexOf(strOccurrence) != -1)
            {
                m_strContent = m_strContent.Replace(strOccurrence, strReplacement);
                nReplacements++;
            }
            m_strContentLC = m_strContent.ToLower();
            return (nReplacements);
        }

        /// <summary>
        /// Resets the parser's position to the start of the content.
        /// </summary>
        public void ResetPosition()
        {
            m_nIndex = 0;
        }

        /// <summary>
        /// Advances the parser's position to the start of the next case-sensitive
        /// occurence of a string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns>true if the parser's position was advanced, false otherwise.</returns>
        public bool SkipToStartOf(string strString)
        {
            bool bStatus = SeekTo(strString, false, false);
            return (bStatus);
        }

        /// <summary>
        /// Advances the parser's position to the start of the next case-insensitive
        /// occurence of a string.
        /// </summary>
        /// <param name="strText">The string.</param>
        /// <returns>true if the parser's position was advanced, false otherwise.</returns>
        public bool SkipToStartOfNoCase(string strText)
        {
            bool bStatus = SeekTo(strText, true, false);
            return (bStatus);
        }

        /// <summary>
        /// Advances the parser's position to the end of the next case-sensitive
        /// occurence of a string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <returns>true if the parser's position was advanced, false otherwise.</returns>
        public bool SkipToEndOf(string strString)
        {
            bool bStatus = SeekTo(strString, false, true);
            return (bStatus);
        }

        /// <summary>
        /// Advances the parser's position to the end of the next case-insensitive
        /// occurence of a string.
        /// </summary>
        /// <param name="strText">The string.</param>
        /// <returns>true if the parser's position was advanced, false otherwise.</returns>
        public bool SkipToEndOfNoCase(string strText)
        {
            bool bStatus = SeekTo(strText, true, true);
            return (bStatus);
        }
        #endregion

        #region Private
        #region Implementation (members)
        /// <summary>Content to be parsed.</summary>
        string m_strContent = "";

        /// <summary>Lower-cased version of content to be parsed.</summary>
        string m_strContentLC = "";

        /// <summary>Current position in content.</summary>
        int m_nIndex = 0;

        private static Regex _tags = new Regex("<[^>]*(>|$)",
            RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        private static Regex _whitelist = new Regex(@"
            ^</?(b(lockquote)?|code|d(d|t|l|el)|em|h(1|2|3)|i|kbd|li|ol|p(re)?|s(ub|up|trong|trike)?|ul)>$|
            ^<(b|h)r\s?/?>$",
            RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        private static Regex _whitelist_a = new Regex(@"
            ^<a\s
            href=""(\#\d+|(https?|ftp)://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+)""
            (\stitle=""[^""<>]+"")?\s?>$|
            ^</a>$",
            RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        private static Regex _whitelist_img = new Regex(@"
            ^<img\s
            src=""https?://[-a-z0-9+&@#/%?=~_|!:,.;\(\)]+""
            (\swidth=""\d{1,3}"")?
            (\sheight=""\d{1,3}"")?
            (\salt=""[^""<>]*"")?
            (\stitle=""[^""<>]*"")?
            \s?/?>$",
            RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        private static Regex _whitelist_customTags = new Regex(@"^<(p|ul|ol|li) style='margin-left: 40px;'>",
            RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Implementation (methods)

        /// <summary>
        /// Advances the parser's position to the next occurence of a string.
        /// </summary>
        /// <param name="strString">The string.</param>
        /// <param name="bNoCase">Flag: perform a case-insensitive search.</param>
        /// <param name="bPositionAfter">Flag: position parser just after string.</param>
        /// <returns></returns>
        bool SeekTo(string strString, bool bNoCase, bool bPositionAfter)
        {
            if (Position < m_strContent.Length)
            {

                // Find the start of the string - return if not found
                int nNewIndex = 0;
                if (bNoCase)
                {
                    strString = strString.ToLower();
                    nNewIndex = m_strContentLC.IndexOf(strString, Position);
                }
                else
                {
                    nNewIndex = m_strContent.IndexOf(strString, Position);
                }
                if (nNewIndex == -1)
                    return (false);

                // Position the parser
                m_nIndex = nNewIndex;
                if (bPositionAfter)
                    m_nIndex += strString.Length;
                return (true);
            }
            return (false);
        }

        #endregion
        #endregion
    }
}

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SM.Utilities;
using System.Collections.Generic;
using System.Text;

namespace SM.Utilities
{
    public static class StringHelper
    {
        private static StringReplacer _whiteSpaceRemover;
        private static Regex _namedtags = new Regex(@"</?(?<tagname>\w+)[^>]*(\s|$|>)", RegexOptions.Singleline | RegexOptions.ExplicitCapture);
        public static bool IsNullOrWhiteSpace(string s)
        {
            return s == null || "".Equals(s.Trim()) ? true : false;
        }


        public static string BalanceTags(string html, StringCollection ignoreTags)
        {
            string tagname = string.Empty;
            string tag = string.Empty;
            int match = 0;

            MatchCollection tags = _namedtags.Matches(html);
            bool[] tagpaired = new bool[tags.Count];

            // loop through matched tags in reverse order
            for (int i = tags.Count - 1; i > -1; i--)
            {
                tagname = tags[i].Groups["tagname"].Value.ToLower();
                tag = tags[i].Value;
                match = -1;

                // skip any tags in our ignore list; assume they're self-closed
                if (!tagpaired[i] && !ignoreTags.Contains(tagname))
                {
                    if (tag.StartsWith("</"))
                    {
                        // if searching backwards, look for opening tags
                        for (int j = i - 1; j > -1; j--)
                        {
                            if (!tagpaired[j] && tags[j].Value.ToLower().StartsWith("<" + tagname))
                            {
                                match = j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // if searching forwards, look for closing tags
                        for (int j = i + 1; j < tags.Count; j++)
                        {
                            if (!tagpaired[j] && tags[j].Value.ToLower().StartsWith("</" + tagname))
                            {
                                match = j;
                                break;
                            }
                        }
                    }

                    if (match > -1)
                    {
                        // found matching opening/closing tag
                        tagpaired[match] = true;
                        tagpaired[i] = true;
                    }
                    else
                    {
                        // no matching opening/closing tag found -- remove this tag!
                        html = html.Remove(tags[i].Index, tags[i].Length);
                        tagpaired[i] = true;
                        // System.Diagnostics.Debug.WriteLine("unbalanced tag removed: " + tags[i]);
                    }
                }
            }

            return html;
        }

        public static StringCollection ExtractBlocks(string content, string startTemplate, string startItemTemplate, string endItemTemplate)
        {
            var strings = new StringCollection();
            var index = content.IndexOf(startTemplate, StringComparison.Ordinal);
            var num2 = 0;
            while ((index >= 0) && (num2 >= 0))
            {
                index = content.IndexOf(startItemTemplate, (int)(index + 1), System.StringComparison.Ordinal);
                num2 = content.IndexOf(endItemTemplate, (int)(index + startItemTemplate.Length), System.StringComparison.Ordinal);
                if ((index >= 0) && (num2 >= 0))
                {
                    strings.Add(content.Substring(index + startItemTemplate.Length, (num2 - index) - startItemTemplate.Length));
                }
            }
            return strings;
        }

        public static string ExtractTextFromHtml(string html)
        {
            return ExtractTextFromHtml(html, false);
        }

        public static string ExtractTextFromHtml(string html, bool trimWhiteSpace)
        {
            string str = new Regex("<!*[^<>]*>", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(html, string.Empty);
            if (trimWhiteSpace)
            {
                return TrimWhiteSpace(HtmlEncoder.DecodeValue(str));
            }
            return HtmlEncoder.DecodeValue(str);
        }

        public static string GetStringBetween(string startTag, string content, string endTag)
        {
            if ((!string.IsNullOrEmpty(startTag) && !string.IsNullOrEmpty(content)) && !string.IsNullOrEmpty(endTag))
            {
                int index = content.IndexOf(startTag, System.StringComparison.Ordinal);
                int num2 = content.IndexOf(endTag, (int)(index + startTag.Length), System.StringComparison.Ordinal);
                if ((index >= 0) && (num2 >= 0))
                {
                    return content.Substring(index + startTag.Length, (num2 - index) - startTag.Length).Trim();
                }
            }
            return string.Empty;
        }

        public static string GetStringByGroup(string input, string expression)
        {
            Regex regex = new Regex(expression, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return regex.Match(input).Groups["group"].Value.Trim();
        }

        public static StringCollection GetStringListByGroup(string input, string expression, bool distinct)
        {
            if (string.IsNullOrEmpty(input)) return null;
            StringCollection sc = new StringCollection();

            Regex r = new Regex(expression, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection mc = r.Matches(input);
            foreach (Match m in mc)
            {
                if (distinct)
                {
                    if (!sc.Contains(m.Groups["group"].Value.Trim()))
                        sc.Add(m.Groups["group"].Value.Trim());
                }
                else
                {
                    sc.Add(m.Groups["group"].Value.Trim());
                }
            }

            return sc;
        }

        public static string RemoveStringBetween(string startTag, string endTag, string content)
        {
            string str = GetStringBetween(startTag, endTag, content);
            if (str != string.Empty)
            {
                return StringReplacer.Replace(content, startTag + str + endTag, startTag + endTag);
            }
            return content;
        }

        public static string Reverse(string str)
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public static string[] Split(string input, string delimiter)
        {
            return Regex.Split(input, Regex.Escape(delimiter));
        }

        public static string Trim(string input)
        {
            if (input == null)
            {
                input = "";
            }
            return input.Trim();
        }

        public static string Trim(HtmlInputText txtInput)
        {
            return txtInput.Value.Trim();
        }

        public static string Trim(HtmlSelect lsInput)
        {
            if (lsInput.Multiple)
            {
                throw new Exception("Cannot process with Multiple Selection mode");
            }
            return lsInput.Value.Trim();
        }

        public static string Trim(DropDownList ddlInput)
        {
            return ddlInput.SelectedValue.Trim();
        }

        public static string Trim(ListBox lbInput)
        {
            if (lbInput.SelectionMode == ListSelectionMode.Multiple)
            {
                throw new Exception("Cannot process with Multiple Selection mode");
            }
            return lbInput.SelectedValue.Trim();
        }

        public static string Trim(TextBox txtInput)
        {
            return txtInput.Text.Trim();
        }

        public static string Trim(string input, string DelimitSign)
        {
            if (input == null)
            {
                input = "";
            }
            return input.Trim(DelimitSign.ToCharArray());
        }

        public static string Trim(HtmlInputText txtInput, string DelimitSign)
        {
            return txtInput.Value.Trim(DelimitSign.ToCharArray());
        }

        public static string Trim(HtmlSelect lsInput, string DelimitSign)
        {
            if (lsInput.Multiple)
            {
                throw new Exception("Cannot process with Multiple Selection mode");
            }
            return lsInput.Value.Trim(DelimitSign.ToCharArray());
        }

        public static string Trim(DropDownList ddlInput, string DelimitSign)
        {
            return ddlInput.SelectedValue.Trim(DelimitSign.ToCharArray());
        }

        public static string Trim(ListBox lbInput, string DelimitSign)
        {
            if (lbInput.SelectionMode == ListSelectionMode.Multiple)
            {
                throw new Exception("Cannot process with Multiple Selection mode");
            }
            return lbInput.SelectedValue.Trim(DelimitSign.ToCharArray());
        }

        public static string Trim(TextBox txtInput, string DelimitSign)
        {
            return txtInput.Text.Trim(DelimitSign.ToCharArray());
        }

        /// <summary>
        /// Get indexes of Character in input data
        /// </summary>
        /// <param name="input">Data input</param>
        /// <param name="character">Character need to get indexes</param>
        /// <returns>List of index</returns>
        public static List<int> GetIndexOfChar(string input, char character)
        {
            List<int> indexes = new List<int>();
            int at = 0;
            int start = 0;
            while ((start < input.Length) && (at > -1))
            {
                at = input.IndexOf(character, start);
                if (at == -1) break;
                indexes.Add(at);
                start = at + 1;
            }
            return indexes;
        }

        public static string TrimWhiteSpace(string content)
        {
            if (_whiteSpaceRemover == null)
            {
                _whiteSpaceRemover = new StringReplacer();
                _whiteSpaceRemover.AddStringReplacementPair("\r\n", " ");
                _whiteSpaceRemover.AddStringReplacementPair("\t", " ");
                _whiteSpaceRemover.AddStringReplacementPair("  ", " ");
            }
            _whiteSpaceRemover.Content = content;
            return _whiteSpaceRemover.GetReplacedContent();
        }

        public static string ModifyHTML(string html)
        {
            StringReplacer replacer = new StringReplacer(html);
            replacer.AddStringReplacementPair("\r", string.Empty);
            replacer.AddStringReplacementPair("\n", string.Empty);
            replacer.AddStringReplacementPair("\t", string.Empty);
            replacer.AddStringReplacementPair("&nbsp;", " ");
            replacer.AddStringReplacementPair("&#160;", " ");
            replacer.AddStringReplacementPair("&#039;", "'");
            replacer.AddStringReplacementPair("&#39;", "'");
            replacer.AddStringReplacementPair("&amp;", "&");
            replacer.AddStringReplacementPair("&pound;", "£");
            replacer.AddStringReplacementPair("  ", " ");
            return replacer.GetReplacedContent();
        }

        public static string NotRegex(string input)
        {
            StringReplacer replacer = new StringReplacer(input);
            replacer.AddStringReplacementPair("\\", "\\\\");
            replacer.AddStringReplacementPair(".", "\\.");
            replacer.AddStringReplacementPair("(", "\\(");
            replacer.AddStringReplacementPair(")", "\\)");
            replacer.AddStringReplacementPair("$", "\\$");
            replacer.AddStringReplacementPair("^", "\\^");
            replacer.AddStringReplacementPair("{", "\\{");
            replacer.AddStringReplacementPair("}", "\\}");
            replacer.AddStringReplacementPair("[", "\\[");
            replacer.AddStringReplacementPair("]", "\\]");
            replacer.AddStringReplacementPair("|", "\\|");
            replacer.AddStringReplacementPair("*", "\\*");
            replacer.AddStringReplacementPair("+", "\\+");
            replacer.AddStringReplacementPair("?", "\\?");
            return replacer.GetReplacedContent();
        }

        /// <summary>
        /// Truncates a string to a certain length
        /// </summary>
        /// <param name="valueToTruncate"></param>
        /// <param name="maxLength">Maximum length that the string should allow</param>
        /// <param name="options">Option flags to determine how to truncate the string
        ///		<para>&#160;&#160;&#160;&#160;<c>None</c>: Just cut off the word at the maximum length</para>
        ///		<para>&#160;&#160;&#160;&#160;<c>FinishWord</c>: Make sure that the string is not truncated in the middle of a word</para>
        ///		<para>&#160;&#160;&#160;&#160;<c>AllowLastWordToGoOverMaxLength</c>: If FinishWord is set, this allows the string to be longer than the maximum length if there is a word started and not finished before the maximum length</para>
        ///		<para>&#160;&#160;&#160;&#160;<c>IncludeEllipsis</c>: Include an ellipsis HTML character at the end of the truncated string.  This counts as one of the characters for the maximum length</para>
        /// </param>
        /// <returns>Truncated string</returns>
        public static string Truncate(this string valueToTruncate, int maxLength, TruncateOptions options)
        {
            if (valueToTruncate == null || maxLength <= 0)
            {
                return "";
            }

            if (valueToTruncate.Length <= maxLength)
            {
                return valueToTruncate;
            }

            bool includeEllipsis = (options & TruncateOptions.IncludeEllipsis) == TruncateOptions.IncludeEllipsis;
            bool finishWord = (options & TruncateOptions.FinishWord) == TruncateOptions.FinishWord;
            bool allowLastWordOverflow = (options & TruncateOptions.AllowLastWordToGoOverMaxLength) == TruncateOptions.AllowLastWordToGoOverMaxLength;

            string retValue = valueToTruncate;

            if (includeEllipsis)
            {
                maxLength -= 1;
            }

            int lastSpaceIndex = retValue.LastIndexOf(" ", maxLength, StringComparison.CurrentCultureIgnoreCase);

            if (!finishWord)
            {
                retValue = retValue.Remove(maxLength);
            }
            else if (allowLastWordOverflow)
            {
                int spaceIndex = retValue.IndexOf(" ", maxLength, StringComparison.CurrentCultureIgnoreCase);
                if (spaceIndex != -1)
                {
                    retValue = retValue.Remove(spaceIndex);
                }
            }
            else if (lastSpaceIndex > -1)
            {
                retValue = retValue.Remove(lastSpaceIndex);
            }

            if (includeEllipsis && retValue.Length < valueToTruncate.Length)
            {
                retValue += "...";
            }
            return retValue;
        }

        public static string Truncate(string str, int maxLength, string suffix)
        {
            if (str != null && str.Length > maxLength)
            {
                str = str.Substring(0, maxLength + 1);
                str = str.Substring(0, Math.Min(str.Length, str.LastIndexOf(" ") == -1 ? 0 : str.LastIndexOf(" ")));
                str = str + suffix;
            }
            return str.Trim();
        }

        public static string Truncate(string str, int fromTextIndex, int maxLength, string suffix)
        {
            if (str != null && str.Length > maxLength)
            {
                string[] words = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (fromTextIndex < words.Length)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = fromTextIndex; sb.ToString().Length + words[i].Length <= maxLength; i++)
                    {
                        sb.Append(words[i]);
                        sb.Append(" ");
                    }
                    str = sb.ToString().TrimEnd(' ') + suffix;
                }
            }
            return str.Trim();
        }
        public static string TruncateLongString(string str, int maxlength)
        {
            string result = str;

            if (str != null && str.Length > maxlength)
            {
                result = str.Substring(0, maxlength - ("...".Length));
                int lastIndexOfSpace = result.LastIndexOfAny(new char[] { ' ', '\t', '\n' });
                if (lastIndexOfSpace > 0)
                {
                    result = result.Substring(0, lastIndexOfSpace);
                }
                result += "...";

            }
            return result;
        }
        public static string TruncateLongString2(string str, int maxlength)
        {
            string result = string.IsNullOrEmpty(str) ? string.Empty : str;

            if (str != null && str.Length > maxlength)
            {
                result = str.Substring(0, maxlength - ("...".Length));

                result += "...";

            }
            return result;
        }

        public static bool ContainSubstring(this string str, string substring, char separate, bool ignoreCase)
        {
            string[] arrStr = str.Split(separate);
            foreach (string item in arrStr)
            {
                if (item.Trim().Equals(substring.Trim(), ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture))
                    return true;
            }
            return false;
        }

        public static string CleanWordHtml(this string html)
        {
            StringCollection sc = new StringCollection();
            // get rid of unnecessary tag spans (comments and title)
            sc.Add(@"<!--(\w|\W)+?-->");
            sc.Add(@"<title>(\w|\W)+?</title>");
            // Get rid of classes and styles
            sc.Add(@"\s?class=\w+");
            sc.Add(@"\s+style='[^']+'");
            // Get rid of unnecessary tags
            sc.Add(
            @"<(meta|link|/?o:|/?style|/?div|/?st\d|/?head|/?html|body|/?body|/?span|!\[)[^>]*?>");
            // Get rid of empty paragraph tags
            sc.Add(@"(<[^>]+>)+&nbsp;(</\w+>)+");
            // remove bizarre v: element attached to <img> tag
            sc.Add(@"\s+v:\w+=""[^""]+""");
            // remove extra lines
            sc.Add(@"(\n\r){2,}");
            foreach (string s in sc)
            {
                html = Regex.Replace(html, s, "", RegexOptions.IgnoreCase);
            }
            //return FixEntities(html);
            return html;
        }

        private static string FixEntities(string html)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("\"", "&ldquo;");
            nvc.Add("\"", "&rdquo;");
            nvc.Add("–", "&mdash;");
            foreach (string key in nvc.Keys)
            {
                html = html.Replace(key, nvc[key]);
            }
            return html;
        }

        /// <summary>
        /// Returns a string with backslashes before characters that need to be quoted
        /// </summary>
        /// <param name="InputTxt">Text string need to be escape with slashes</param>
        public static string AddSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }

        /// <summary>
        /// Un-quotes a quoted string
        /// </summary>
        /// <param name="InputTxt">Text string need to be escape with slashes</param>
        public static string StripSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"(\\)([\000\010\011\012\015\032\042\047\134\140])", "$2");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }

        public static string BuildJsonString(StringCollection keys, StringCollection values)
        {
            string json = "[";
            for (int i = 0; i < keys.Count; i++)
            {
                json += "{\"key\":\"" + keys[i] + "\",\"value\":\"" + values[i] + "\"},";
            }
            json = json.TrimEnd(',') + "]";
            return json;
        }

        public static string TrimLast(this string source, string value)
        {
            int index = source.LastIndexOf(value);
            return index != -1 ? source.Remove(index, value.Length) : source;
        }

        private static string join_unit(long number)
        {
            var n = number.ToString();
            int sokytu = n.Length;
            int sodonvi = (sokytu % 3 > 0) ? (sokytu / 3 + 1) : (sokytu / 3);
            n = n.PadLeft(sodonvi * 3, '0');
            sokytu = n.Length;
            string chuoi = "";
            int i = 1;
            while (i <= sodonvi)
            {
                if (i == sodonvi) chuoi = join_number((int.Parse(n.Substring(sokytu - (i * 3), 3))).ToString()) + Unit(i) + chuoi;
                else chuoi = join_number(n.Substring(sokytu - (i * 3), 3)) + Unit(i) + chuoi;
                i += 1;
            }
            return chuoi;
        }


        private static string Unit(int n)
        {
            string chuoi = "";
            if (n == 1) chuoi = " đồng chẵn";
            else if (n == 2) chuoi = " nghìn ";
            else if (n == 3) chuoi = " triệu ";
            else if (n == 4) chuoi = " tỷ ";
            else if (n == 5) chuoi = " nghìn tỷ ";
            else if (n == 6) chuoi = " triệu tỷ ";
            else if (n == 7) chuoi = " tỷ tỷ ";
            return chuoi;
        }


        private static string convert_number(string n)
        {
            string chuoi = "";
            if (n == "0") chuoi = "không";
            else if (n == "1") chuoi = "một";
            else if (n == "2") chuoi = "hai";
            else if (n == "3") chuoi = "ba";
            else if (n == "4") chuoi = "bốn";
            else if (n == "5") chuoi = "năm";
            else if (n == "6") chuoi = "sáu";
            else if (n == "7") chuoi = "bảy";
            else if (n == "8") chuoi = "tám";
            else if (n == "9") chuoi = "chín";
            return chuoi;
        }


        private static string join_number(string n)
        {
            string chuoi = "";
            int i = 1, j = n.Length;
            while (i <= j)
            {
                if (i == 1) chuoi = convert_number(n.Substring(j - i, 1)) + chuoi;
                else if (i == 2) chuoi = convert_number(n.Substring(j - i, 1)) + " mươi " + chuoi;
                else if (i == 3) chuoi = convert_number(n.Substring(j - i, 1)) + " trăm " + chuoi;
                i += 1;
            }
            return chuoi;
        }


        private static string replace_special_word(string chuoi)
        {
            chuoi = chuoi.Replace("không mươi không ", "");
            chuoi = chuoi.Replace("không mươi", "lẻ");
            chuoi = chuoi.Replace("i không", "i");
            chuoi = chuoi.Replace("i năm", "i lăm");
            chuoi = chuoi.Replace("một mươi", "mười");
            chuoi = chuoi.Replace("mươi một", "mươi mốt");
            chuoi = chuoi.Replace("không trăm ", "");
            chuoi = chuoi.Replace("nghìn trăm", "trăm");
            chuoi = chuoi.Replace("triệu nghìn", "triệu");
            chuoi = chuoi.Replace("tỷ triệu", "tỷ");

            return chuoi;
        }

        public static string ConvertMoneyToString(decimal number)
        {
            var num = Math.Abs(number);
            var strnumber = replace_special_word(join_unit((long)num)).Trim();
            var negative = number < 0 ? "âm " : "";
            strnumber = negative + strnumber;
            var first = strnumber.Substring(0, 1).ToUpper();
            return first + strnumber.Substring(1);
        }
        public static string ImageToBase64(string url)
        {
            var path = HttpContext.Current.Server.MapPath(url);
            if (!File.Exists(path)) return string.Empty;
            var image = System.Drawing.Image.FromFile(path);
            using (var ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static string SEOTextReplacer(string input)
        {
            string tmp = string.Empty;
            string result = input;
            int count = 0;

            var dicTextReplacer = new Dictionary<string, string>();
            dicTextReplacer.Add("jobs,  jobs,  jobs", string.Empty);
            dicTextReplacer.Add("Other, , , , , , ,", string.Empty);
            while (tmp != result)
            {
                count++;
                tmp = result;
                foreach (KeyValuePair<string, string> k in dicTextReplacer)
                {
                    result = Regex.Replace(result, k.Key.Replace("nbsp", "&nbsp;"), k.Value);
                }
                if (tmp == result || count > 50)//because we cannot control for the textreplacement input by users
                {
                    //break immediately
                    break;
                }
            }

            return result;
        }

        public static string BalanceTags(string html)
        {
            StringCollection ignoreTags = new StringCollection();
            ignoreTags.Add("img");
            ignoreTags.Add("br");
            ignoreTags.Add("li");

            return BalanceTags(html, ignoreTags);
        }

        public static string formatTextInUrl(string text)
        {
            string urlBuilder = new StringBuilder(text)
                                    .Replace(" ", "-").Replace("--", "-").Replace("'", "-")
                                    .Replace("\"", "").Replace("/", "").Replace(".", "").Replace("&", "and").ToString();
            return HttpContext.Current.Server.UrlPathEncode(urlBuilder);
        }

        public static string RemoveSpecialChar(string input)
        {
            return input.Replace("(", string.Empty).Replace(")", string.Empty).Replace("'", string.Empty);
        }

        public static string RemoveMultipleSpecialChar(string input, string key)
        {
            var pattern = new StringBuilder();

            pattern.Append(key);
            pattern.Append("{2,}");
            return !string.IsNullOrEmpty(input) ? Regex.Replace(input, pattern.ToString(), key) : string.Empty;
        }
        public static string ExtractDomainName(string url)
        {
            if (url.Contains(@"://"))
                url = url.Split(new string[] { "://" }, 2, StringSplitOptions.None)[1];

            return url.Split('/')[0];
        }

        public static string ConvertToSlugUrl(string text)
        {
            text = text.ToLower();
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            text = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            text = Regex.Replace(text, @"[^a-z0-9-]", "-");
            text = Regex.Replace(text, @"-+", "-");
            text = text.Trim().TrimStart('-').TrimEnd('-');

            return text;
        }
    }

    [Flags]
    public enum TruncateOptions
    {
        None = 0x0,
        [Description("Make sure that the string is not truncated in the middle of a word")]
        FinishWord = 0x1,
        [Description("If FinishWord is set, this allows the string to be longer than the maximum length if there is a word started and not finished before the maximum length")]
        AllowLastWordToGoOverMaxLength = 0x2,
        [Description("Include an ellipsis HTML character at the end of the truncated string.  This counts as one of the characters for the maximum length")]
        IncludeEllipsis = 0x4
    }


}


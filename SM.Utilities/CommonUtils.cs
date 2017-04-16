using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SM.Utilities
{
    public class CommonUtils
    {
        public const string SQLStopWordList = @"1,before,these,on,him,
            2,being,they,only,himself,
            3,between,this,or,his,
            4,both,those,other,how,
            5,but,through,our,if,
            6,by,to,out,in,
            7,came,too,over,into.
            8,can,under,re,is,
            9,come,up,said,it,
            0,could,use,same,its,
            about,did,very,see,just,
            after,do,want,should,like,
            all,does,was,since,make,
            also,each,way,so,many,
            an,else,we,some,me,
            and,for,well,still,might,
            another,from,were,such,more,
            any,get,what,take,most,
            are,got,when,than,much,
            as,has,where,that,must,
            at,had,which,the,my,
            be,he,while,their,never,
            $,have,who,them,no,
            because,her,will,then,now,
            been,here,with,there,of,
            would,you,your,
            a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z";
        private static System.Collections.ArrayList _SQLStopWords = null;
        public static System.Collections.ArrayList SQLStopWords
        {
            get
            {
                if (_SQLStopWords == null)
                {
                    _SQLStopWords = new ArrayList();
                    //dont use stop words any more
                    return _SQLStopWords;
                    string[] allWords = SQLStopWordList.Split(new string[] { ",", System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < allWords.Length; i++)
                    {
                        _SQLStopWords.Add(allWords[i].Trim());
                    }
                }
                return _SQLStopWords;
            }
        }

        public static string RemoveSQLStopWords(string input)
        {
            string result = string.Empty;
            string[] arr = input.Split(new string[] { " ", "\n", "\r", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            if (arr != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!SQLStopWords.Contains(arr[i].ToLower()))
                    {
                        result += arr[i] + " ";
                    }
                }
            }
            return result.Trim();
        }

        public static string GetValidFormalCharacters(string inputstring, bool isRemoveStopWord)
        {
            string result = inputstring.Replace("\"", "quot").Replace("'", "singleqt");
            Regex rx = new Regex("[^\\w]");

            result = rx.Replace(result, " ").Replace("  ", " ");
            result = result.Replace("quot", "\"").Replace("singleqt", "''");
            result = result.Replace("''''", "''");
            if (isRemoveStopWord)
                result = RemoveSQLStopWords(result);
            return result;
        }
        public static string EncodeKeywordFields(string keywords)
        {
            string result = keywords;
            result = result.Replace("%", "");
            string[] specialCharacters = new string[] { "&", "+", "*", "?", ":", ">", "<", "=", "/", "\\" };

            for (int i = 0; i < specialCharacters.Length; i++)
            {
                result = result.Replace(specialCharacters[i], " ");
            }
            result = System.Web.HttpUtility.UrlEncode(result);

            for (int i = 0; i < specialCharacters.Length; i++)
            {
                result = result.Replace(specialCharacters[i], " ");
            }
            return result.Trim();
        }


        public static bool IsFileTypeValid(HttpPostedFileBase file)
        {
            bool isValid = false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    if (IsOneOfValidFormats(img.RawFormat))
                    {
                        isValid = true;
                    }
                }
            }
            catch
            {
                //Image is invalid
            }
            return isValid;
        }


        public static string RemoveMultipleSpace(string input)
        {
            return !string.IsNullOrEmpty(input) ? Regex.Replace(input, " {2,}", " ") : string.Empty;
        }

        private static bool IsOneOfValidFormats(ImageFormat rawFormat)
        {
            List<ImageFormat> formats = getValidFormats();

            foreach (ImageFormat format in formats)
            {
                if (rawFormat.Equals(format))
                {
                    return true;
                }
            }
            return false;
        }

        private static List<ImageFormat> getValidFormats()
        {
            List<ImageFormat> formats = new List<ImageFormat>();
            formats.Add(ImageFormat.Png);
            formats.Add(ImageFormat.Jpeg);
            formats.Add(ImageFormat.Gif);
            //add types here
            return formats;
        }
    }
}

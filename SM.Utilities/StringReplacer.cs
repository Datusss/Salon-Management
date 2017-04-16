using System.Collections.Generic;
using System.Text;

namespace SM.Utilities
{
    public class StringReplacer
    {
        public string Content { get; set; }
        private readonly Dictionary<string, string> _stringReplacementPairs;

        public StringReplacer()
        {
            this._stringReplacementPairs = new Dictionary<string, string>();
            this.Content = string.Empty;
        }

        public StringReplacer(string content)
        {
            this._stringReplacementPairs = new Dictionary<string, string>();
            this.Content = string.Empty;
            this.Content = content;
        }

        public void AddStringReplacementPair(string stringToFind, string stringToReplace)
        {
            this._stringReplacementPairs.Add(stringToFind, stringToReplace);
        }

        public void ClearPairs()
        {
            this._stringReplacementPairs.Clear();
        }

        public string GetReplacedContent()
        {
            string content = this.Content;
            foreach (KeyValuePair<string, string> pair in this._stringReplacementPairs)
            {
                content = Replace(content, pair.Key, pair.Value);
            }
            return content;
        }

        public static string Replace(string content, string stringToFind, string stringToReplace)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(stringToFind))
                return content;

            var builder = new StringBuilder(content);
            while (builder.ToString().Contains(stringToFind))
            {
                builder.Replace(stringToFind, stringToReplace);
            }
            return builder.ToString();
        }
    }
}


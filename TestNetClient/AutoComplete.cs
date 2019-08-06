using System;
using System.Collections.Generic;
using System.IO;

namespace NetDebugger
{
    public class AutoComplete
    {
        private static List<string> texts = new List<string>();

        static AutoComplete()
        {
            using (var sr = new StreamReader("./keywords.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var text = line.Trim();
                    if (!string.IsNullOrEmpty(text))
                        texts.Add(text);
                }
            }
        }

        public static string GetHint(string str, int index)
        {
            var dotIndex = str.LastIndexOf('.');
            var prefix = "";
            if (dotIndex >= 0)
            {
                prefix = str.Substring(0, dotIndex+1);
                str = str.Substring(dotIndex+1);
            }

            if (string.IsNullOrEmpty(str))
                return "";

            List<string> results = new List<string>();
            foreach (var text in texts)
            {
                if (text.StartsWith(str))
                    results.Add(prefix + text);
            }

            if (results.Count == 0)
                return "";

            return results[index % results.Count];
        }
    }
}
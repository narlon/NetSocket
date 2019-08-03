using System.Collections.Generic;
using System.IO;

namespace TestNetClient
{
    public class InitRunCmd
    {
        public static List<string> Cmds = new List<string>();

        static InitRunCmd()
        {
            using (var sr = new StreamReader("./initrun.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var text = line.Trim();
                    if (!string.IsNullOrEmpty(text))
                        Cmds.Add(text);
                }
            }
        }
    }
}
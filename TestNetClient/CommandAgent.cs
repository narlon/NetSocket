using System.IO;
using System.Text.RegularExpressions;
using JLM.NetSocket;

namespace NetDebugger
{
    public static class CommandAgent
    {
        private static NetClient client;
        private static string command;
        private static string path;
        private static int bindPlayerIndex = 1;

        public static void Init(NetClient c)
        {
            client = c;
        }
        
        public static void SetCommand(string cmd, string p)
        {
            command = cmd.Trim();
            path = p;
            bindPlayerIndex = 1;

            if (path != "")
            {
                if (File.Exists(p))
                    File.Delete(p);
            }
        }

        public static bool OnReply(string ret)
        {
            switch (command)
            {
                case "pg.entities": OnEntities(ret); break;
            }

            if (path != "")
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                    sw.WriteLine(ret);
                return false;
            }

            return true;
        }

        private static void OnEntities(string line)
        {
            Regex rg = new Regex(@".*\[(.*)\].*=.*\[(.*)\]");
            var matches = rg.Matches(line);
            if (matches.Count > 0)
            {
                string entityId = matches[0].Groups[1].Value;
                string entityType = matches[0].Groups[2].Value;

                // autobind
                //var cmd = string.Format("e{0}=pg.entities[{1}]\n", int.Parse(entityId) % 1000, entityId);
                //client.Send(System.Text.Encoding.Default.GetBytes(cmd));

                if (entityType == "Player")
                {
                    var cmd = string.Format("p{0}=pg.entities[{1}]\n", bindPlayerIndex++, entityId);
                    client.Send(System.Text.Encoding.Default.GetBytes(cmd));
                }
                else if (entityType.EndsWith("Stub"))
                {
                    var callName = entityType.Substring(0, entityType.Length - 4).ToLower();
                    var cmd = string.Format("{0}=pg.entities[{1}]\n", callName, entityId);
                    client.Send(System.Text.Encoding.Default.GetBytes(cmd));
                }
            }

        }
    }
}
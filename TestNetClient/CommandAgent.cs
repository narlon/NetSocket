using System.Text.RegularExpressions;
using JLM.NetSocket;

namespace TestNetClient
{
    public static class CommandAgent
    {
        private static NetClient client;
        private static string command;
        private static int bindPlayerIndex = 1;

        public static void Init(NetClient c)
        {
            client = c;
        }
        
        public static void SetCommand(string cmd)
        {
            command = cmd.Trim();
            bindPlayerIndex = 1;
        }

        public static void OnReply(string ret)
        {
            switch (command)
            {
                case "pg.entities": OnEntities(ret); break;
            }
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
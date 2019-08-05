using System;
using System.Collections.Generic;

namespace TestNetClient
{
    public class CommandHistory
    {
        private static List<string> savedCommands = new List<string>();
        private static int savedIndex = 0;

        public static void Save(string cmd)
        {
            savedCommands.RemoveAll(c => c == cmd);
            savedCommands.Add(cmd);
            savedIndex = 0;
        }

        public static bool TryLoadFormer(out string dts)
        {
            if (savedCommands.Count > 0)
            {
                savedIndex--;
                if (Math.Abs(savedIndex) > savedCommands.Count)
                    savedIndex = -savedCommands.Count;
                dts = savedCommands[savedCommands.Count + savedIndex];
                return true;
            }

            dts = "";
            return false;
        }
        public static bool TryLoadLatter(out string dts)
        {
            if (savedCommands.Count > 0)
            {
                savedIndex++;
                if (savedIndex >= 0)
                    savedIndex = -1;
                dts = savedCommands[savedCommands.Count + savedIndex];
                return true;
            }

            dts = "";
            return false;
        }
    }
}
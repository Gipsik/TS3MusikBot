using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TS3MusicBot
{
    public class commands
    {
        Dictionary<string, string> userCommands;

        public commands()
        {
            userCommands = new Dictionary<string, string>();
        }

        public bool AddCommand(string trigger, string command)
        {
            if (command.Contains("!bind") || command.Contains("|") || userCommands.Keys.Contains(trigger))
            {
                return false;
            }
            else
            {
                File.AppendAllText("commands.txt", trigger + "=" + command + "|");
                updateCommands();
                return true;
            }
        }

        public void updateCommands()
        {
            if(File.Exists("commands.txt") == false)
            {
                File.Create("commands.txt").Close(); ;
            }
            string rawcommands = File.ReadAllText("commands.txt");
            List<string> ripStage1 = rawcommands.Split('|').ToList();
            ripStage1.Remove("");
            foreach(string command in ripStage1)
            {
                try
                {
                    string trigger = command.Substring(0, command.IndexOf("="));
                    string assigncommand = command.Substring(command.IndexOf("=") + 1, command.Length - command.IndexOf("=") - 1);
                    userCommands.Add(trigger, assigncommand);
                }
                catch(Exception e)
                {
                    continue;
                }
                
            }
        }

        public string getCommand(string trigger)
        {
            string retval = "";

            if(userCommands.Keys.Contains(trigger))
            {
                retval = userCommands[trigger];
            }
            else
            {
                retval = "custom command '"+trigger+"' not found. Sorry </3";
            }

            return retval;
        }

    }
}

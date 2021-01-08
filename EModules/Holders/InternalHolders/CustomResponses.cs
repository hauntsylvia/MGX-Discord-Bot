using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;
using MGX_Discord_Bot.EModules.Scheduler;
using Newtonsoft.Json;

namespace MGX_Discord_Bot.EModules.Holders.InternalHolders
{
    class CustomResponses
    {
        public static Dictionary<string, string> WordResp = new Dictionary<string, string>();

        public static void UpdateResponses()
        {
            using(StreamReader SR = new StreamReader(@".\CustomResponses.txt"))
            {
                foreach(var Line in SR.ReadToEnd().Split('\n'))
                {
                    string Key = Line.Split('"')[1].ToLower();
                    if(!WordResp.ContainsKey(Key))
                    {
                        string Val = Line.Split('"')[3].ToLower();
                        WordResp.Add(Key, Val);
                    }
                }
            }
        }
        public static string ReturnResponse(string Given)
        {
            if(WordResp.ContainsKey(Given.ToLower()))
            {
                return WordResp[Given.ToLower()];
            }
            return null;
        }
    }
}

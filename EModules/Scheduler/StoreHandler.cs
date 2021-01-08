using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Management;
using System.Reflection;

using Discord;
using Discord.API;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Scheduler
{
    class StoreHandler
    {
        public static string DirectoryS = @".\AbstractSaved\";

        public static string[] ReturnFilePathOfPattern(object Pattern)
        {
            if (!Directory.Exists(DirectoryS))
            {
                Directory.CreateDirectory(DirectoryS);
            }

            List<string> TEMP = new List<string>();

            foreach (var Fi in Directory.GetFiles(DirectoryS))
            {
                if (Fi.Contains(Pattern.ToString()))
                {
                    TEMP.Add(Fi);
                }
            }

            return TEMP.ToArray();
        }

        public static int ReturnCountOfPattern(object Pattern)
        {
            if (!Directory.Exists(DirectoryS))
            {
                Directory.CreateDirectory(DirectoryS);
            }

            int Occ = 0;
            foreach(var Fi in Directory.GetFiles(DirectoryS))
            {
                if(File.ReadAllText(Fi).Contains(Pattern.ToString()))
                {
                    Occ += 1;
                }
            }
            return Occ;
        }

        public static void SaveData(ulong UserId, object Key, object Value)
        {
            try
            {
                if (!Directory.Exists(DirectoryS))
                {
                    Directory.CreateDirectory(DirectoryS);
                }
                string Ui = DirectoryS + UserId + ".mgx";
                Dictionary<string, string> Data = new Dictionary<string, string>();
                if(File.Exists(Ui))
                {
                    using(StreamReader SR = new StreamReader(Ui))
                    {
                        foreach(var Line in SR.ReadToEnd().Split('\n'))
                        {
                            if(Line.Contains("*"))
                            {
                                string ThisKey = Line.Split('*')[0];
                                string ThisValue = Line.Split('*')[1];
                                if ((string)Key != ThisKey)
                                {
                                    Data.Add(ThisKey, ThisValue);
                                }
                            }
                        }
                        Data.Add(Key.ToString(), Value.ToString());
                    }
                }
                string Format = "";
                foreach (var DKey in Data.Keys)
                {
                    Format = Format + $"{DKey}*{Data[DKey]}" + "\n";
                }
                using (StreamWriter SW = new StreamWriter(Ui))
                {
                    SW.Write(Format);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static string ReturnFilePath(ulong UserId)
        {
            if (!Directory.Exists(DirectoryS))
            {
                Directory.CreateDirectory(DirectoryS);
            }
            return File.Exists($"{DirectoryS}{UserId}.mgx") ? $"{DirectoryS}{UserId}.mgx" : null;
        }
        public static string LoadData(ulong UserId, object Key)
        {
            try
            {
                if (!Directory.Exists(DirectoryS))
                {
                    Directory.CreateDirectory(DirectoryS);
                }
                string ToRet = null;
                string Ui = DirectoryS + UserId + ".mgx";
                if (File.Exists(Ui))
                {
                    using (StreamReader SR = new StreamReader(Ui))
                    {
                        foreach (var Line in SR.ReadToEnd().Split('\n'))
                        {
                            if(Line.Contains("*"))
                            {
                                string ThisKey = Line.Split('*')[0];
                                string ThisValue = Line.Split('*')[1];
                                if ((string)Key == ThisKey && ThisValue.Length > 0)
                                {
                                    return ThisValue;
                                }
                            }
                        }
                    }
                }
                return ToRet;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public static void RemoveData(ulong UserId, object Key)
        {
            SaveData(UserId, Key, "");
        }

        public static void ImExpData()
        {
            foreach(var Fi in Directory.GetFiles(DirectoryS))
            {
                if(!Path.GetExtension(Fi).ToLower().Contains("mgx"))
                {
                    var UserId = ulong.Parse(Path.GetFileNameWithoutExtension(Fi).Split('-')[0]);
                    var Data = File.ReadAllText(Fi);
                    var Key = Path.GetFileNameWithoutExtension(Fi).Split(new[] { '-' }, 2)[1];
                    SaveData(UserId, Key, Data);
                    File.Delete(Fi);
                }
            }
        }
    }
}

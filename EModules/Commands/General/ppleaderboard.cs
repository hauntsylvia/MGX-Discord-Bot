using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

using MGX_Discord_Bot.EModules.Scheduler;
using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class ppleaderboard
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "fun",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                var Builder = new EmbedBuilder()
                {
                    Color = Color.Teal,
                    Title = "**PP LEADERBOARD**",
                    ThumbnailUrl = "https://i.gyazo.com/e45b78214bb4120c53625852935dfa33.png"
                };

                var Stuff = StoreHandler.ReturnFilePathOfPattern("-PPSIZE");
                var T3 = new Dictionary<string, int>();
                foreach (string Fi in Stuff)
                {
                    T3.Add(Fi.Split('-')[0].Split(new string[] { @"\" }, StringSplitOptions.None)[2], Int32.Parse(File.ReadAllText(Fi)));
                }
                var Arr = T3.Values.ToArray();
                Array.Sort(Arr);
                Array.Reverse(Arr);
                Dictionary<string, int> RematchedVals = new Dictionary<string, int>();
                foreach (int Val in Arr)
                {
                    foreach (string SKey in T3.Keys)
                    {
                        if (Val == T3[SKey] & !RematchedVals.ContainsKey(SKey))
                        {
                            RematchedVals.Add(SKey, Val);
                        }
                    }
                }
                try
                {
                    Builder.AddField("**1st**", $"{Program.Client.GetUser(ulong.Parse(RematchedVals.Keys.First())).Username}\n[{RematchedVals[RematchedVals.Keys.ToArray()[0]]} pp size]");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    StoreHandler.RemoveData(ulong.Parse(RematchedVals.Keys.First()), $"PPSIZE");
                }
                try
                {
                    Builder.AddField("**2nd**", $"{Program.Client.GetUser(ulong.Parse(RematchedVals.Keys.ToArray()[1])).Username}\n[{RematchedVals[RematchedVals.Keys.ToArray()[1]]} pp size]");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    if (RematchedVals.Keys.ToArray().Length >= 2)
                    {
                        StoreHandler.RemoveData(ulong.Parse(RematchedVals.Keys.ToArray()[2]), "PPSIZE");
                    }
                }
                try
                {
                    Builder.AddField("**3rd**", $"{Program.Client.GetUser(ulong.Parse(RematchedVals.Keys.ToArray()[2])).Username}\n[{RematchedVals[RematchedVals.Keys.ToArray()[2]]} pp size]");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    if (RematchedVals.Keys.ToArray().Length >= 3)
                    {
                        StoreHandler.RemoveData(ulong.Parse(RematchedVals.Keys.ToArray()[2]), "PPSIZE");
                    }
                }

                await arg.Channel.SendMessageAsync("", false, Builder.Build());
            }
            catch (Exception FullExc)
            {
                Console.WriteLine(FullExc);
            }
        }
    }
}

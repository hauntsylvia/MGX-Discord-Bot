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
    class rate
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "fun",
            Premium = false,
            RequiredPermissions = 1
        };
        private static string LastRateCache = "";
        public static async Task Maindo(SocketMessage arg)
        {
            Random RanClass = new Random();
            string ToRate = arg.Content.Split(new[] { "rate" }, StringSplitOptions.None)[1];
            if (ToRate.Trim().Length > 0)
            {
                int RateGiven = 1;
                if (Program.RateCache.ContainsKey(ToRate.ToLower()))
                {
                    RateGiven = Program.RateCache[ToRate.ToLower()];
                    await arg.Channel.SendMessageAsync($"{ToRate} is {RateGiven}/10");
                }
                else if (ToRate.Trim().ToLower() == "mgx")
                {
                    RateGiven = -5;
                    await arg.Channel.SendMessageAsync($"{ToRate} is {RateGiven}/10 ;c");
                }
                else
                {
                    RateGiven = RanClass.Next(-2, 10);
                    if (ToRate.Length >= 2)
                    {
                        LastRateCache = ToRate;
                        Thread RateCacheBGThread = new Thread(PrepareRemoveRateCache);
                        RateCacheBGThread.Start();
                        Program.RateCache.Add(ToRate.ToLower(), RateGiven);
                    }
                    await arg.Channel.SendMessageAsync($"{ToRate} is {RateGiven}/10");
                }
            }
        }

        private static void PrepareRemoveRateCache()
        {
            string Prep = LastRateCache;
            Thread.Sleep(500 * 1000);
            Program.RateCache.Remove(Prep);
        }
    }
}

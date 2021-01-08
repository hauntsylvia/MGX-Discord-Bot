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

using Discord;
using Discord.API;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using System.Reflection;

using MGX_Discord_Bot.EModules.Scheduler;
using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class pick
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
            Random RanClass = new Random();
            List<string> AllArgs = new List<string>();
            if (!arg.Content.ToLower().Contains(" or "))
            {
                await arg.Channel.SendMessageAsync("Nothing being compared, use like so; 'this or that or them'");
                return;
            }
            foreach (string Arg in arg.Content.ToLower().Split(new string[] { "pick" }, StringSplitOptions.None)[1].Split(new string[] { "or " }, StringSplitOptions.None))
            {
                AllArgs.Add(Arg.Trim());
            }
            int RanNum = RanClass.Next(0, AllArgs.Count());
            await arg.Channel.SendMessageAsync(AllArgs[RanNum]);
        }
    }
}

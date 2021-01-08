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
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.Maintenance
{
    class gchat_ban0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if(arg.Content.Contains(' ') && ulong.TryParse(arg.Content.ToLower().Split(' ')[1], out ulong ToBan))
            {
                string Reason = "No reason given.";
                if(arg.Content.Split(' ').Length > 2)
                {
                    Reason = arg.Content.Split(new[] { ' ' }, 3)[2];
                }
                else
                {
                    await arg.Channel.SendMessageAsync("Remember that this command allows two arguments; ```\n<user id> <reason>\n```");
                }
                StoreHandler.SaveData(ToBan, "GLOBALCHAT-BAN", Reason);
                await arg.Channel.SendMessageAsync("Banned.");
            }
            else
            {
                await arg.Channel.SendMessageAsync("You need to include the user's id.");
            }
        }
    }
    class gchat_unban0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if (arg.Content.Contains(' ') && ulong.TryParse(arg.Content.ToLower().Split(' ')[1], out ulong ToBan))
            {
                StoreHandler.RemoveData(ToBan, $"GLOBALCHAT-BAN");
                await arg.Channel.SendMessageAsync("Unbanned.");
            }
            else
            {
                await arg.Channel.SendMessageAsync("You need to include the user's id.");
            }
        }
    }
}

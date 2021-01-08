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
    class bal
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            var Embed = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder()
                {
                    IconUrl = arg.Author.GetAvatarUrl(),
                    Name = $"{arg.Author.Username}'s Balance",
                },
                Color = new Color(255, 125, 125),
                Footer = new EmbedFooterBuilder()
                {
                    Text = "MGX"
                },
                Timestamp = DateTime.UtcNow,
                Description = $"${ShopClass.ReadCurrency(arg.Author.Id)}"
            };

            await arg.Channel.SendMessageAsync("", false, Embed.Build());
        }
    }
    class balance
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = true,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await bal.Maindo(arg);
        }
    }
}

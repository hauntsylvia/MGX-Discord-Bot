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
using MGX_Discord_Bot.EModules.Algorithms;
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class edge
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
            EdgeHolder EdgeHandle = new EdgeHolder();
            var Embed = new EmbedBuilder()
            {
                Color = Color.LighterGrey,
                Footer = new EmbedFooterBuilder()
                {
                    Text = "MGX"
                },
                Timestamp = DateTime.UtcNow,
                ThumbnailUrl = "https://i.gyazo.com/537ad8d7a1da7fb8a3c6cbb69d37da1b.png",
            };
            var Field = new EmbedFieldBuilder()
            {
                Name = "?",
                Value = EdgeHandle.ReturnRandomResponse()
            };
            Embed.AddField(Field);
            await arg.Channel.SendMessageAsync("", false, Embed.Build());
            return;
        }
    }
}

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
    class ask
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
            _8BallHolder EightBallHandle = new Holders.CommandHolders._8BallHolder();
            var Embed = new EmbedBuilder()
            {
                Color = Color.DarkMagenta,
                Footer = new EmbedFooterBuilder()
                {
                    Text = "MGX"
                },
                Timestamp = DateTime.UtcNow,
                ThumbnailUrl = Program.Client.CurrentUser.GetAvatarUrl(),
            };
            var Field = new EmbedFieldBuilder()
            {
                Name = "8-Ball's Answer",
                Value = EightBallHandle.ReturnRandomResponse(),
            };
            Embed.AddField(Field);
            await arg.Channel.SendMessageAsync("", false, Embed.Build());
        }
    }
}

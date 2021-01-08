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
    class joke
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
            JokeCommandHolder jkHandle = new JokeCommandHolder();
            int RandomNum = RanClass.Next(0, jkHandle.ReturnList().Count());

            string Joke = jkHandle.ReturnList()[RandomNum];

            var Embed = new EmbedBuilder()
            {
                Color = Color.Purple,
                Description = $"[ {Joke} ]",
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"Joke #{RandomNum}"
                },
                Timestamp = DateTime.UtcNow,
                Author = new EmbedAuthorBuilder()
                {
                    IconUrl = Program.Client.CurrentUser.GetAvatarUrl(),
                    Name = Program.Client.CurrentUser.Username,
                },
            };

            await arg.Channel.SendMessageAsync("", false, Embed.Build());
        }
    }
}

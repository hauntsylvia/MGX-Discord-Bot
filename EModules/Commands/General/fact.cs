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
    class fact
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
            var FactClass = new FactHolder();
            var AllFacts = FactClass.ReturnAllFacts();

            int RandomNum = RanClass.Next(0, AllFacts.Count);

            var Fact = FactClass.ReturnAllFacts()[RandomNum];

            var Embed = new EmbedBuilder()
            {
                Color = Color.LightOrange,
                Description = $"[ {Fact} ]",
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"Fact #{RandomNum}"
                },
                Timestamp = DateTime.UtcNow,
                Author = new EmbedAuthorBuilder()
                {
                    IconUrl = Program.Client.CurrentUser.GetAvatarUrl(),
                    Name = Program.Client.CurrentUser.Username,
                },
            };

            await arg.Channel.SendMessageAsync("", false, Embed.Build());

            return;
        }
    }
}

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
using System.Collections.Concurrent;
using System.Diagnostics;
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

using NekosSharp;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class anal
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "nsfw",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if (((SocketTextChannel)arg.Channel).IsNsfw)
            {
                var NSFWHandleGrab = new NsfwUrlHolder();
                var Builder = new EmbedBuilder()
                {
                    Color = Color.DarkPurple,
                    Description = "NSFW"
                };
                Builder.WithImageUrl((await new NekoClient("MGX").Nsfw_v3.AnalGif()).ImageUrl);
                await arg.Channel.SendMessageAsync("", false, Builder.Build());
            }
            else
            {
                await arg.Channel.SendMessageAsync("Channel isn't nsfw, sorry.");
            }
        }
    }
}

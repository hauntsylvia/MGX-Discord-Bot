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
    class responses
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "settings",
            Premium = false,
            RequiredPermissions = 18
        };
        public static async Task Maindo(SocketMessage arg)
        {
            var GuildCh = arg.Channel as SocketGuildChannel;
            var Guild = GuildCh.Guild;
            bool Cur = GuildStuff.ReadSetting(GuildStuff.GuildSettings.CustomResponses, Guild.Id);
            if (!Cur)
            {
                await arg.Channel.SendMessageAsync($"Custom responses enabled");
                GuildStuff.NewGuildSetting(GuildStuff.GuildSettings.CustomResponses, Guild.Id, true);
            }
            else
            {
                await arg.Channel.SendMessageAsync($"Custom responses disabled");
                GuildStuff.NewGuildSetting(GuildStuff.GuildSettings.CustomResponses, Guild.Id, false);
            }
        }
    }
}

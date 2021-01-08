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
    class stats
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            var A = Assembly.GetExecutingAssembly().GetName().Version;
            var Embed = new EmbedBuilder()
            {
                Description = $"buildv{A.Build}",
                Color = Color.DarkerGrey,
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"Shard {Program.Client.GetShardFor(((SocketGuildChannel)arg.Channel).Guild).ShardId}"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                {
                    {
                        new EmbedFieldBuilder()
                        {
                            IsInline = true,
                            Name = $"[assembly: {A}]",
                            Value = $"```diff\n+ STATISTICS +\n- Mem Usage    ::  {MGXStatistics.MemoryUsageInMB} MB\n- Uptime       ::  {MGXStatistics.GetUptime().Days}d, {MGXStatistics.GetUptime().Hours}h, {MGXStatistics.GetUptime().Minutes}m and {MGXStatistics.GetUptime().Seconds}s\n- Users        ::  {MGXStatistics.GetUserCount()}\n- Servers      ::  {MGXStatistics.GetGuildCount()}```"
                        }
                    },
                },
            };

            await arg.Channel.SendMessageAsync("", false, Embed.Build());
            //arg.Channel.SendMessageAsync($"```diff\n+ STATISTICS +\n- Mem Usage    ::  {MGXStatistics.MemoryUsageInMB} MB\n- Uptime       ::  {MGXStatistics.GetUptime().Days}d, {MGXStatistics.GetUptime().Hours}h, {MGXStatistics.GetUptime().Minutes}m and {MGXStatistics.GetUptime().Seconds}s\n- Users        ::  {MGXStatistics.GetUserCount()}\n- Servers      ::  {MGXStatistics.GetGuildCount()}```");
        }
    }
}

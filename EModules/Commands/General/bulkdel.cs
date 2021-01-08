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
    class bulkdel
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = true,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 18
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                var ChannelAsGuild = (SocketTextChannel)arg.Channel;
                int ToDel = 100;

                ToDel = arg.Content.ToLower().Contains(" ") ? int.Parse(arg.Content.ToLower().Split(' ')[1]) : 100;

                await ((SocketTextChannel)arg.Channel).DeleteMessagesAsync(await arg.Channel.GetMessagesAsync(ToDel).FlattenAsync());

                var Msg = await arg.Channel.SendMessageAsync($"Deleted {ToDel} messages from your trashy channel.");

                Thread.Sleep(3000);

                await Msg.DeleteAsync();
            }
            catch
            {
                await arg.Channel.SendMessageAsync("Messages can't be older than two weeks.");
            }
        }
    }
    class purge
    {
        public static CommandInformation Info = bulkdel.Info;

        public static async Task Maindo(SocketMessage arg)
        {
            await bulkdel.Maindo(arg);
        }
    }
}

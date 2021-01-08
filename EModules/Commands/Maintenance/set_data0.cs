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
using MGX_Discord_Bot.EModules.Entities;
using MGX_Discord_Bot.EModules.Commands.General;
using MGX_Discord_Bot.EModules.Scheduler;

namespace MGX_Discord_Bot.EModules.Commands.Maintenance
{
    class set_data0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };

        public static async Task Maindo(SocketMessage arg)
        {
            await arg.Channel.SendMessageAsync("user");
            ulong UserToEdit = ulong.Parse((await new WaitForResponse()
            {
                TimeLimitS = 10,
                ChannelId = arg.Channel.Id,
                UserId = arg.Author.Id
            }.Start()).Content);
            await arg.Channel.SendMessageAsync("key");
            string KeyToEdit = (await new WaitForResponse()
            {
                TimeLimitS = 10,
                ChannelId = arg.Channel.Id,
                UserId = arg.Author.Id
            }.Start()).Content;
            await arg.Channel.SendMessageAsync("value");
            string Value = (await new WaitForResponse()
            {
                TimeLimitS = 10,
                ChannelId = arg.Channel.Id,
                UserId = arg.Author.Id
            }.Start()).Content;
            StoreHandler.SaveData(UserToEdit, KeyToEdit, Value);
            await arg.Channel.SendMessageAsync("saved");
        }
    }
}

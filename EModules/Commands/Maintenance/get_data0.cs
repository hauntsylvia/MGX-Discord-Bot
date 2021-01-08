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
    class get_data0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };

        public static async Task Maindo(SocketMessage arg)
        {
            var DataE = StoreHandler.ReturnFilePath(0);
            if(DataE != null)
            {
                await arg.Author.SendMessageAsync(File.ReadAllText(StoreHandler.ReturnFilePath(0)));
            }
            else
            {
                await arg.Author.SendMessageAsync("No data.");
            }
        }
    }
}

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

namespace MGX_Discord_Bot.EModules.Commands.Maintenance
{
    class power0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await arg.Channel.SendMessageAsync("Power state = 0");
            await Program.Client.LogoutAsync();
            Environment.Exit(-1);
        }
    }
    class p_0
    {
        public static CommandInformation Info = power0.Info;

        public static async Task Maindo(SocketMessage arg)
        {
            await power0.Maindo(arg);
        }
    }
}

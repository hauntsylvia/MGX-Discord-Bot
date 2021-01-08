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

using Discord;
using Discord.API;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using System.Reflection;
using Discord.Audio;

using MGX_Discord_Bot.EModules.Scheduler;
using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class breakup
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "lovey",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if (StoreHandler.LoadData(arg.Author.Id, "DATINGDATA") != null)
            {
                string ID = StoreHandler.LoadData(arg.Author.Id, "DATINGDATA");
                StoreHandler.RemoveData(ulong.Parse(StoreHandler.LoadData(arg.Author.Id, "DATINGDATA")), "DATINGDATA");
                StoreHandler.RemoveData(arg.Author.Id, $"DATINGDATA");
                await arg.Channel.SendMessageAsync($"You've broken up with <@{ID}>");
            }
            else
            {
                await arg.Channel.SendMessageAsync($"You're not dating anyone..");
            }
        }
    }

}

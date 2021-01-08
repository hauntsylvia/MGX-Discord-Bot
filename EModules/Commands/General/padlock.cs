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
using MGX_Discord_Bot.EModules.Entities;
using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class padlock
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "items",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            string Key = $"PADLOCK-ACTIVE";
            if(StoreHandler.LoadData(arg.Author.Id, Key) != null)
            {
                await arg.Channel.SendMessageAsync("You already have a padlock active!");
            }
            else if(Economy.CountProductForUser(Product.padlock, arg.Author.Id) > 0)
            {
                Economy.ChangeProductCountForUser(Product.padlock, -1, arg.Author.Id);
                StoreHandler.SaveData(arg.Author.Id, Key, true);
                await arg.Channel.SendMessageAsync("Padlock is now active on your balance! This is one-time use, when someone tries to steal from you they will fail immediately.");
            }
            else
            {
                await arg.Channel.SendMessageAsync("You need to buy a padlock to use this!");
            }
        }
    }
}

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
using System.IO.Compression;
using System.Reflection;

using Discord;
using Discord.API;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;

using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;
using MGX_Discord_Bot.EModules.Scheduler;
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class data
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 0
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                var UserChannel = await arg.Author.GetOrCreateDMChannelAsync();
                await arg.Author.SendMessageAsync("Data stored pertaining to you will be sent to this chat - Please wait while it is compiled..");
                await arg.Channel.SendMessageAsync("Check DMs!");
                string UserFile = $@".\{StoreHandler.DirectoryS}\{arg.Author.Id}.mgx";
                await arg.Author.SendFileAsync(UserFile);
                await UserChannel.TriggerTypingAsync();
                Thread.Sleep(3000);
                await UserChannel.SendMessageAsync($"This data is formatted like so: key*value");
                await UserChannel.TriggerTypingAsync();
                Thread.Sleep(4000);
                await UserChannel.SendMessageAsync($"It just contains text, you can open it in Notepad or some other text editor. If you need further help, please email `kxtze.cxffee@gmail.com`!");
            }
            catch(Exception e)
            {
                await arg.Channel.SendMessageAsync($"Unable to direct message, all data is personal and must be sent through direct messages.\nReason: {e.Message}");
            }
        }
    }
}

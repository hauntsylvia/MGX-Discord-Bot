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
    class flower
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
            if (Economy.CountProductForUser(Product.flower, arg.Author.Id) > 0)
            {
                var User = arg.MentionedUsers.Count >= 1 ? arg.MentionedUsers.First() : null;
                if (User == null)
                {
                    await arg.Channel.SendMessageAsync("Please mention a user to give the flower to!");
                    var WFR = new WaitForResponse()
                    {
                        TimeLimitS = 15,
                        ChannelId = arg.Channel.Id,
                        UserId = arg.Author.Id
                    };
                    var Msg = await WFR.Start();
                    User = Msg.MentionedUsers.Count > 0 ? Msg.MentionedUsers.First() : null;
                }
                await arg.Channel.SendMessageAsync($"You gave {User.Mention} a flower! <3");
                Economy.ChangeProductCountForUser(Product.flower, -1, arg.Author.Id);
                Economy.ChangeProductCountForUser(Product.flower, 1, User.Id);
            }
            else
            {
                await arg.Channel.SendMessageAsync($"You need to buy flowers first! `{Program.Prefix}buy flower`");
            }
        }
    }
}

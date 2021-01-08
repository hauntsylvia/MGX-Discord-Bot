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
    class ban
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 19
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await Action(arg, true);
        }

        public static async Task Action(SocketMessage arg, bool IsBanning = false)
        {
            if(arg.MentionedUsers.Count > 0)
            {
                var User = (SocketGuildUser)arg.MentionedUsers.First();
                var UserId = User.Id;
                var Guild = ((SocketTextChannel)arg.Channel).Guild;

                if(User != arg.Author)
                {
                    SocketGuildUser MGX = Guild.GetUser(Program.Client.CurrentUser.Id);

                    if (MGX.GuildPermissions.KickMembers)
                    {
                        if (IsBanning)
                        {
                            await User.BanAsync();
                        }
                        else
                        {
                            await User.KickAsync();
                        }
                        await arg.Channel.SendMessageAsync($"<@{UserId}> removed.");
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync($"Missing permissions to kick/ban!");
                    }
                }
                else
                {
                    await arg.Channel.SendMessageAsync($"Can't do that to yourself noob.");
                }
            }
            else
            {
                await arg.Channel.SendMessageAsync($"Please mention a user.");
            }
        }
    }

    class kick
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 19
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await ban.Action(arg);
        }
    }
}

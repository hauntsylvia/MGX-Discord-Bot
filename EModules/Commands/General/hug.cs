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
using NekosSharp;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class hug
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
            var ChannelChanged = arg.Channel as SocketGuildChannel;
            var Rand = new Random();

            SocketGuildUser User = arg.MentionedUsers.Count() >= 1 ? arg.MentionedUsers.First() as SocketGuildUser : null;

            if (User == null)
            {
                await arg.Channel.SendMessageAsync($"Who would you like to hug?");
                async Task WaitForMsg()
                {
                    Task Ev(SocketMessage newArg)
                    {
                        if(newArg.Author == arg.Author)
                        {
                            Program.Client.MessageReceived -= Ev;
                            if (newArg.MentionedUsers.Count > 0)
                            {
                                User = newArg.MentionedUsers.First() as SocketGuildUser;
                            }
                            else
                            {
                                arg.Channel.SendMessageAsync("lol");
                            }
                        }
                        return Task.CompletedTask;
                    }
                    Program.Client.MessageReceived += Ev;
                }
                await WaitForMsg();
            }

            new Thread(x =>
            {
                int Timer = 30;
                while(true)
                {
                    if(User != null)
                    {
                        var Embed = new EmbedBuilder()
                        {
                            Description = $"***{arg.Author.Mention} hugged {User.Mention}***",
                            Color = new Color(143, 255, 219),
                            Footer = new EmbedFooterBuilder()
                            {
                                Text = "MGX",
                            },
                            ImageUrl = new NekoClient("MGX").Action.Hug().GetAwaiter().GetResult().ImageUrl,
                            Timestamp = DateTime.UtcNow,
                        };

                        arg.Channel.SendMessageAsync("", false, Embed.Build());
                        break;
                    }
                    if(Timer <= 0)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                    Timer = Timer--;
                }
            }).Start();
        }
    }
}

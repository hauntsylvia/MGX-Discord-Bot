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
using System.Collections.Concurrent;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class unquarantine
    {
        private static Dictionary<ulong, ulong> AUsers = new Dictionary<ulong, ulong>();

        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "fun",
            Premium = false,
            RequiredPermissions = 1
        };

        public static async Task Maindo(SocketMessage arg)
        {
            var TargUser = arg.MentionedUsers.Count > 0 ? arg.MentionedUsers.First() : null;
            if (TargUser != null)
            {
                if (quarantine.AUsers.ContainsKey(TargUser.Id) && quarantine.AUsers[TargUser.Id] == ((SocketTextChannel)arg.Channel).Guild.Id)
                {
                    if (quarantine.AUsers.ContainsKey(arg.Author.Id))
                    {
                        var Msg = await arg.Channel.SendMessageAsync("No <:megaflushed:705773621494153287>");
                        Thread.Sleep(5000);
                        await Msg.DeleteAsync();
                    }
                    else
                    {
                        var Msg = await arg.Channel.SendMessageAsync("Removing user from quarantine..");
                        if (quarantine.AUsers.TryRemove(TargUser.Id, out ulong Val))
                        {
                            await Msg.ModifyAsync(x =>
                            {
                                x.Content = $"User (<@{TargUser.Id}>) unquarantined!";
                            });
                        }
                        else
                        {
                            await Msg.ModifyAsync(x =>
                            {
                                x.Content = $"User (<@{TargUser.Id}>) couldn't be unquarantined..";
                            });
                        }
                    }
                }
                else
                {
                    await arg.Channel.SendMessageAsync("This user isn't quarantined in this channel :flushed:");
                }
            }
            else
            {
                await arg.Channel.SendMessageAsync("Please mention a user!");
            }
        }
    }
    class quarantine
    {
        public static ConcurrentDictionary<ulong, ulong> AUsers = new ConcurrentDictionary<ulong, ulong>();

        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "fun",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                if(arg.Content.ToLower().Contains("disable"))
                {
                    GuildStuff.NewGuildSetting(GuildStuff.GuildSettings.CanUseQuarantine, ((SocketTextChannel)arg.Channel).Guild.Id, false);
                    await arg.Channel.SendMessageAsync("No more quarantining in this server!");
                }
                else if (GuildStuff.ReadSetting(GuildStuff.GuildSettings.CanUseQuarantine, ((SocketTextChannel)arg.Channel).Guild.Id))
                {
                    var Guild = ((SocketGuildChannel)arg.Channel).Guild;
                    Emoji VoteYes = new Emoji("☣");
                    Emoji VoteNo = new Emoji("👎");
                    var User = arg.MentionedUsers.Count() >= 1 ? arg.MentionedUsers.First() : arg.Author;
                    if (!AUsers.ContainsKey(arg.Author.Id))
                    {
                        if (AUsers.ContainsKey(User.Id))
                        {
                            AUsers.TryRemove(User.Id, out ulong Value);
                            await arg.Channel.SendMessageAsync($"User {User.Mention} removed from quarantine.");
                        }
                        else
                        {
                            var Embed = new EmbedBuilder()
                            {
                                Footer = new EmbedFooterBuilder()
                                {
                                    Text = $"If you want this command disabled, type '{Program.Prefix}quarantine disable'"
                                },
                                Timestamp = DateTime.UtcNow,
                                Color = Color.Orange,
                                Description = $"Should {User.Mention} Be Quarantined?",
                                Fields = new List<EmbedFieldBuilder>()
                                {
                                    {
                                        new EmbedFieldBuilder()
                                        {
                                            Name = "Vote",
                                            Value = $"{VoteYes} - Yes\n{VoteNo} - No",
                                        }
                                    }
                                },
                                ThumbnailUrl = User.GetAvatarUrl() != null ? User.GetAvatarUrl() : Program.Client.CurrentUser.GetAvatarUrl(),
                            };
                            var Msg = await arg.Channel.SendMessageAsync("", false, Embed.Build());
                            new Thread(async x =>
                            {
                                await Msg.AddReactionsAsync(new[] { VoteYes, VoteNo });
                                int VotesInFavor = 0;
                                ulong Target = User.Id;
                                Func<Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> Ev = (arg1, arg2, arg3) =>
                                {
                                    if (arg3.Emote.Name == "☣")
                                    {
                                        VotesInFavor++;
                                    }
                                    else if (arg3.Emote.Name == "👎")
                                    {
                                        VotesInFavor--;
                                    }
                                    return Task.CompletedTask;
                                };
                                Program.Client.ReactionAdded += Ev;
                                Thread.Sleep(10000);

                                if (VotesInFavor >= 0)
                                {
                                    AUsers.TryAdd(User.Id, Guild.Id);
                                    var MsgOfMGX = await arg.Channel.SendMessageAsync($"<@{Target}> is being quarantined..");
                                    long MsgCount = 0;
                                    Task MessageEv(SocketMessage MsgArg)
                                    {
                                        var ThisGuild = ((SocketGuildChannel)MsgArg.Channel).Guild;
                                        if (!MsgArg.Author.IsBot && MsgArg.Author.Id == Target && MsgArg.Channel.Id == arg.Channel.Id && AUsers.ContainsKey(Target) && AUsers[Target] == ThisGuild.Id)
                                        {
                                            MsgCount++;
                                            MsgArg.DeleteAsync();
                                            MsgOfMGX.ModifyAsync(xxx =>
                                            {
                                                xxx.Content = $"Blocked {MsgCount} messages from quarantine - No infected messages here!";
                                            });
                                        }
                                        if (!AUsers.ContainsKey(Target))
                                        {
                                            Program.Client.MessageReceived -= MessageEv;
                                            Thread.CurrentThread.Abort();
                                        }
                                        else if (!MsgArg.Author.IsBot && MsgArg.Author.Id == Target && MsgArg.Channel.Id == arg.Channel.Id && !AUsers.ContainsKey(Target))
                                        {
                                            Program.Client.MessageReceived -= MessageEv;
                                            Thread.CurrentThread.Abort();
                                        }
                                        return Task.CompletedTask;
                                    }
                                    Program.Client.MessageReceived += MessageEv;
                                }
                                else
                                {
                                    await arg.Channel.SendMessageAsync($"<@{Target}> is not going to be quarantined. Congrats!\n~~you will die soon~~");
                                    Thread.CurrentThread.Abort();
                                }
                            }).Start();
                        }
                    }
                }
                else if (GuildStuff.ReadRank(((SocketTextChannel)arg.Channel).Guild.Id, arg.Author.Id) >= 19)
                {
                    await arg.Channel.SendMessageAsync("Would you like to enable this command for this server?");
                    var WFR = await new WaitForResponse() { TimeLimitS = 30, ChannelId = arg.Channel.Id, UserId = arg.Author.Id }.Start();
                    if (WFR != null && WFR.Content.ToLower().Contains("yes"))
                    {
                        await arg.Channel.SendMessageAsync($"Enabled {Program.Prefix}quarantine as a command!");
                        GuildStuff.NewGuildSetting(GuildStuff.GuildSettings.CanUseQuarantine, ((SocketTextChannel)arg.Channel).Guild.Id, true);
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync($"👌");
                    }
                }
                else
                {
                    await arg.Channel.SendMessageAsync("Quarantine can't be used in this server!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

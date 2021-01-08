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
using System.Drawing;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class globalchat
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "settings",
            Premium = false,
            RequiredPermissions = 19
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await DefSet(arg, false);
        }

        public static async Task DefSet(SocketMessage arg, bool SecondCheck)
        {
            try
            {
                var Guild = ((SocketGuildChannel)arg.Channel).Guild;
                bool GlobalChatEnabled = GuildStuff.ReadSetting(GuildStuff.GuildSettings.GlobalChannelForOthers, Guild.Id);
                if (arg.MentionedChannels.Count > 0)
                {
                    var NewGlobalChannel = (SocketTextChannel)arg.MentionedChannels.First();
                    GuildStuff.NewGuildSetting(GuildStuff.GuildSettings.GlobalChannelForOthers, Guild.Id, true, NewGlobalChannel.Id);
                    await arg.Channel.SendMessageAsync($"{NewGlobalChannel.Mention} is now MGX's global chat. Send a message there and it'll be sent to all other MGX chats in other servers.");
                }
                else if (GlobalChatEnabled && !SecondCheck)
                {
                    GuildStuff.NewGuildSetting(GuildStuff.GuildSettings.GlobalChannelForOthers, Guild.Id, false);
                    await arg.Channel.SendMessageAsync($"Global chat for this server has been removed.");
                }
                else
                {
                    await arg.Channel.SendMessageAsync($"Please mention a channel to use as MGX's global chat.");
                    var Response = await new WaitForResponse()
                    {
                        TimeLimitS = 30,
                        ChannelId = arg.Channel.Id,
                        UserId = arg.Author.Id
                    }.Start();
                    if (Response != null && Response.MentionedChannels.Count > 0)
                    {
                        var NewGlobalChannel = (SocketTextChannel)Response.MentionedChannels.First();
                        GuildStuff.NewGuildSetting(GuildStuff.GuildSettings.GlobalChannelForOthers, Guild.Id, true, NewGlobalChannel.Id);
                        await arg.Channel.SendMessageAsync($"{NewGlobalChannel.Mention} is now MGX's global chat. Send a message there and it'll be sent to all other MGX chats in other servers.");
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync("No channel set!");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task MessageReceived(SocketMessage arg)
        {
            try
            {
                if (!arg.Author.IsBot && arg.Channel.GetType() == typeof(SocketTextChannel))
                {
                    var ThisGuild = ((SocketTextChannel)arg.Channel).Guild;
                    bool ThisGlobalChatEnabled = GuildStuff.ReadSetting(GuildStuff.GuildSettings.GlobalChannelForOthers, ThisGuild.Id);
                    if (ThisGlobalChatEnabled)
                    {
                        string Id = GuildStuff.ReturnExtraSaved(GuildStuff.GuildSettings.GlobalChannelForOthers, ThisGuild.Id);
                        SocketTextChannel ThisGlobalChatChannel = ThisGuild.GetTextChannel(ulong.Parse(Id));
                        if (ThisGlobalChatChannel.Id == arg.Channel.Id)
                        {
                            bool MsgUsed = false;
                            EmbedBuilder Embed;
                            var UserDataForEmb = StoreHandler.LoadData(arg.Author.Id, "GBCHATEMBED");
                            if (UserDataForEmb != null)
                            {
                                string AuthorForEmb = UserDataForEmb.Split(new[] { "author.name=" }, StringSplitOptions.None)[1].Split('=')[0];
                                System.Drawing.Color HexColor = ColorTranslator.FromHtml($"#{UserDataForEmb.Split(new[] { "embcolor=" }, StringSplitOptions.None)[1].Split('=')[0]}");
                                string FooterMsg = UserDataForEmb.Split(new[] { "footer.text=" }, StringSplitOptions.None)[1].Split('=')[0];
                                Embed = new EmbedBuilder()
                                {
                                    Author = new EmbedAuthorBuilder()
                                    {
                                        IconUrl = arg.Author.GetAvatarUrl() ?? Program.Client.CurrentUser.GetAvatarUrl(),
                                        Name = $"{arg.Author.Username} • [{AuthorForEmb}]"
                                    },
                                    Color = new Discord.Color(HexColor.R, HexColor.G, HexColor.B),
                                    Description = arg.Content,
                                    Footer = new EmbedFooterBuilder()
                                    {
                                        Text = FooterMsg.Length > 0 ? $"{arg.Author.Id} • [{FooterMsg}]" : arg.Author.Id.ToString()
                                    },
                                };
                            }
                            else
                            {
                                Embed = new EmbedBuilder()
                                {
                                    Author = new EmbedAuthorBuilder()
                                    {
                                        IconUrl = arg.Author.GetAvatarUrl() ?? Program.Client.CurrentUser.GetAvatarUrl(),
                                        Name = arg.Author.Username
                                    },
                                    Color = Discord.Color.Teal,
                                    Description = arg.Content,
                                    Footer = new EmbedFooterBuilder()
                                    {
                                        Text = arg.Author.Id.ToString(),
                                    },
                                };
                            }
                            bool Passed = false;
                            foreach (var Guild in Program.GetAllShardGuilds())
                            {
                                try
                                {
                                    bool GlobalChatEnabled = GuildStuff.ReadSetting(GuildStuff.GuildSettings.GlobalChannelForOthers, Guild.Id);
                                    if (GlobalChatEnabled)
                                    {
                                        SocketTextChannel GlobalChatChannel = Guild.GetTextChannel(ulong.Parse(GuildStuff.ReturnExtraSaved(GuildStuff.GuildSettings.GlobalChannelForOthers, Guild.Id)));
                                        if (Guild != ThisGuild)
                                        {
                                            var MsgF = MessageFilter(arg);
                                            if (MsgF.MsgPassed)
                                            {
                                                Passed = MsgF.MsgPassed;
                                                await GlobalChatChannel.SendMessageAsync("", false, Embed.Build());
                                            }
                                            else if (!MsgUsed)
                                            {
                                                MsgUsed = true;
                                                await arg.Channel.SendMessageAsync($"Sorry! Your message contained content that was deemed inappropriate - {MsgF.Reason}");
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                            if (new Random().Next(0, 100) > 98)
                            {
                                await arg.Channel.SendMessageAsync("`pls tell people about me if you like me` :pleading_face: ");
                            }
                            if (Passed)
                            {
                                await ((SocketUserMessage)arg).AddReactionAsync(new Emoji("\u2705"));
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private static MsgFilter MessageFilter(SocketMessage arg)
        {
            var ToRet = new MsgFilter
            {
                MsgPassed = false
            };
            if (StoreHandler.LoadData(arg.Author.Id, $"GLOBALCHAT-BAN") == null)
            {
                if (!(arg.Attachments.Count > 0))
                {
                    if (!(arg.Content.ToLower().Contains("http") && arg.Content.ToLower().Contains("discord")) && !(arg.Content.ToLower().Contains("discord") && arg.Content.ToLower().Contains("inv")) && !arg.Content.ToLower().Contains("discord.gg") && !arg.Content.ToLower().Contains(".gg/"))
                    {
                        ToRet.MsgPassed = true;
                    }
                    else
                    {
                        ToRet.Reason = "Discord invites can't be sent through here, let's keep this place as ad-free as possible.";
                    }
                }
                else
                {
                    ToRet.Reason = $"Images/files can't be sent through global chat - *Most* links can though!";
                }
            }
            else
            {
                ToRet.Reason = $"You have been banned from global chat for this reason: - {StoreHandler.LoadData(arg.Author.Id, $"GLOBALCHAT-BAN")}";
            }
            return ToRet;
        }
    }

    
}

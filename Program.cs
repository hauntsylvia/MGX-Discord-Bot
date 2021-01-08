using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Discord;
using Discord.WebSocket;
using MGX_Discord_Bot.EModules.Commands.General;
using MGX_Discord_Bot.EModules.Scheduler;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;
using MGX_Discord_Bot.EModules.Entities;
using MGX_Discord_Bot.EModules._X;
using System.IO;
using DiscordBotsList.Api;
using DiscordBotsList;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MGX_Discord_Bot
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static WebClient WebClient = new WebClient();

        /// <summary>
        /// MGX's owner's account.
        /// </summary>
        public static SocketUser MyAccount;
        /// <summary>
        /// MGX's home server.
        /// </summary>
        public static SocketGuild VoidServer;
        public static DiscordShardedClient Client;

        public static AuthDiscordBotListApi DiscordBotListClient;

        public static Dictionary<string, int> RateCache = new Dictionary<string, int>();

        public static HostingProperties HostP = new HostingProperties();
        public static string BasePropertiesString = $"AutoClose={HostP.AutoClose} #0 or 5\nUpdateURL={HostP.UpdateURL} #URL to use whenever update is invoked";

        public static string Prefix = "+";
        public static string Eepers = "NjY2MDQxMzk1OTAzMDcwMjQw.XhuZYw.-QsS9jAwVwXiEeWAR7bKOx8FT9E"; //new mgx
        //public static string Eepers = "NTM0MTkzMTkyMzI5ODA1ODU0.Xt1nNg.cYfN03PsVQw1mHy_lm1n7mVUZHQ"; //mgx
        //public static string Eepers = "Njk1Nzg5MjIwNDI1NDk4NjI1.XofSOg.Gl0e3pljaNXv3eMDhUuom6ZD8uc"; //deb

        static void Main()
        {
            ReadSettings();
            new Thread(x =>
            {
                while(true)
                {
                    ReadSettings();
                    try
                    {
                        int Close = int.Parse(HostP.AutoClose);
                        if(Close != 0 && Close != 5)
                            throw new FormatException("AutoClose property can only be 0 or 5.");
                        ShowWindow(GetConsoleWindow(), Close);
                    }
                    catch(Exception E)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(E);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
            MainAsync(Eepers).GetAwaiter().GetResult();
            Console.WriteLine("Task finished.");
            Main();
        }
        public static void ReadSettings()
        {
            try
            {
                if(File.Exists(@".\hosting.properties"))
                {
                    using (StreamReader SR = new StreamReader(@".\hosting.properties"))
                    {
                        var str = SR.ReadToEnd();
                        foreach (var Line in str.Split('\n'))
                        {
                            var Key = Line.Split('=')[0];
                            var Value = Line.Split('=')[1];
                            if (Value.Contains("#"))
                                Value = Value.Substring(0, Value.IndexOf('#'));
                            HostP.GetType().GetField(Key).SetValue(HostP, Value);
                        }
                    }
                }
                else
                {
                    using (StreamWriter SW = new StreamWriter(@".\hosting.properties"))
                    {
                        SW.Write(BasePropertiesString);
                    }
                }
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error while reading host properties!\n{e}");
                Console.ForegroundColor = ConsoleColor.White;
                HostP = new HostingProperties();
                using (StreamWriter SW = new StreamWriter(@".\hosting.properties"))
                {
                    SW.Write(BasePropertiesString);
                }
            }
        }

        public async static Task MainAsync(string Token)
        {
            MGXStatistics.StartedAt = DateTime.UtcNow;
            var config = new DiscordSocketConfig { MessageCacheSize = 5, TotalShards = 1 };
            Client = new DiscordShardedClient(config);
            DiscordBotListClient = new AuthDiscordBotListApi(534193192329805854, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjUzNDE5MzE5MjMyOTgwNTg1NCIsImJvdCI6dHJ1ZSwiaWF0IjoxNTg3NDI3NzQxfQ.lU-e43AqYswMC3UZsc145RgadqEhrrrelKoSCvT8UXg");
            //(await DiscordBotListClient.GetMeAsync()).
            CustomResponses.UpdateResponses();
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
            Client.MessageReceived += Client_MessageReceived;
            Client.MessageReceived += globalchat.MessageReceived;
            Client.MessageReceived += snipe.OnReceived;
            Client.MessageReceived += Secondary.MyClient_MessageReceived;
            Client.ShardReady += Client_Ready;
            Client.JoinedGuild += Client_JoinedGuild;
            Client.LeftGuild += Client_LeftGuild;
            Client.MessageDeleted += snipe.OnDelete;
            new Thread(async x =>
            {
                await Secondary.Secondary_Thread();
            }).Start();

            await Task.Delay(-1);
        }
        public static SocketGuild[] GetAllShardGuilds()
        {
            var ToArr = new List<SocketGuild>();
            foreach (var Client in Client.Shards)
            {
                ToArr.AddRange(Client.Guilds);
            }
            return ToArr.ToArray();
        }
        private static async Task Client_LeftGuild(SocketGuild arg)
        {
            var Log = new EmbedBuilder()
            {
                Color = new Color(255, 100, 100),
                Description = $"Left guild: {arg.Name}",
                Timestamp = DateTime.UtcNow,
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"{arg.Id}"
                },
                ThumbnailUrl = arg.IconUrl ?? Client.CurrentUser.GetAvatarUrl(),
            };
            LogEvent.SendLog(Log);
        }

        private static async Task Client_JoinedGuild(SocketGuild arg)
        {
            var List = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder()
                {
                    Name = "Thank you!",
                    Value = "Thank you for inviting me! 💜 - I'm still learning and growing, so it means a lot."
                },
                new EmbedFieldBuilder()
                {
                    Name = "Links:",
                    Value = $"[MGX's Server](https://discord.gg/yPGer64)\n[Invite](https://discordapp.com/oauth2/authorize?client_id={Client.CurrentUser.Id}&scope=bot&permissions=8)"
                }
            };
            EmbedBuilder ToSend = new EmbedBuilder()
            {
                Color = Color.Teal,
                Fields = List,
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"MGX"
                },
                Timestamp = DateTime.UtcNow
            };

            await arg.TextChannels.First().SendMessageAsync("", false, ToSend.Build());

            var Builder = new EmbedBuilder()
            {
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"MGX • {DateTime.UtcNow.ToShortTimeString()}"
                },
                Color = Color.Teal,
                Title = $"Thank you, {arg.Owner.Username}",
                Description = $"Thank you so much for using MGX!  :pleading_face:  - To get started, simply use `{Prefix}help` in your server.\n\nIf you need any support, just [click here](https://discord.gg/yPGer64)"
            };
            await arg.Owner.SendMessageAsync("", false, Builder.Build());

            var Log = new EmbedBuilder()
            {
                Color = Color.DarkerGrey,
                Description = $"Joined guild: {arg.Name}",
                Timestamp = DateTime.UtcNow,
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"{arg.Id}"
                },
                ThumbnailUrl = arg.IconUrl ?? Client.CurrentUser.GetAvatarUrl(),
            };
            LogEvent.SendLog(Log);

            await Client.SetGameAsync($"{Client.Guilds.Count} Guilds | {Prefix}help", null, ActivityType.Watching);
            await DiscordBotListClient.UpdateStats(Client.Guilds.Count);
        }


        
        //FIRED ONCE FOR EVERY SHARD ON STARTUP
        private static async Task Client_Ready(DiscordSocketClient Sclient)
        {
            Console.WriteLine("Shard started");
            await Client.SetStatusAsync(UserStatus.Idle);
            await Client.SetGameAsync($"{Prefix}help | {Client.Guilds.Count} Guilds", null, ActivityType.Watching);
            MyAccount = Client.GetUser(528750326107602965);
            VoidServer = Client.GetGuild(687266658771140608);
        }



        //COMMAND HANDLING
        private static async Task Client_MessageReceived(SocketMessage arg)
        {
            try
            {
                if (!arg.Author.IsBot && arg.Channel.GetType() == typeof(SocketTextChannel))
                {
                    var Guild = ((SocketGuildChannel)arg.Channel).Guild;
                    if (arg.Content.ToLower().StartsWith(Prefix))
                    {
                        var Cmd = Type.GetType($"MGX_Discord_Bot.EModules.Commands.General.{arg.Content.Split(' ')[0].Split(new[] { Prefix }, StringSplitOptions.None)[1].ToLower()}");
                        int UserGuildRank = GuildStuff.ReadRank(Guild.Id, arg.Author.Id);
                        int UserMGXRank = RankingManager.BotOwners.ContainsKey(arg.Author.Id) ? RankingManager.BotOwners[arg.Author.Id] : 1;
                        if (Cmd != null)
                        {
                            var CmdInfo = (CommandInformation)Cmd.GetField("Info").GetValue(null);
                            if (CmdInfo.Premium && false) // change false to check database for user if they have premium
                            {
                                await arg.Channel.SendMessageAsync("MGX premium is required for this command :pensive:");
                            }
                            else if (CmdInfo.RequiredPermissions <= UserGuildRank)
                            {
                                Cmd.GetMethod("Maindo").Invoke(null, new[] { arg });
                                int CountAlr = StoreHandler.LoadData(arg.Author.Id, $"CMD-COUNT") != null ? int.Parse(StoreHandler.LoadData(arg.Author.Id, $"CMD-COUNT")) : 0;
                                StoreHandler.SaveData(arg.Author.Id, "CMD-COUNT", CountAlr + 1);
                            }
                            else
                            {
                                await arg.Channel.SendMessageAsync($"You aren't able to run this! You must be `{GuildStuff.GetNameOfRank(CmdInfo.RequiredPermissions)}` or higher.");
                            }
                            int CurCmd = StoreHandler.LoadData(0, "cmdcount") == null ? 0 : int.Parse(StoreHandler.LoadData(0, "cmdcount"));
                            StoreHandler.SaveData(0, "cmdcount", CurCmd + 1);
                        }
                        else if (RankingManager.BotOwners.ContainsKey(arg.Author.Id))
                        {
                            Cmd = Type.GetType($"MGX_Discord_Bot.EModules.Commands.Maintenance.{arg.Content.Split(' ')[0].Split(new[] { Prefix }, StringSplitOptions.None)[1].ToLower()}");
                            var CmdInfo = (CommandInformation)Cmd.GetField("Info").GetValue(null);
                            if (Cmd != null && CmdInfo.RequiredPermissions <= UserMGXRank)
                            {
                                Cmd.GetMethod("Maindo").Invoke(null, new[] { arg });
                            }
                            else
                            {
                                Console.WriteLine("Maint null");
                            }
                        }
                    }
                    // CUSTOM RESPONSES \\
                    if (GuildStuff.ReadSetting(GuildStuff.GuildSettings.CustomResponses, Guild.Id))
                    {
                        var ReturnedWordResp = CustomResponses.ReturnResponse(arg.Content);
                        if (ReturnedWordResp != null)
                        {
                            await arg.Channel.SendMessageAsync(ReturnedWordResp);
                        }
                        else if (arg.Content.ToLower().StartsWith("ree"))
                        {
                            await arg.Channel.SendMessageAsync("*r e e ?*");
                        }
                        else if (arg.Content.ToLower().Contains("anti") && arg.Content.ToLower().Contains("va") || arg.Content.ToLower().Contains("vac") && arg.Content.ToLower().Contains("do") && arg.Content.ToLower().Contains("nt"))
                        {
                            await arg.Channel.SendMessageAsync($"i swear to fucking god you better vaccinate your kids");
                        }
                        else if (arg.Content.ToLower().Contains("sksk") | arg.Content.ToLower().Contains(" oop ") | arg.Content.ToLower().Contains("ess kay") | arg.Content.ToLower().Contains("esskay"))
                        {
                            await arg.Channel.SendMessageAsync("ew, vsco. begone thot");
                        }
                        else if (arg.Content.ToLower().StartsWith("i am ") || arg.Content.ToLower().StartsWith("im ") || arg.Content.ToLower().StartsWith("i'm "))
                        {
                            string WhatTheyAre = arg.Content.Split(new string[] { "m" }, 2, StringSplitOptions.None)[1];
                            await arg.Channel.SendMessageAsync($"hi {WhatTheyAre.Trim()}, im dad");
                        }
                        CustomResponses.UpdateResponses();
                    }
                    if (GuildStuff.ReadSetting(GuildStuff.GuildSettings.ReplaceInvites, Guild.Id) && arg.Content.ToLower().Contains("discord.gg/"))
                    {
                        var RepInvEm = new EmbedBuilder()
                        {
                            Color = new Color(0x36393F),
                            Timestamp = DateTime.UtcNow,
                            Author = new EmbedAuthorBuilder()
                            {
                                Name = $"{arg.Author.Username} has invited you to a guild!"
                            },
                        };
                    }
                }
                else if (!arg.Author.IsBot)
                {
                    await MyAccount.SendMessageAsync($"{arg.Author.Username}#{arg.Author.Discriminator}: {arg.Content}\n\n{arg.Author.Id}");
                    foreach (var Attachment in arg.Attachments)
                    {
                        await MyAccount.SendMessageAsync(Attachment.Url);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task<bool> SendEmbed(SocketMessage arg, string DescriptionS, Dictionary<string, string> AllVals, Color SendColor, string FooterText = "", SocketUser User = null, bool UseLastParamsPfp = false, string CustomPictureURL = "", bool InLine = true)
        {
            if(arg.MentionedUsers.Count() > 0)
            {
                User = arg.MentionedUsers.First();
            }
            else if(!UseLastParamsPfp)
            {
                User = Client.CurrentUser;
            }
            var Builder = new EmbedBuilder()
            {
                Color = SendColor,
                Description = DescriptionS
            };
            if(FooterText.Length > 0)
            {
                Builder.WithFooter(footer => footer.Text = FooterText);
            }
            foreach (string Key in AllVals.Keys)
            {
                Builder.AddField(Key, AllVals[Key], InLine);
            }
            if(CustomPictureURL.Length > 2)
            {
                Builder.WithThumbnailUrl(CustomPictureURL);
            }
            else
            {
                Builder.WithThumbnailUrl(User.GetAvatarUrl());
            }
            await arg.Channel.SendMessageAsync("", false, Builder.Build());
            return true;
        }
    }
}

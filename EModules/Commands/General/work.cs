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
    class work
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 1
        };
        private static Dictionary<ulong, double> NotDone = new Dictionary<ulong, double>();
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                string KeyForJob = $"JOB";
                if (StoreHandler.LoadData(arg.Author.Id, KeyForJob) != null)
                {
                    long ToAdd = long.Parse(StoreHandler.LoadData(arg.Author.Id, $"{KeyForJob}-PAY"));
                    Random Random = new Random();

                    double WaitTime = 3600;
                    if (!NotDone.ContainsKey(arg.Author.Id))
                    {
                        NotDone.Add(arg.Author.Id, WaitTime);
                        new Thread(x =>
                        {
                            while (true)
                            {
                                Thread.Sleep(1000);
                                NotDone[arg.Author.Id]--;
                                if (NotDone[arg.Author.Id] <= 0)
                                {
                                    NotDone.Remove(arg.Author.Id);
                                    break;
                                }
                            }
                        }).Start();
                        var Embed = new EmbedBuilder()
                        {
                            Color = Color.Teal,
                            Timestamp = DateTime.UtcNow,
                            Footer = new EmbedFooterBuilder()
                            {
                                Text = "MGX"
                            },
                            ThumbnailUrl = arg.Author.GetAvatarUrl() ?? Program.Client.CurrentUser.GetAvatarUrl(),
                        };

                        ShopClass.AddCurrency(arg.Author.Id, ToAdd);

                        var Field = new EmbedFieldBuilder()
                        {
                            IsInline = false,
                            Name = $"`{Program.Prefix}work`",
                            Value = $"Here's your pay! - `${ToAdd}`\nBalance - `${ShopClass.ReadCurrency(arg.Author.Id)}`"
                        };
                        Embed.AddField(Field);
                        await arg.Channel.SendMessageAsync("", false, Embed.Build());
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync($"Hot hands! Gotta wait! `{(NotDone[arg.Author.Id] / 60).ToString().Substring(0, 3)}min` left.");
                    }
                }
                else
                {
                    var Prompt = new Prompt()
                    {
                        ChannelForPrompt = arg.Channel,
                        MaxTime = 15,
                        Options = new List<Emoji>()
                        {
                            { new Emoji("\uD83D\uDC4D") },
                            { new Emoji("\uD83D\uDC4E") }
                        }.ToArray(),
                        Target = arg.Author,
                        Title = "You don't have a job (noob), wanna get one?"
                    };

                    var Response = await Prompt.ShowPrompt();

                    if (Response == null || Response.Name != "👍")
                    {
                        await arg.Channel.SendMessageAsync("you're broke but ok.");
                    }
                    else
                    {
                        await joblist.Maindo(arg);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    class quit
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            string KeyForJob = "JOB";
            if (StoreHandler.LoadData(arg.Author.Id, KeyForJob) != null)
            {
                await arg.Channel.SendMessageAsync($"You have quit your current occupation: `{StoreHandler.LoadData(arg.Author.Id, KeyForJob)}`");

                StoreHandler.RemoveData(arg.Author.Id, KeyForJob);
            }
            else
            {
                await arg.Channel.SendMessageAsync($"You kind of need a job to quit your job");
            }
        }
    }

    class job
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            string KeyForJob = $"JOB";
            if (StoreHandler.LoadData(arg.Author.Id, KeyForJob) != null)
            {
                var Job = UserStuff.GetJob(arg.Author.Id);
                var Embed = new EmbedBuilder()
                {
                    Author = new EmbedAuthorBuilder()
                    {
                        IconUrl = arg.Author.GetAvatarUrl(),
                        Name = $"{arg.Author.Username}'s Job",
                    },
                    Color = new Color(85, 255, 157),
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = Job.Timestamp >= new DateTime(2010, 1, 1) ? $"Has had job for {(DateTime.UtcNow - Job.Timestamp).Days} days" : "MGX"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        {
                            new EmbedFieldBuilder()
                            {
                                Name = "Name",
                                Value = $"{Job.Name}"
                            }
                        },
                        {
                            new EmbedFieldBuilder()
                            {
                                Name = "Pay",
                                Value = $"${Job.Pay}"
                            }
                        },
                        {
                            new EmbedFieldBuilder()
                            {
                                Name = $"People w/{Job.Name}",
                                Value = $"{StoreHandler.ReturnCountOfPattern(StoreHandler.LoadData(arg.Author.Id, KeyForJob))}"
                            }
                        },
                        {
                            new EmbedFieldBuilder()
                            {
                                Name = $"Is Available",
                                Value = $"{Job.StillAvailable}"
                            }
                        },
                    }
                };

                await arg.Channel.SendMessageAsync("", false, Embed.Build());
            }
            else
            {
                await arg.Channel.SendMessageAsync($"You kind of need a job to view your job: `{Program.Prefix}joblist`");
            }
        }
    }

    class jobtrade
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await arg.Channel.SendMessageAsync("Ping the person you want to trade with!");
            SocketUser TradingWith = (await new WaitForResponse()
            {
                TimeLimitS = 10,
                ChannelId = arg.Channel.Id,
                UserId = arg.Author.Id
            }.Start()).MentionedUsers.First();
            IEmote Response = await new Prompt()
            {
                ChannelForPrompt = arg.Channel,
                MaxTime = 15,
                Options = new List<Emoji>()
                {
                    { new Emoji("\uD83D\uDC4D") },
                    { new Emoji("\uD83D\uDC4E") }
                }.ToArray(),
                Target = TradingWith,
                Title = $"Do you ({TradingWith.Mention}) accept to trade your job with ({arg.Author.Mention})?",
            }.ShowPrompt();
            if(Response != null && Response.Name == "👍")
            {
                Entities.job TradersJob = UserStuff.GetJob(arg.Author.Id);
                Entities.job TradeesJob = UserStuff.GetJob(TradingWith.Id);
                if(TradersJob != null && TradeesJob != null)
                {
                    UserStuff.SaveJob(arg.Author.Id, TradeesJob);
                    UserStuff.SaveJob(TradingWith.Id, TradersJob);
                    await arg.Channel.SendMessageAsync("Successfully traded jobs!");
                }
                else
                {
                    await arg.Channel.SendMessageAsync("One or more users don't have a job.");
                }
            }
        }
    }

    class joblist
    {
        private static Emoji OneEm = new Emoji("1\u20e3");
        private static Emoji TwoEm = new Emoji("2\u20e3");
        private static Emoji ThrEm = new Emoji("3\u20e3");
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            List<Entities.job> RandomChosen = new List<Entities.job>()
            {
                { UserStuff.GenerateRandomJob(false) },
                { UserStuff.GenerateRandomJob(false) },
            };
            var Embed = new EmbedBuilder()
            {
                Color = new Color(251, 125, 255),
                Description = "Job List",
                Footer = new EmbedFooterBuilder()
                {
                    Text = arg.Author.Username,
                    IconUrl = arg.Author.GetAvatarUrl() != null ? arg.Author.GetAvatarUrl() : Program.Client.CurrentUser.GetAvatarUrl(),
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                {
                    {
                        new EmbedFieldBuilder()
                        {
                            IsInline = false,
                            Name = "1. " + RandomChosen[0].Name,
                            Value = $"`Pay - ${RandomChosen[0].Pay}`",
                        }
                    },
                    {
                        new EmbedFieldBuilder()
                        {
                            IsInline = false,
                            Name = "2. " + RandomChosen[1].Name,
                            Value = $"`Pay - ${RandomChosen[1].Pay}`",
                        }
                    }
                },
            };

            var JobList = new Prompt()
            {
                ChannelForPrompt = arg.Channel,
                MaxTime = 15,
                Options = new List<Emoji>()
                {
                    {OneEm},
                    {TwoEm},
                }.ToArray(),
                Target = arg.Author,
                Embed = Embed,
            };

            var JobChosen = await JobList.ShowPrompt();
            Console.WriteLine(JobChosen);

            if(JobChosen != null)
            {
                if (StoreHandler.LoadData(arg.Author.Id, $"JOB") != null)
                {
                    var Prompt = new Prompt()
                    {
                        ChannelForPrompt = arg.Channel,
                        MaxTime = 15,
                        Options = new List<Emoji>()
                        {
                            { new Emoji("\uD83D\uDC4D") },
                            { new Emoji("\uD83D\uDC4E") }
                        }.ToArray(),
                        Target = arg.Author,
                        Title = "You have to quit your current job to get a new one. Continue?"
                    };

                    var Answer = await Prompt.ShowPrompt();
                    if(Answer != null && Answer.Name == "👎")
                    {
                        await arg.Channel.SendMessageAsync("Nothing has changed.");
                        return;
                    }

                    await Prompt.Message.DeleteAsync();
                }
                if (JobChosen.Name == OneEm.Name)
                {
                    UserStuff.SaveJob(arg.Author.Id, RandomChosen[0]);
                }
                else if(JobChosen.Name == TwoEm.Name)
                {
                    UserStuff.SaveJob(arg.Author.Id, RandomChosen[1]);
                }
                else
                {
                    return;
                }
                await arg.Channel.SendMessageAsync($"Congrats! You're now listed as: `{UserStuff.GetJob(arg.Author.Id).Name}`");
            }

            await JobList.Message.DeleteAsync();
        }
    }
}

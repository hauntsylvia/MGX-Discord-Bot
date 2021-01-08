using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
    class date
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
            try
            {
                var User = arg.MentionedUsers.Count > 0 ? arg.MentionedUsers.First() : null;
                if (User != null)
                {
                    if (User.Id == arg.Author.Id)
                    {
                        await arg.Channel.SendMessageAsync("You can't do that <:MegaFlushed:687278501274845201>");
                        return;
                    }
                    string Key = $"DATINGDATA";
                    bool HasPartner = StoreHandler.LoadData(User.Id, Key) != null;
                    ulong PartnerOfMentionedId = HasPartner ? ulong.Parse(StoreHandler.LoadData(User.Id, Key)) : 0;

                    bool AuthorHasPartner = StoreHandler.LoadData(arg.Author.Id, Key) != null;
                    ulong PartnerOfAuthorId = AuthorHasPartner ? ulong.Parse(StoreHandler.LoadData(arg.Author.Id, Key)) : 0;
                    if (!HasPartner)
                    {
                        if (!AuthorHasPartner)
                        {
                            var Prompt = new Prompt()
                            {
                                ChannelForPrompt = arg.Channel,
                                MaxTime = 15,
                                Options = new List<Emoji>()
                                {
                                    { new Emoji("\uD83D\uDC4D") },
                                    { new Emoji("\uD83D\uDC4E") },
                                }.ToArray(),
                                Target = User,
                                Title = $"{User.Mention} has 15 seconds to accept!",
                            };
                            IEmote Answer = await Prompt.ShowPrompt();
                            if (Answer == null)
                            {
                                await arg.Channel.SendMessageAsync($"Sorry, the answer is a no );");
                            }
                            else if (Answer.Name == "👍")
                            {
                                await arg.Channel.SendMessageAsync($"{arg.Author.Mention} and {User.Mention} are now dating.");
                                StoreHandler.SaveData(arg.Author.Id, Key, User.Id);
                                StoreHandler.SaveData(User.Id, Key, arg.Author.Id);
                            }
                            else
                            {
                                await arg.Channel.SendMessageAsync($"Sorry, the answer is a no );");
                            }
                        }
                        else
                        {
                            var PartnerOfAuthor = Program.Client.GetUser(PartnerOfMentionedId);
                            if (PartnerOfAuthor.Id == User.Id)
                            {
                                await arg.Channel.SendMessageAsync($"You're both already dating-");
                            }
                            else
                            {
                                await arg.Channel.SendMessageAsync($"{PartnerOfAuthor.Mention} probably wouldn't like that <:CoolFlushed:687278501287821400>");
                            }
                        }
                    }
                    else
                    {
                        var PartnerOfMentioned = Program.Client.GetUser(PartnerOfMentionedId);
                        await arg.Channel.SendMessageAsync($"{User.Mention} is with someone already, sorry!");
                    }
                }
                else
                {
                    await arg.Channel.SendMessageAsync($"Please mention a user.");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

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
    class steal
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 0
        };
        private static List<ulong> Wait = new List<ulong>();
        private static int WaitTime_Minutes = 120;
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                SocketUser Victim = arg.MentionedUsers.Count() > 0 ? arg.MentionedUsers.First() : null;
                if (Victim != null && Victim.Id != arg.Author.Id)
                {
                    long AuthorBalance = ShopClass.ReadCurrency(arg.Author.Id);
                    long VictimBalance = ShopClass.ReadCurrency(Victim.Id);

                    string VictimPadlockKey = $"PADLOCK-ACTIVE";
                    bool VictimHasPadlock = StoreHandler.LoadData(Victim.Id, VictimPadlockKey) != null;

                    if (AuthorBalance >= 1000 && VictimBalance >= 1000)
                    {
                        if (Wait.Contains(arg.Author.Id))
                        {
                            await arg.Channel.SendMessageAsync($"You have to wait! You're robbing people too much, maybe hang low for a bit? :weary:");
                        }
                        else
                        {
                            new Thread(x =>
                            {
                                Wait.Add(arg.Author.Id);
                                Thread.Sleep(WaitTime_Minutes * 60000);
                                Wait.Remove(arg.Author.Id);
                            }).Start();
                            if(VictimHasPadlock)
                            {
                                int Lost = (int)(AuthorBalance * 0.1);
                                await arg.Channel.SendMessageAsync($"Your victim had a padlock! You had no chance );  ..  Lost 10% of your balance (`${Lost}`)");
                                StoreHandler.RemoveData(Victim.Id, VictimPadlockKey);
                                Thread.Sleep(3500);
                                await arg.Channel.TriggerTypingAsync();
                                Thread.Sleep(3500);
                                await arg.Channel.SendMessageAsync($"My bad! Ima ping you so you know your padlock is gone noob <@{Victim.Id}>");
                            }
                            else
                            {
                                Random Ran = new Random();
                                int SuccessRate = Ran.Next(0, 20) - 10;
                                int Took = (int)(((double)SuccessRate) / 100 * VictimBalance);
                                int LostIfFail = (int)(((double)SuccessRate) / 100 * AuthorBalance);

                                if (SuccessRate >= 0)
                                {
                                    ShopClass.AddCurrency(Victim.Id, -Took);
                                    ShopClass.AddCurrency(arg.Author.Id, Took);
                                    await arg.Channel.SendMessageAsync($"Hope you're happy with your trash life decisions, take the money you stole: `${Took}`");
                                }
                                else
                                {
                                    ShopClass.AddCurrency(arg.Author.Id, LostIfFail);
                                    await arg.Channel.SendMessageAsync($"For once you thought maybe you were good at something. Keep looking, you're terrible at this; you got caught. Paid `${LostIfFail}` to the authorities so they'd leave you alone.");
                                }
                            }
                        }
                    }
                    else if (VictimBalance <= 1000 && AuthorBalance >= 1000)
                    {
                        await arg.Channel.SendMessageAsync($"You're gonna steal basically everything they have. :pleading_face: (Victim must have over `$1000`)");
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync($"You need more than `$1000` to rob someone.");
                    }
                }
                else if(Victim != null && arg.Author.Id == Victim.Id)
                {
                    await arg.Channel.SendMessageAsync($"you cant rob yourself - please mention a *different* user.");
                }
                else
                {
                    await arg.Channel.SendMessageAsync($"lol you cant rob yourself - please mention a user.");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

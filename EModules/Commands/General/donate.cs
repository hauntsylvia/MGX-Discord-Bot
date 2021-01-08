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
    class donate
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
            if (arg.MentionedUsers.Count > 0)
            {
                var User = arg.MentionedUsers.First();
                try
                {
                    string[] args = arg.Content.ToLower().Split(' ');
                    foreach(string sarg in args)
                    {
                        if(long.TryParse(sarg, out long DonNum))
                        {
                            if (ShopClass.ReadCurrency(arg.Author.Id) >= DonNum)
                            {
                                if (DonNum < 0)
                                {
                                    await arg.Channel.SendMessageAsync("We don't do negative numbers here.");
                                    return;
                                }
                                if (User.Id == arg.Author.Id)
                                {
                                    await arg.Channel.SendMessageAsync("Look, I know you want money quickly .. But you're broke, and you're most likely always going to be that way. Don't donate to yourself kids.");
                                    return;
                                }
                                bool Success = await ShopClass.SpendCurrency(arg.Author.Id, DonNum, arg.Channel);

                                if (Success)
                                {
                                    await arg.Channel.SendMessageAsync($"You've donated ${DonNum} to {User.Username} .. Your new balance is ${ShopClass.ReadCurrency(arg.Author.Id)}");
                                    ShopClass.AddCurrency(User.Id, DonNum);
                                }
                            }
                            else
                            {
                                await arg.Channel.SendMessageAsync($"You don't have ${DonNum} or more.");
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    await arg.Channel.SendMessageAsync($"Did you type a number? Usage: `{Program.Prefix}donate <amount> <user>`");
                }
            }
            else
            {
                await arg.Channel.SendMessageAsync("Please mention a user to donate to.");
            }
        }
    }
}

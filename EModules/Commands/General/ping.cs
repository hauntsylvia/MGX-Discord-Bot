using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
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
    class ping
    {
        private static List<ulong> Channels = new List<ulong>();

        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                if(!Channels.Contains(arg.Channel.Id))
                {
                    Channels.Add(arg.Channel.Id);

                    string Url = arg.Content.ToLower().Contains(' ') ? arg.Content.ToLower().Split(' ')[1] : "www.discordapp.com";

                    var Ping = new Ping();

                    var Reply = Ping.Send(Url.Trim(), 3500);

                    if (Reply.Status == IPStatus.Success)
                    {
                        var Msg = arg.Channel.SendMessageAsync($"Pinging {Url.Trim()}..").GetAwaiter().GetResult();

                        new Thread(async xThr =>
                        {
                            int Max = 4;
                            List<long> Results = new List<long>();
                            long AvgPing = -1;
                            for (int i = 1; i < Max; i++)
                            {
                                Thread.Sleep(4000);
                                long PingINT = Ping.Send(Url).RoundtripTime;
                                Results.Add(PingINT);
                                long AddedRes = 0;
                                foreach (long PingAttempt in Results)
                                {
                                    AddedRes += PingAttempt;
                                }

                                AvgPing = AddedRes / Results.Count;

                                await Msg.ModifyAsync(x =>
                                {
                                    x.Content = $"`{AvgPing}ms` (avg)\n`{Max - i}`attempts left";
                                });
                            }

                            await Msg.ModifyAsync(x =>
                            {
                                x.Content = $"Done pinging {Url} (`{Reply.Address}`) (`{AvgPing}ms`)";
                            });

                            Results = null;

                            Channels.Remove(arg.Channel.Id);
                        }).Start();
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync("I can't seem to reach that hostname - make sure it looks like so: `www.discordapp.com`");
                        Channels.Remove(arg.Channel.Id);
                    }
                }
                else
                {
                    await arg.Channel.SendMessageAsync($"To prevent bandwidth issues, you are being limited - Please wait for the current ping to finish");
                }
            }
            catch(Exception e)
            {
                if(Channels.Contains(arg.Channel.Id))
                {
                    Channels.Remove(arg.Channel.Id);
                }
                await arg.Channel.SendMessageAsync(e.Message);
            }
        }
    }
}

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
    class slowmode
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 18
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if (arg.Content.ToLower().Contains(' ') && Int32.TryParse(arg.Content.ToLower().Split(' ')[1], out int Time))
            {
                int Max = 21600;
                if (Time < Max && Time >= 0)
                {
                    var GuildChannel = (SocketTextChannel)(SocketGuildChannel)arg.Channel;

                    await GuildChannel.ModifyAsync(x =>
                    {
                        x.SlowModeInterval = Time;
                    });
                    string ToSend = Time == 0 ? $"Slowmode removed." : $"Slowmode added to this channel! Messages can be sent once every {Time} seconds.";
                    await arg.Channel.SendMessageAsync(ToSend);
                }
                else
                {
                    await arg.Channel.SendMessageAsync($"Can't have a number larger than {Max} or a number lower than 1");
                }
            }
            else
            {
                await arg.Channel.SendMessageAsync("Please type a valid number!");
            }
        }
    }
}

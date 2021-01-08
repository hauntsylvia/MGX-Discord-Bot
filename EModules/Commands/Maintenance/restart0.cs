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
    class restart0
    {
        private static double Duration = 10;
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                double.TryParse(arg.Content.ToLower().Split(' ')[1], out Duration);
                if(Duration <= 60)
                {
                    await arg.Channel.SendMessageAsync($"Logged out, restarting process (`{Process.GetCurrentProcess().Id}id`) (`{Duration}s`).");
                    await Program.Client.LogoutAsync();

                    string Args = $@"reopen ""{Assembly.GetExecutingAssembly().Location}"" {Duration}";
                    var ReOpen = new Process();

                    ReOpen.StartInfo = new ProcessStartInfo()
                    {
                        FileName = @".\MGXReOpen.exe",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = Args,
                    };

                    ReOpen.Start();
                }
                else
                {
                    await arg.Channel.SendMessageAsync("60 seconds or less.");
                }
            }
            catch(Exception e)
            {
                await arg.Channel.SendMessageAsync(e.Message);
            }
        }
    }

    class r_0
    {
        public static CommandInformation Info = restart0.Info;
        public static async Task Maindo(SocketMessage arg)
        {
            await restart0.Maindo(arg);
        }
    }
}

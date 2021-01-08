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
    class chkupd0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 10
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                using (WebClient Cl = new WebClient())
                {
                    var Msg = await arg.Channel.SendMessageAsync($"Downloading new file");
                    int Step = 0;
                    Thread Thr = new Thread(z =>
                    {
                        while(true)
                        {
                            Step += 1;
                            Thread.Sleep(1);
                            if (Step > 550)
                                Step = 0;
                        }
                    });
                    Thr.Start();
                    Cl.DownloadProgressChanged += (s, e) =>
                    {
                        if (Step == 500)
                        {
                            Step = 0;
                            try
                            {
                                Msg.ModifyAsync(x =>
                                {
                                    x.Content = $"Downloading new file {e.BytesReceived / 1e+6}mb/{e.TotalBytesToReceive / 1e+6}mb";
                                }).GetAwaiter().GetResult();
                            }
                            catch { }
                            Console.WriteLine($"Downloading new file {e.BytesReceived / 1e+6}mb/{e.TotalBytesToReceive / 1e+6}mb");
                        }
                    };
                    Cl.DownloadFileCompleted += (s, e) =>
                    {
                        Thr.Abort();
                        Console.WriteLine($"Done downloading file.");
                    };
                    string DLTo = Program.HostP.UpdateURL.Substring(Program.HostP.UpdateURL.LastIndexOf('.'), Program.HostP.UpdateURL.Length - Program.HostP.UpdateURL.LastIndexOf('.'));
                    
                    Cl.DownloadFileAsync(new Uri(Program.HostP.UpdateURL), $@".\NewBuild{DLTo}");
                }
            }
            catch(Exception E)
            {
                Console.WriteLine(E);
            }
        }
    }
    class update0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 19
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                if (arg.Content.Contains(' '))
                {
                    string Url = arg.Content.Split(' ')[1];
                    var Prompt = new Prompt()
                    {
                        ChannelForPrompt = arg.Channel,
                        MaxTime = 10,
                        Options = new Emoji[]
                            {
                                new Emoji("\uD83D\uDC4D"),
                                new Emoji("\uD83D\uDC4E")
                            },
                        Target = arg.Author,
                        Title = "Are you sure this is the correct url? (Double check.)"
                    };
                    var Answer = await Prompt.ShowPrompt();
                    if (Answer.Name != "👍")
                    {
                        await arg.Channel.SendMessageAsync("Operation stopped.");
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync("MGX will reopen after this process.");
                        await Start(Url);
                    }
                }
                else
                {
                    await arg.Channel.SendMessageAsync("No download url specified");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static async Task Start(string Url)
        {
            string Args = $@"update ""{Url}"" ""{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}""";
            var ReOpen = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = @".\MGXReOpen.exe",
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = Args,
                }
            };
            ReOpen.Start();
            await Program.Client.LogoutAsync();
            Environment.Exit(-1);
        }
    }
    class u_0
    {
        public static CommandInformation Info = update0.Info;

        public static async Task Maindo(SocketMessage arg)
        {
            await update0.Maindo(arg);
        }
    }

}

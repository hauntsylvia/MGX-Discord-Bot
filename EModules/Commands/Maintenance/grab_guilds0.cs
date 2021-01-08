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
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.Maintenance
{
    class grab_guilds0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 19
        };
        private static List<ulong> InProg = new List<ulong>();
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                if (!InProg.Contains(arg.Author.Id))
                {
                    string Path = $@".\{arg.Author.Id}.txt";
                    InProg.Add(arg.Author.Id);
                    await arg.Channel.SendMessageAsync("Compiling your file..");
                    using (StreamWriter SW = new StreamWriter(Path))
                    {
                        SW.WriteLine($"[{Program.Client.Guilds.Count} Guilds]\n\n");
                        foreach (var Guild in Program.Client.Guilds)
                        {
                            try
                            {
                                SW.WriteLine("Guild Name + Id - " + Guild.Name + $"({Guild.Id})");
                                SW.WriteLine("Guild Owner Name + Id - " + Guild.Owner.Username + $"({Guild.Owner.Id})");
                                SW.WriteLine("User Count - " + Guild.Users.Count);
                                string InvStr = "Invites - ";
                                foreach (var Channel in Guild.TextChannels)
                                {
                                    var Invs = await Channel.GetInvitesAsync();
                                    if (Invs.Count > 0)
                                    {
                                        foreach (var Inv in Invs)
                                        {
                                            InvStr += Inv + ", ";
                                        }
                                    }
                                }
                                SW.Write(InvStr.Substring(0, InvStr.Length - 2));
                            }
                            catch { }
                            SW.WriteLine("\n");
                        }
                    }
                    await arg.Author.SendFileAsync(Path);
                    InProg.Remove(arg.Author.Id);
                    File.Delete(Path);
                }
                else
                {
                    await arg.Channel.SendMessageAsync("Still compiling the file to send you!");
                }
            }
            catch(Exception e)
            {
                await arg.Author.SendMessageAsync(e.Message);
            }
        }
    }
}

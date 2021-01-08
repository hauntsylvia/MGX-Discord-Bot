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
    class mass_dm0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await arg.Channel.SendMessageAsync($@"This will direct message all users available.{"\n"}Type ""cancel"" to cancel this operation.");
            List<ulong> AlreadyUsersS = new List<ulong>();
            Program.Client.MessageReceived += Ev;
            new Thread(x =>
            {
                Thread.Sleep(120000);
                Program.Client.MessageReceived -= Ev;
            }).Start();
            async Task Ev(SocketMessage message)
            {
                Console.WriteLine("E");
                if(message.Author == arg.Author)
                {
                    if(message.Content.ToLower().Trim() == "cancel")
                    {
                        await arg.Channel.SendMessageAsync("Operation returned.");
                        return;
                    }
                    if (message.Content.Length <= 100)
                    {
                        await arg.Channel.SendMessageAsync("Message can't be 100 characters long or less.");
                    }
                    else
                    {
                        Program.Client.MessageReceived -= Ev;
                        var ToEdit = await arg.Channel.SendMessageAsync($"Sent x message");
                        long i = 1;
                        foreach (SocketGuild Guild in Program.Client.Guilds)
                        {
                            foreach (SocketGuildUser User in Guild.Users)
                            {
                                try
                                {
                                    if(!AlreadyUsersS.Contains(User.Id))
                                    {
                                        AlreadyUsersS.Add(User.Id);
                                        await User.SendMessageAsync(message.Content);
                                        await ToEdit.ModifyAsync(x =>
                                        {
                                            x.Content = $"Sent {i} message(s)";
                                        });
                                        await arg.Channel.SendMessageAsync($"Sent {User.Username} a message");
                                        i++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    await arg.Channel.SendMessageAsync($"Couldn't send {User.Username} a message; `{e.Message}`");
                                }
                                Thread.Sleep(3000);
                            }
                        }
                    }
                }
            }
        }
    }

    class mass_dm1
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                foreach (var Guild in Program.GetAllShardGuilds())
                {
                    try
                    {
                        var Emb = new EmbedBuilder()
                        {
                            Color = new Color(143, 255, 199),
                            Description = "Hello! This is extremely important, and I apologize if this is interrupting anything. Discord has taken moderative action on my account, and so I ask that you instead invite this version of me:\n\n[click here](https://discord.com/oauth2/authorize?client_id=666041395903070240&scope=bot&permissions=8). :pleading_face:",
                            Timestamp = DateTime.UtcNow,
                            Footer = new EmbedFooterBuilder()
                            {
                                Text = "Thank you!"
                            },
                        };
                        await Guild.TextChannels.First().SendMessageAsync("", false, Emb.Build());
                        await Guild.TextChannels.First().SendMessageAsync("Last message before I go offline <3");
                        await arg.Author.SendMessageAsync($"Sent to {Guild.Name}");
                    }
                    catch (Exception e)
                    {
                        //await arg.Author.SendMessageAsync(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

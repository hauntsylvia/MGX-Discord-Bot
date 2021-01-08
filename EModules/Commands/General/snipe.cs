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
using System.Net.Sockets;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class snipe
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 1
        };
        public static Dictionary<ulong, IMessage> Messages = new Dictionary<ulong, IMessage>(); // channel id - array of last 50
        public static Dictionary<ulong, string> Attachments = new Dictionary<ulong, string>(); // channel id - url to last attachment
        public static async Task Maindo(SocketMessage arg)
        {
            if (!Messages.ContainsKey(arg.Channel.Id))
            {
                await arg.Channel.SendMessageAsync("No deleted messages!");
            }
           
            IMessage Sniped = Messages[arg.Channel.Id];
            EmbedBuilder Msg = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder()
                {
                    IconUrl = Sniped.Author.GetAvatarUrl(),
                    Name = Sniped.Author.Username + $"#{Sniped.Author.Discriminator}",
                },
                Color = new Color(66, 245, 230),
                Timestamp = DateTime.UtcNow,
                Description = Sniped.Content,
                ImageUrl = Attachments.ContainsKey(arg.Channel.Id) ? Attachments[arg.Channel.Id] : "",
            };
            await arg.Channel.SendMessageAsync("", false, Msg.Build());
        }

        public static async Task OnReceived(SocketMessage arg)
        {
            try
            {
                if (arg.Attachments.Count > 0 && !arg.Author.IsBot)
                {
                    Program.WebClient.DownloadFile(arg.Attachments.First().Url, $@".\{arg.Channel.Id}.png");
                    var NewUrl = await ((SocketTextChannel)Program.Client.GetChannel(689551621297930401)).SendFileAsync($@".\{arg.Channel.Id}.png", "");
                    if (!Attachments.ContainsKey(arg.Channel.Id))
                    {
                        Attachments.Add(arg.Channel.Id, NewUrl.Attachments.First().Url);
                    }
                    else
                    {
                        Attachments[arg.Channel.Id] = NewUrl.Attachments.First().Url;
                    }
                    File.Delete($@".\{arg.Channel.Id}.png");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static async Task OnDelete(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            if(!Messages.ContainsKey(arg1.Value.Channel.Id))
            {
                Messages.Add(arg1.Value.Channel.Id, arg1.Value);
            }
            else
            {
                Messages[arg1.Value.Channel.Id] = arg1.Value;
            }
        }
    }
}

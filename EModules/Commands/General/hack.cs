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
    class hack
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "fun",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {

            var User = arg.MentionedUsers.Count > 0 ? arg.MentionedUsers.First() : arg.Author as SocketGuildUser;
            var Msg = await arg.Channel.SendMessageAsync($"Hacking {User.Mention}..");

            var UserPfp = User.GetAvatarUrl() == null ? "null" : User.GetAvatarUrl();

            Thread.Sleep(1500);

            List<string> RandomMsgs = new List<string>()
            {
                {"Hacking discord account..."},
                {"Injecting SUP3R DE4DLY H4XX..."},
                {"public static string IP address = 127.0.0.1"},
                {"getting dms..."},
                {@"last DM sent = ""i think i might be gay.."""},
                {"getting profile picture.."},
                {$"profile picture = {UserPfp}"},
                {$"id = {User.Id}"},
                {@"name = ""Joe mama"""},
                {$"cleaning up SUP3R DE4DLY H4XX..."},
            };

            string Slash = "/";
            var Random = new Random();
            for (int i = 0; i < RandomMsgs.Count; i++)
            {
                if(Slash == "/")
                {
                    Slash = @"\";
                }
                else if(Slash == @"\")
                {
                    Slash = "x";
                }
                else if(Slash == "x")
                {
                    Slash = "/";
                }
                await Msg.ModifyAsync(x =>
                {
                    x.Content = $"{Slash} | {RandomMsgs[i]}";
                });
                Thread.Sleep(Random.Next(1750, 2500));
            }

            await arg.Channel.SendMessageAsync("haxx complete");

            Thread.Sleep(5000);

            await Msg.DeleteAsync();
        }
    }
}

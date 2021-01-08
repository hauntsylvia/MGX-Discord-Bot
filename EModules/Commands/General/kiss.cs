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
    class kiss
    {
        private static List<string> Urls = new List<string>()
        {
            {"https://cdn.nekos.life/kiss/kiss_057.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss14.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss0.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss1.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss2.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss3.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss4.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss5.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss6.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss7.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss8.gif"},
            {"https://weebs4life.ga/images/kiss/image/kiss9.gif"},
        };
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "lovey",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            var Rand = new Random();

            SocketGuildUser User = arg.MentionedUsers.Count() >= 1 ? arg.MentionedUsers.First() as SocketGuildUser : arg.Author as SocketGuildUser;

            if(User.Id == arg.Author.Id)
            {
                await arg.Channel.SendMessageAsync($"Somehow, {User.Mention} kisses {arg.Author.Mention} - idk how you manage to kiss yourself but you do you");
            }
            else
            {
                var Embed = new EmbedBuilder()
                {
                    Description = $"***{arg.Author.Mention} kissed {User.Mention}***",
                    Color = Color.DarkRed,
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "MGX",
                    },
                    ImageUrl = Urls[Rand.Next(0, Urls.Count)],
                    Timestamp = DateTime.UtcNow,
                };

                await arg.Channel.SendMessageAsync("", false, Embed.Build());
            }
        }
    }
    class kis
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = true,
            Category = "lovey",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await kiss.Maindo(arg);
        }
    }
}

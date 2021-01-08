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
    class inv
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            var User = arg.MentionedUsers.Count > 0 ? arg.MentionedUsers.First() : Program.Client.CurrentUser;
            if (!User.IsBot)
                User = Program.Client.CurrentUser;
            string Link = $"[Invite {User.Username} by clicking here!](https://discordapp.com/oauth2/authorize?client_id={User.Id}&scope=bot&permissions=8)";
            var Builder = new EmbedBuilder()
            {
                Color = new Color(0x2494F4),
                Description = Link,
                Title = $"{User.Username}",
                Footer = new EmbedFooterBuilder()
                {
                },
                Timestamp = DateTime.UtcNow
            };
            Builder.WithThumbnailUrl(Program.Client.CurrentUser.GetAvatarUrl(ImageFormat.Auto, 2048));
            await arg.Channel.SendMessageAsync("", false, Builder.Build());
        }
    }

    class invite
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = true,
            Category = "utils",
            Premium = false,
            RequiredPermissions = inv.Info.RequiredPermissions
        };

        public static async Task Maindo(SocketMessage arg)
        {
            await inv.Maindo(arg);
        }
    }
}

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
    class info
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
            var Guild = ((SocketGuildChannel)arg.Channel).Guild;
            SocketGuildUser UserToCheck = arg.MentionedUsers.Count >= 1 ? arg.MentionedUsers.First() as SocketGuildUser : arg.Author as SocketGuildUser;

            string TitleOfBuilder = $"{UserToCheck.Username}'s Information";
            var Fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder()
                {
                    Name = "Full Username",
                    Value = $"{UserToCheck.Username} #{UserToCheck.Discriminator}"
                },
                new EmbedFieldBuilder()
                {
                    Name = "Developer ID",
                    Value = UserToCheck.Id.ToString()
                },
                new EmbedFieldBuilder()
                {
                    Name = "Profile Picture",
                    Value = $"[Link]({UserToCheck.GetAvatarUrl(ImageFormat.Auto, 2048)})"
                },
                new EmbedFieldBuilder()
                {
                    Name = "Account Age (Days)",
                    Value = (DateTime.UtcNow - UserToCheck.CreatedAt.UtcDateTime).Days.ToString()
                },
                new EmbedFieldBuilder()
                {
                    Name = "Other",
                    Value = InfoCommandRemarkHolder.ReturnRemarkOfUser(UserToCheck.Id)
                },
                new EmbedFieldBuilder()
                {
                    Name = "Commands Run",
                    Value = StoreHandler.LoadData(UserToCheck.Id, "CMD-COUNT") != null ? StoreHandler.LoadData(UserToCheck.Id, "CMD-COUNT") : "0"
                },
            };

            var Builder = new EmbedBuilder()
            {
                Fields = Fields,
                Color = new Color(110, 110, 255),
                Footer = new EmbedFooterBuilder()
                {
                    Text = ":D"
                },
                Timestamp = DateTime.UtcNow,
                Title = TitleOfBuilder,
                ThumbnailUrl = UserToCheck.GetAvatarUrl() != null ? UserToCheck.GetAvatarUrl() : Program.Client.CurrentUser.GetAvatarUrl()
            };

            await arg.Channel.SendMessageAsync("", false, Builder.Build());
        }
    }
}

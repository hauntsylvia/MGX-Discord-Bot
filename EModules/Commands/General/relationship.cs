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
    class relationship
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "lovey",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            var Builder = new EmbedBuilder()
            {
                Color = Color.Magenta,
                Description = "Your Relationship"
            };

            if (StoreHandler.LoadData(arg.Author.Id, $"DATINGDATA") != null)
            {
                var Chchh = arg.Channel as SocketGuildChannel;
                var Guild = Chchh.Guild;

                var User = ulong.Parse(StoreHandler.LoadData(arg.Author.Id, $"DATINGDATA"));

                Builder.AddField($" - - - ", $"<@{User}> + {arg.Author.Mention}", true);

                if (Guild.GetUser(User) != null)
                {
                    Builder.WithThumbnailUrl(Guild.GetUser(User).GetAvatarUrl(ImageFormat.Auto, 2048));
                }
                else
                {
                    Builder.WithThumbnailUrl(arg.Author.GetAvatarUrl(ImageFormat.Auto, 2048));
                }

                await arg.Channel.SendMessageAsync("", false, Builder.Build());
            }
            else
            {
                await arg.Channel.SendMessageAsync($"Sorry {arg.Author.Username}, you're single );");
            }
        }
    }
}

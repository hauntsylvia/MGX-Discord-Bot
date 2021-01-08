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
    class admin
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 20
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if (arg.MentionedUsers.Count() > 0)
            {
                var User = arg.MentionedUsers.First();
                var Guild = ((SocketGuildChannel)arg.Channel).Guild;
                if (User.Id == Program.Client.CurrentUser.Id)
                {
                    await arg.Channel.SendMessageAsync("already got higher roles so gl with that noob");
                }
                else if (GuildStuff.ReadRank(Guild.Id, User.Id) >= 19)
                {
                    await arg.Channel.SendMessageAsync($"{User.Username} Is already an administrator or higher");
                }
                else
                {
                    GuildStuff.SetRank(Guild.Id, User.Id, 19);
                    await arg.Channel.SendMessageAsync($"Successfully added {User.Username} as an administrator");
                }
            }
        }
    }
    class unadmin
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 20
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if (arg.MentionedUsers.Count() > 0)
            {
                var User = arg.MentionedUsers.First();
                var Guild = ((SocketGuildChannel)arg.Channel).Guild;
                if (User.Id == Program.Client.CurrentUser.Id)
                {
                    await arg.Channel.SendMessageAsync("But I'm MGX ;c");
                }
                else if (GuildStuff.ReadRank(Guild.Id, User.Id) != 19)
                {
                    await arg.Channel.SendMessageAsync($"{User.Username} Isn't an administrator");
                }
                else
                {
                    GuildStuff.SetRank(Guild.Id, User.Id, 1);
                    await arg.Channel.SendMessageAsync($"Successfully removed {User.Username} from administrator");
                }
            }
            else
            {
                await arg.Channel.SendMessageAsync($"Please mention user to unadmin. Eg; '{Program.Prefix}unadmin <@{Program.Client.CurrentUser.Id}>'");
            }
        }
    }

    class mod
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 19
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if (arg.MentionedUsers.Count() > 0)
            {
                var User = arg.MentionedUsers.First();
                var Guild = ((SocketGuildChannel)arg.Channel).Guild;
                if (User.Id == Program.Client.CurrentUser.Id)
                {
                    await arg.Channel.SendMessageAsync("But I'm MGX );");
                    return;
                }
                if (GuildStuff.ReadRank(Guild.Id, User.Id) >= 18)
                {
                    await arg.Channel.SendMessageAsync($"{User.Username} Is already a moderator or higher");
                }
                else
                {
                    GuildStuff.SetRank(Guild.Id, User.Id, 18);
                    await arg.Channel.SendMessageAsync($"Successfully added {User.Username} as a moderator");
                }
            }
        }
    }

    class unmod
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 19
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if (arg.MentionedUsers.Count() > 0)
            {
                var User = arg.MentionedUsers.First();
                var Guild = ((SocketGuildChannel)arg.Channel).Guild;
                if (User.Id == Program.Client.CurrentUser.Id)
                {
                    await arg.Channel.SendMessageAsync("But I like my perms ;c");
                    return;
                }

                else if (GuildStuff.ReadRank(Guild.Id, User.Id) != 18)
                {
                    await arg.Channel.SendMessageAsync($"{User.Username} Isn't a moderator");
                }
                else
                {
                    GuildStuff.SetRank(Guild.Id, User.Id, 1);
                    await arg.Channel.SendMessageAsync($"Successfully removed {User.Username} from moderator");
                }
            }
            else
            {
                await arg.Channel.SendMessageAsync($"Please mention user to unmod. Eg; '{Program.Prefix}unmod <@{Program.Client.CurrentUser.Id}>'");
            }
        }
    }

    class rank
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

            string TitleOfBuilder = $"{UserToCheck.Username}'s Rank";

            var Builder = new EmbedBuilder()
            {
                Title = TitleOfBuilder,
                Color = Color.LightOrange,
                Footer = new EmbedFooterBuilder()
                {
                    Text = "ure kinda nice <3"
                },
                ThumbnailUrl = UserToCheck.GetAvatarUrl(),
                Timestamp = DateTime.UtcNow,
            };

            var Field = new EmbedFieldBuilder()
            {
                Name = "Name",
                Value = GuildStuff.GetNameOfRank(GuildStuff.ReadRank(Guild.Id, UserToCheck.Id))
            };

            Builder.AddField(Field);

            await arg.Channel.SendMessageAsync("", false, Builder.Build());
        }
    }
}

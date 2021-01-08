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

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class inventory
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                var Embed = new EmbedBuilder()
                {
                    Author = new EmbedAuthorBuilder()
                    {
                        IconUrl = arg.Author.GetAvatarUrl(),
                        Name = $"{arg.Author.Username}'s Inventory",
                    },
                    Color = new Color(255, 137, 139),
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "MGX"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                };

                foreach (string Fi in Directory.GetFiles(Economy._eco_dir + arg.Author.Id))
                {
                    var Field = new EmbedFieldBuilder();
                    Field.Name = $"Product";
                    Field.IsInline = true;
                    string Count = File.ReadAllText(Fi);
                    Field.Value = $"`{Path.GetFileNameWithoutExtension(Fi)} x{Count}`";
                    Embed.Fields.Add(Field);
                }

                await arg.Channel.SendMessageAsync("", false, Embed.Build());
            }
            catch
            {
                await arg.Channel.SendMessageAsync("You don't have any items!");
            }
        }
    }
}

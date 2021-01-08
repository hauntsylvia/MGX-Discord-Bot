using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.API;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;

using UrbanDictionaryNet;

using MGX_Discord_Bot.EModules.Scheduler;
using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class define
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
            string _Term = "";
            try
            {
                if (arg.Content.Contains(' '))
                {
                    _Term = arg.Content.Split(new[] { ' ' }, 2)[1];
                    var Defs = UrbanDictionary.Define(_Term).Definitions;
                    if (Defs.Count > 0)
                    {
                        var Emb = new EmbedBuilder()
                        {
                            Author = new EmbedAuthorBuilder()
                            {
                                IconUrl = "https://lh3.googleusercontent.com/unQjigibyJQvru9rcCOX7UCqyByuf5-h_tLpA-9fYH93uqrRAnZ0J2IummiejMMhi5Ch",
                                Name = $@"Author - ""{Defs.First().Author}""",
                                Url = "https://www.urbandictionary.com/"
                            },
                            Color = new Color(255, 118, 5),
                            Description = Defs.First().Definition,
                            Footer = new EmbedFooterBuilder()
                            {
                                Text = $"👍{Defs.First().ThumbsUp} 👎{Defs.First().ThumbsDown}"
                            },
                            Timestamp = DateTime.UtcNow,
                        };

                        await arg.Channel.SendMessageAsync("", false, Emb.Build());
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync($"No definition found for `{_Term}`");
                    }
                }
                else
                {
                    await arg.Channel.SendMessageAsync("Please give a phrase or word to define!");
                }
            }
            catch
            {
                await arg.Channel.SendMessageAsync($"No definition found for `{_Term}`");
            }
        }
    }
}

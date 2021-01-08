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
using System.Web.UI;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class shop
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
                List<EmbedFieldBuilder> Pages = new List<EmbedFieldBuilder>();
                EmbedFieldBuilder CurPage = new EmbedFieldBuilder();
                int Cur = 0;
                foreach (var Product in Economy.Definitions.Keys)
                {
                    long Cost = long.Parse(Economy.Definitions[Product][1].ToString());
                    string Desc = (string)Economy.Definitions[Product][0];
                    Emoji Associated = (Emoji)Economy.Definitions[Product][2];
                    CurPage.Name = $"Page 1";
                    CurPage.Value += $"\n {Associated} **{Product}** -- __${Cost}__ \n`{Desc}`\n";
                    if ((Cur % 5 == 0 && Cur > 4) || Cur + 1 == Economy.Definitions.Count)
                    {
                        Pages.Add(CurPage);
                        CurPage = new EmbedFieldBuilder();
                    }
                    Cur++;
                }
                var Emb = new EmbedBuilder()
                {
                    Color = new Color(230, 156, 255),
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        {
                            Pages.First()
                        }
                    },
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = $"ure an amazing person <3 • Page 1/{Pages.Count}"
                    },
                    Timestamp = DateTime.UtcNow,
                    Title = $"{Program.Client.CurrentUser.Username}'s Shop",
                };
                var Msg = await arg.Channel.SendMessageAsync("", false, Emb.Build());
                await Msg.AddReactionsAsync(new[] { new Emoji(LeftArr), new Emoji(RightArr) });
                new PageManager(Msg.Id, Pages.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public static string LeftArr = "◀";
        public static string RightArr = "▶";
    }
    class PageManager
    {
        ulong MessageID;
        EmbedFieldBuilder[] Pages;
        private int CurrentPage = 0;
        public PageManager(ulong _MessageID, EmbedFieldBuilder[] _Pages)
        {
            MessageID = _MessageID;
            Pages = _Pages;
            Program.Client.ReactionAdded += Client_ReactionAdded;
        }

        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if(arg3.Message.Value.Id == MessageID && !arg3.User.Value.IsBot)
            {
                EmbedBuilder Embed = arg3.Message.Value.Embeds.First().ToEmbedBuilder();
                if (arg3.Emote.Name == shop.LeftArr)
                {
                    if(CurrentPage - 1 >= 0)
                    {
                        Embed.Fields.First().Value = Pages[CurrentPage - 1].Value;
                        Embed.Fields.First().Name = $"Page {CurrentPage + 1}/{Pages.Count()}";
                        CurrentPage--;
                        await arg3.Message.Value.ModifyAsync(x =>
                        {
                            x.Embed = Embed.Build();
                        });
                    }
                }
                else if(arg3.Emote.Name == shop.RightArr)
                {
                    if(CurrentPage + 1 < Pages.Count())
                    {
                        Embed.Fields.First().Value = Pages[CurrentPage + 1].Value;
                        Embed.Fields.First().Name = $"Page {CurrentPage + 1}/{Pages.Count()}";
                        CurrentPage++;
                        await arg3.Message.Value.ModifyAsync(x =>
                        {
                            x.Embed = Embed.Build();
                        });
                    }
                }
                await arg3.Message.Value.RemoveReactionsAsync(arg3.User.Value, new[] { arg3.Emote });
            }
        }
    }

    class buy
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
                string ToBuy = arg.Content.ToLower().Contains(' ') ? arg.Content.ToLower().Split(' ')[1] : null;
                long Count = 1;

                if (arg.Content.ToLower().Split(' ').Length >= 3)
                {
                    if (!long.TryParse(arg.Content.ToLower().Split(' ')[2], out Count))
                    {
                        Count = 1;
                    }
                }

                if (ToBuy != null)
                {
                    if (Enum.TryParse(ToBuy, out Product ProductBuying))
                    {
                        Receipt Buy = Economy.Buy(ProductBuying, Count, arg.Author.Id);
                        if(Buy.Success)
                        {
                            await arg.Channel.SendMessageAsync($"Successfully bought `{ProductBuying}` (x{Buy.Amount}) for `${Buy.Cost}`.");
                        }   
                        else
                        {
                            await arg.Channel.SendMessageAsync($"Not enough money! You have `${ShopClass.ReadCurrency(arg.Author.Id)}` and need `${Buy.Cost}`");
                        }
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync($@"The item ""`{ToBuy}`"" doesn't exist. :pensive");
                    }
                }
                else
                {
                    await arg.Channel.SendMessageAsync($"Please specify what you'd like to buy!");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

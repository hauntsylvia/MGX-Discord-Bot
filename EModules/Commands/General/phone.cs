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

#pragma warning disable IDE0051 // Remove unused private members
namespace MGX_Discord_Bot.EModules.Commands.General
{
    class phone
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "items",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if (Economy.CountProductForUser(Product.phone, arg.Author.Id) > 0)
            {
                var ThisType = Type.GetType("MGX_Discord_Bot.EModules.Commands.General.phone");
                if (arg.Content.Contains(" "))
                {
                    var AskedMethod = ThisType.GetMethod(arg.Content.ToLower().Split(' ')[1].Split(' ')[0], BindingFlags.Static | BindingFlags.NonPublic);
                    if(AskedMethod != null)
                    {
                        AskedMethod.Invoke(null, new[] { arg });
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync("That command doesn't exist!");
                    }
                }
                else
                {
                    var Embed = new EmbedBuilder()
                    {
                        Color = new Color(125, 255, 240),
                        Timestamp = DateTime.UtcNow,
                        Footer = new EmbedFooterBuilder()
                        { 
                            Text = "Phone commands!"
                        },
                    };
                    string All = "";
                    foreach(var Method in ThisType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
                    {
                        All += $"`{Program.Prefix}phone {Method.Name.ToLower()}`\n";
                    }
                    Embed.AddField(new EmbedFieldBuilder()
                    {
                        Name = "Phone Usage",
                        Value = All
                    });
                    await arg.Channel.SendMessageAsync("", false, Embed.Build());
                }
            }
            else
            {
                await arg.Channel.SendMessageAsync($"You need to buy a phone first! `{Program.Prefix}buy phone`");
            }
        }
        private static async Task postmeme(SocketMessage arg)
        {
            string Prefix = "phone.";
            if (StoreHandler.LoadData(arg.Author.Id, $"{Prefix}post-meme") == null || (DateTime.TryParse(StoreHandler.LoadData(arg.Author.Id, $"{Prefix}post-meme"), out DateTime LastPosted) && LastPosted < DateTime.UtcNow))
            {
                StoreHandler.SaveData(arg.Author.Id, $"{Prefix}post-meme", DateTime.UtcNow.AddHours(12));
                int Gain = new Random().Next(-1500, 2000);
                if (Gain == 0)
                    Gain += 2;
                var Embed = new EmbedBuilder()
                {
                    Color = new Color(0x34ebd5),
                    Timestamp = DateTime.UtcNow,
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = $"🔼{Gain / 2}"
                    },
                    Author = new EmbedAuthorBuilder()
                    {
                        Name = arg.Author.Username,
                        IconUrl = arg.Author.GetAvatarUrl(ImageFormat.Auto, 2048)
                    },
                    Description = $"Ad revenue: ${Gain}"
                };
                _ = Gain > 0 ? await arg.Channel.SendMessageAsync($"Looks like people enjoyed your meme!") : await arg.Channel.SendMessageAsync($"make better memes pls");
                await arg.Channel.TriggerTypingAsync();
                Thread.Sleep(3500);
                await arg.Channel.SendMessageAsync("Here are the analytics:", false, Embed.Build());
                ShopClass.AddCurrency(arg.Author.Id, Gain);
            }
            else if ((LastPosted - DateTime.UtcNow).TotalHours < 1)
                await arg.Channel.SendMessageAsync($"Almost there! `{(LastPosted - DateTime.UtcNow).Minutes}m` left");
            else
                await arg.Channel.SendMessageAsync($"You've recently posted a meme! Sorry, wait `{(LastPosted - DateTime.UtcNow).Hours}hrs`");
        }
    }
}
#pragma warning restore IDE0051 // Remove unused private members

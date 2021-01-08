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

using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Scheduler
{
    class ShopClass
    {
        private static string SetDirectory = @".\Currency\";
        private static long DefaultVal = 50;

        private static void RefreshDirectory()
        {
            if(!Directory.Exists(SetDirectory))
            {
                Directory.CreateDirectory(SetDirectory);
            }
        }
        public static long ReadCurrency(ulong UserId)
        {
            RefreshDirectory();
            string FC = $"{SetDirectory}{UserId}.txt";
            if(File.Exists(FC))
            {
                if (long.TryParse(File.ReadAllText(FC), out long Res))
                {
                    if(Res < 0)
                    {
                        SetCurrency(UserId, 0);
                    }
                }
            }
            return File.Exists(FC) ? long.Parse(File.ReadAllText(FC)) : DefaultVal;
        }

        private static void SetCurrency(ulong UserId, long ToSet)
        {
            string FC = $"{SetDirectory}{UserId}.txt";
            File.WriteAllText(FC, ToSet.ToString());
        }

        public static void AddCurrency(ulong UserId, long ToAdd)
        {
            RefreshDirectory();
            string FC = $"{SetDirectory}{UserId}.txt";
            File.WriteAllText(FC, (ReadCurrency(UserId) + ToAdd).ToString());
        }

        public static async Task<bool> SpendCurrency(ulong UserId, long ToSub, ISocketMessageChannel ChannelForPrompt)
        {
            RefreshDirectory();
            if(ReadCurrency(UserId) >= ToSub && ToSub >= 0)
            {
                string FC = $"{SetDirectory}{UserId}.txt";
                var Prompt = new Prompt()
                {
                    ChannelForPrompt = ChannelForPrompt,
                    Options = new List<Emoji>()
                    {
                        { new Emoji("\uD83D\uDC4D") },
                        { new Emoji("\uD83D\uDC4E") }
                    }.ToArray(),
                    MaxTime = 10,
                    Title = $"You (<@{UserId}>) are about to spend ${ToSub} - Continue?",
                    Target = Program.Client.GetUser(UserId)
                };

                IEmote Purchased = await Prompt.ShowPrompt();
                if(Purchased != null)
                {
                    if (Purchased.Name == "👍")
                    {
                        File.WriteAllText(FC, (ReadCurrency(UserId) - ToSub).ToString());
                        return true;
                    }
                }
                await ChannelForPrompt.SendMessageAsync($"No purchase made.");
                return false;
            }
            else
            {
                await ChannelForPrompt.SendMessageAsync($"You need ${ToSub - ReadCurrency(UserId)} more. Check your balance with `{Program.Prefix}bal`");
                return false;
            }
                
        }
    }
}

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
using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class pills
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "items",
            Premium = false,
            RequiredPermissions = 1
        };
        private static List<string> AllNegate = new List<string>()
        {
            {"You're dumb so it shrunk."}, 
            {"The girl that sold you the pills lied, now your pp shrunk."}, 
            {"You're a fucking dick, so now you'll have less of one."}, 
            {"A bigger penis won't make you feel better, you still look ugly."}, 
            {"Oop sister - Wrong pills."},
            {"Now you're wondering why you have no anti depressants and a TON of enlargement pills (you still have to buy them though haha nOoB)."}, 
            {"I apologize for this, but I must shrink it."}, 
            {"Oh, you meant dick pills and not ego pills."}, 
            {"Dude, since you can't accept your dick, I'm shrinking it. Fuck you."},
            {"ok."},
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                long Count = 1;
                long CountOfProductsForUser = Economy.CountProductForUser(Product.pills, arg.Author.Id); 

                if (arg.Content.ToLower().Split(' ').Length >= 2)
                {
                    if (!long.TryParse(arg.Content.ToLower().Split(' ')[1], out Count))
                    {
                        Count = 1;
                    }
                }

                if (CountOfProductsForUser >= Count)
                {
                    var Prompt = new Prompt()
                    {
                        ChannelForPrompt = arg.Channel,
                        MaxTime = 10,
                        Options = new Emoji[] { new Emoji("\uD83D\uDC4D"), new Emoji("\uD83D\uDC4E") },
                        Target = arg.Author,
                        Title = $"Are you sure you want to take pills? (x{Count})"
                    };

                    var Response = await Prompt.ShowPrompt();

                    if (Response != null && Response.Name == "👍")
                    {
                        Economy.ChangeProductCountForUser(Product.pills, -Count, arg.Author.Id);
                        await TakePills(arg, Count);
                    }
                    else
                    {
                        await arg.Channel.SendMessageAsync("kk.");
                    }
                }
                else
                {
                    await arg.Channel.SendMessageAsync($"You need to buy {Count} pill(s) first.");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static async Task TakePills(SocketMessage arg, long Multiplier)
        {
            var User = arg.Author as SocketGuildUser;
            long CurrentSize = ppsize.GetSize(User.Id);

            var Random = new Random();

            long IncreaseRate = Random.Next(5, 15) * Multiplier;
            long DecreaseRate = Random.Next(5, 30) * Multiplier;

            bool Increases = Random.Next(1, 3) == 1;

            if (Increases)
            {
                ppsize.SaveNewSize(User.Id, CurrentSize + IncreaseRate);
                await arg.Channel.SendMessageAsync($"Congrats! They worked.\n`PP Change: +{IncreaseRate}`\n`Pills Left: {Economy.CountProductForUser(Product.pills, arg.Author.Id)}`");
            }
            else
            {
                ppsize.SaveNewSize(User.Id, CurrentSize - DecreaseRate);
                await arg.Channel.SendMessageAsync($"{AllNegate[Random.Next(1, AllNegate.Count)]}\n`PP Change: -{DecreaseRate}`\n`Pills Left: {Economy.CountProductForUser(Product.pills, arg.Author.Id)}`");
            }
        }
    }
}

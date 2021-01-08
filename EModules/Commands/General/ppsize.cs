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
    class ppsize
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
            var User = arg.MentionedUsers.Count >= 1 ? arg.MentionedUsers.First() : arg.Author;
            long PPSize = GetSize(User.Id);
            var MainField = new EmbedFieldBuilder()
            {
                Name = $"PP Size [{PPSize}]"
            };

            MainField.Value = "8";
            try
            {
                long NumberToGoUpTo = PPSize > 750 ? 750 : PPSize;
                long LeftOver = PPSize > 750 ? PPSize - NumberToGoUpTo : 0;
                for (int i = 1; i <= NumberToGoUpTo; i++)
                {
                    MainField.Value = MainField.Value + "=";
                }
                MainField.Value = MainField.Value + "D";
                if (LeftOver > 0)
                {
                    MainField.Value = MainField.Value + $"  ..+{LeftOver} more";
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            if (StoreHandler.LoadData(User.Id, "PPSIZE") == null)
            {
                await arg.Channel.SendMessageAsync($"Just a tip (terrible pun intended) - You can use `{Program.Prefix}pills` to save your pp size (and increase or decrease it!)");
            }

            var Embed = new EmbedBuilder()
            {
                Color = Color.Green,
                Footer = new EmbedFooterBuilder()
                {
                    Text = "MGX"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                {
                    {
                        MainField
                    },
                },
                ThumbnailUrl = User.GetAvatarUrl() == null ? Program.Client.CurrentUser.GetAvatarUrl() : User.GetAvatarUrl(),
            };

            await arg.Channel.SendMessageAsync("", false, Embed.Build());
        }

        public static long GetSize(ulong UserId)
        {
            var Random = new Random();
            long ToReturn = Random.Next(0, 10);

            if(StoreHandler.LoadData(UserId, $"PPSIZE") != null)
            {
                if(long.TryParse(StoreHandler.LoadData(UserId, $"PPSIZE"), out long ReturnedLong))
                {
                    if(ReturnedLong <= 0)
                    {
                        StoreHandler.RemoveData(UserId, $"PPSIZE");
                        return ToReturn;
                    }
                    return ReturnedLong;
                }
                else
                {
                    StoreHandler.RemoveData(UserId, $"PPSIZE");
                    return ToReturn;
                }
            }

            return ToReturn;
        }

        public static void SaveSize(ulong UserId)
        {
            StoreHandler.SaveData(UserId, "PPSIZE", GetSize(UserId));
        }

        public static void SaveNewSize(ulong UserId, long NewSize)
        {
            StoreHandler.SaveData(UserId, "PPSIZE", NewSize);
        }
    }
}

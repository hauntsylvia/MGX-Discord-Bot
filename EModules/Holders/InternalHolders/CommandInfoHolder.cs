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
using System.Collections.Concurrent;
using System.Diagnostics;
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

namespace MGX_Discord_Bot.EModules.Holders.InternalHolders
{
    class CommandInfoHolder
    {

        private static Dictionary<string, string> EmojiAssociate = new Dictionary<string, string>()
        {
            {"utils", "🛠"},
            {"fun", "😄"},
            {"currency", "💰"},
            {"music", "🎵"},
            {"lovey", "❤"},
            {"settings", "⚙"},
            {"items", "🍾"},
            {"nsfw", "😏"},
        };

        public static string ReturnEmojiAssociatedWithCategory(string CategoryOfCommand)
        {
            if(EmojiAssociate.ContainsKey(CategoryOfCommand.ToLower()))
            {
                return EmojiAssociate[CategoryOfCommand.ToLower()];
            }
            return "❓";
        }

        public static Dictionary<string, string> RetAllCMDInfoNEW()
        {
            Dictionary<string, string> InfRet = new Dictionary<string, string>();
            foreach (string Cmd in NamespaceNav.GetAllTypes("MGX_Discord_Bot.EModules.Commands.General"))
            {
                var Command = Type.GetType($"MGX_Discord_Bot.EModules.Commands.General.{Cmd}");
                var CheckIfExBEF = Command.GetField("Info");
                if(CheckIfExBEF != null)
                {
                    var CheckIfEx = (CommandInformation)CheckIfExBEF.GetValue(null);
                    string Cat = CheckIfEx == null ? "other" : CheckIfEx.Category;
                    if (!CheckIfEx.Alias)
                    {
                        InfRet.Add(Cmd, Cat);
                    }
                }
            }
            return InfRet;
        }

        public static string ReturnCategoryType(string Command)
        {
            var CheckIfEx = (string)Type.GetType("MGX_Discord_Bot.EModules.Commands.General").GetField("Category").GetValue(null);
            string Cat = CheckIfEx == null ? "other" : CheckIfEx;
            return Cat;
        }
    }
}
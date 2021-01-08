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
using MGX_Discord_Bot.EModules.Commands.CMD;
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class help
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = true,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await cmds.Maindo(arg);
        }
    }

    class helfe
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = true,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            await cmds.Maindo(arg);
        }
    }
    class cmds
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
            try
            {
                if (arg.Content.ToLower().Contains(" "))
                {
                    string OptArg = arg.Content.ToLower().Split(' ')[1].Trim();
                    grab.INIT(arg, OptArg);
                }
                else
                {
                    var Builder = new EmbedBuilder()
                    {
                        Color = Color.DarkMagenta,
                        Description = "**MGX Commands**",
                        Footer = new EmbedFooterBuilder()
                        {
                            Text = $"Use {Program.Prefix}setup for assistance!"
                        },
                        Timestamp = DateTime.UtcNow
                    };

                    List<string> Used = new List<string>();
                    var FList = CommandInfoHolder.RetAllCMDInfoNEW();
                    foreach (string TypeName in FList.Keys.ToList())
                    {
                        if (!Used.Contains(FList[TypeName]))
                        {
                            Used.Add(FList[TypeName]);
                            string CategoryFirstUpper = FList[TypeName].ToCharArray().First().ToString().ToUpper() + FList[TypeName].Substring(1);
                            Builder.AddField($"{CommandInfoHolder.ReturnEmojiAssociatedWithCategory(FList[TypeName])} {CategoryFirstUpper}", $"`{Program.Prefix}cmds {FList[TypeName].ToLower()}`", true);
                        }
                    }

                    await arg.Channel.SendMessageAsync("", false, Builder.Build());
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
namespace MGX_Discord_Bot.EModules.Commands.CMD
{
    class PrintOut
    {
        private static string NamespaceForCommands = "MGX_Discord_Bot.EModules.Commands.General";
        public static bool INIT(SocketMessage arg, string[] ToPrint, string Title)
        {
            if (ToPrint.Count() >= 1)
            {
                string FinTitle = Title.ToCharArray().First().ToString().ToUpper() + Title.Substring(1);

                var Builder = new EmbedBuilder()
                {
                    Color = Color.DarkMagenta,
                    Description = $"**{CommandInfoHolder.ReturnEmojiAssociatedWithCategory(FinTitle)} {FinTitle} Commands**",
                    ThumbnailUrl = Program.Client.CurrentUser.GetAvatarUrl(),
                };

                string CompiledCmds = string.Empty;
                string CompiledPremiumCmds = string.Empty;
                foreach (string Cmd in ToPrint)
                {
                    CommandInformation Info = (CommandInformation)Type.GetType($"{NamespaceForCommands}.{Cmd}").GetField("Info").GetValue(null);
                    if (!Info.Alias)
                    {
                        if (Info.Premium)
                        {
                            CompiledPremiumCmds = $"{CompiledPremiumCmds}\n`{Program.Prefix}{Cmd}`";
                        }
                        else
                        {
                            CompiledCmds = $"{CompiledCmds}\n`{Program.Prefix}{Cmd}`";
                        }
                    }
                }
                Builder.AddField($"*Regular*", CompiledCmds, true);
                if(CompiledPremiumCmds.Length > 0)
                {
                    Builder.AddField($"*Premium*", CompiledPremiumCmds, true);
                }
                arg.Channel.SendMessageAsync("", false, Builder.Build());
            }
            return true;
        }
    }

    class grab
    {
        public static void INIT(SocketMessage arg, string CommandWant)
        {
            var AllCMDs = CommandInfoHolder.RetAllCMDInfoNEW();
            foreach (string CMD in AllCMDs.Keys.ToList())
            {
                if(AllCMDs[CMD].ToLower() != CommandWant.ToLower())
                {
                    AllCMDs.Remove(CMD);
                }
            }
            PrintOut.INIT(arg, AllCMDs.Keys.ToArray(), CommandWant);
        }
    }
}

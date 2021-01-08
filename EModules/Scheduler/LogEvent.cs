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

using MGX_Discord_Bot.EModules.Commands.General;
using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;

namespace MGX_Discord_Bot.EModules.Scheduler
{
    class LogEvent
    {
        public static void SendLog(EmbedBuilder Embed)
        {
            try
            {
                SocketTextChannel LogChannel = Program.Client.GetGuild(687266658771140608).GetTextChannel(688874307962798170);
                LogChannel.SendMessageAsync("", false, Embed.Build());
            }
            catch
            {

            }
        }
    }
}

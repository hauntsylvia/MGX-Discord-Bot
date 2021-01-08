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
using MGX_Discord_Bot.EModules.Scheduler;
using MGX_Discord_Bot.EModules.Holders.CommandHolders;
using MGX_Discord_Bot.EModules.Holders.InternalHolders;

namespace MGX_Discord_Bot.EModules.Scheduler
{
    class MGXStatistics
    {
        public static DateTime StartedAt;
        public static TimeSpan GetUptime()
        {
            return DateTime.UtcNow - StartedAt;
        }
        private static double GetMemoryUsage()
        {
            var ThisProcess = Process.GetCurrentProcess();
            return ThisProcess.WorkingSet64 / 1e+6;
        }
        public static double MemoryUsageInMB
        {
            get { return GetMemoryUsage(); }
        }
        public static int GetGuildCount()
        {
            return Program.Client.Guilds.Count;
        }
        public static int GetUserCount()
        {
            int AllUsers = 0;
            foreach (SocketGuild GuildRec in Program.Client.Guilds)
            {
                AllUsers += GuildRec.Users.Count();
            }
            return AllUsers;
        }
    }
}

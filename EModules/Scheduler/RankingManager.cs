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
    class RankingManager
    {
        public static readonly Dictionary<ulong, int> BotOwners = new Dictionary<ulong, int>()
        {
            {421126160726884353, 20}, // old account RIP - back alive noobs - nvm RIP again
            {528750326107602965, 20}, // new account
            {579483193665781770, 10}, // BaniShed/andy
        };
    }
}

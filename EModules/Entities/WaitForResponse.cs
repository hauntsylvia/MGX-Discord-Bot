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
using System.Net.Sockets;

namespace MGX_Discord_Bot.EModules.Entities
{
    class WaitForResponse
    {
        public ulong UserId;
        public ulong ChannelId;
        public long TimeLimitS;
        public async Task<SocketMessage> Start()
        {
            SocketMessage ToRet = null;
            async Task Ev(SocketMessage Arg)
            {
                if (Arg.Author.Id == UserId && Arg.Channel.Id == ChannelId)
                {
                    ToRet = Arg;
                    Program.Client.MessageReceived -= Ev;
                }
            }
            Program.Client.MessageReceived += Ev;
            while (ToRet == null && TimeLimitS >= 0)
            {
                TimeLimitS--;
                Thread.Sleep(1000);
            }
            return ToRet;
        }
    }
}

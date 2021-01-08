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

using Discord;
using Discord.API;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using System.Reflection;
using Discord.Audio;

using MGX_Discord_Bot.EModules.Entities;
using MGX_Discord_Bot.EModules.Commands.General;

namespace MGX_Discord_Bot.EModules.Commands.Maintenance
{
    class get_all_invs0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 19
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                foreach (var Guild in Program.GetAllShardGuilds())
                {
                    try
                    {
                        await arg.Author.SendMessageAsync($"{Guild.Name} discord.gg/{(await Guild.GetInvitesAsync()).First().Code}");
                    }
                    catch (Exception e)
                    {
                        //await arg.Author.SendMessageAsync(e.Message);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

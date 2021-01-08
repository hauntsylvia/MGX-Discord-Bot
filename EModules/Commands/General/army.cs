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
    class army
    {
        public static Dictionary<char, string> Stuff = new Dictionary<char, string>()
        {
            {'a', "alpha"},
            {'b', "bravo"},
            {'c', "charlie"},
            {'d', "december"},
            {'e', "echo"},
            {'f', "foxtrot"},
            {'g', "golf"},
            {'h', "hotel"},
            {'i', "india"},
            {'j', "juliett"},
            {'k', "kilo"},
            {'l', "lima"},
            {'m', "motel"},
            {'n', "november"},
            {'o', "oscar"},
            {'p', "papa"},
            {'q', "q"},
            {'r', "romeo"},
            {'s', "september"},
            {'t', "tango"},
            {'u', "uniform"},
            {'v', "victor"},
            {'w', "whiskey"},
            {'x', "xray"},
            {'y', "y"},
            {'z', "zero"},
            {' ', " "},
        };

        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "fun",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            if(arg.Content.ToLower().Contains(' '))
            {
                string ToConvert = arg.Content.ToLower().Split(new[] { ' ' }, 2)[1];

                string Finished = "";

                foreach(char Letter in ToConvert)
                {
                    if(Stuff.ContainsKey(Letter))
                    {
                        Finished = Finished + " " + Stuff[Letter];
                    }
                    else
                    {
                        Finished = Finished + Letter;
                    }
                }

                await arg.Channel.SendMessageAsync(Finished);
            }
            else
            {
                await arg.Channel.SendMessageAsync("You need to specify the message to convert!");
            }
        }
    }
}

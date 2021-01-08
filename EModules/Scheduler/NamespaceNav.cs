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

namespace MGX_Discord_Bot.EModules.Scheduler
{
    class NamespaceNav
    {
        public static List<string> GetAllTypes(string Namespace)
        {
            List<string> types = new List<string>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type.Namespace == Namespace & !types.Contains(type.Name) & type.IsClass & !type.Name.Contains("<") & !type.Name.Contains(">"))
                {
                    types.Add(type.Name);
                }
            }
            return types;
        }

        public static List<Type> GetTypesRaw(string Namespace)
        {
            List<Type> types = new List<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type.Namespace == Namespace & !types.Contains(type) & type.IsClass & !type.Name.Contains("<") & !type.Name.Contains(">"))
                {
                    types.Add(type);
                }
            }
            return types;
        }
    }
}

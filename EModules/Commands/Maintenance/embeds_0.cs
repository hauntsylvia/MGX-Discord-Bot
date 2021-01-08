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
    class embeds_0
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
                var Emb = new EmbedBuilder()
                {
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        { 
                            new EmbedFieldBuilder()
                            {
                                Name = "Rules",
                                Value = "🔗 - Sexual, racial, religious, and general harassment are all against our rules as well as what we stand for. It will not be tolerated under any circumstance.This includes any sort of spam pinging against any user.\n"+
                                "🔗 - Heavy aggression, including sexual harassment, all-caps, heavy swearing towards a user, purposefully getting others mad, yelling at staff, etc. will not be tolerated. Swearing is allowed, as long as it isn't intended as hate, and only as a joke. This does not apply if the user being directed with swearing doesn't want you to swear at them at all.\n"+
                                "🔗 - All suicidal threats will be taken as a reason to contact actual authorities if possible. This offense is an instant-ban with difficulty to appeal for.\n"+
                                "🔗 - Questioning staff is encouraged, harassing staff will not be tolerated. Please know the difference, and do not get aggressive with anyone. If you believe a staff member is abusing their position, please send an email to `kxtze.cxffee@gmail.com`.\n"
                            }
                        },
                    },
                    Color = new Color(210, 110, 110),
                    Timestamp = DateTime.UtcNow,
                };
                await arg.Channel.SendFileAsync(@"D:\My Stuffs\_kryophil\VxidServer\in-server-banner-STAFF.jpg");
                await arg.Channel.SendMessageAsync("", false, Emb.Build());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

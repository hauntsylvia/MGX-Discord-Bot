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
using MGX_Discord_Bot.EModules.Holders.InternalHolders;
using MGX_Discord_Bot.EModules.Scheduler;

namespace MGX_Discord_Bot.EModules.Commands.Maintenance
{
    class dm0
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            RequiredPermissions = 20
        };
        /// <summary>
        /// TKey = ChannelId, TValue = UserId
        /// </summary>
        private static Dictionary<ulong, ulong> Associations = new Dictionary<ulong, ulong>();
        private static bool IsSet = false;
        public static async Task Maindo(SocketMessage arg)
        {
            if(arg.Content.ToLower().Contains("close"))
            {
                if(Associations.ContainsKey(arg.Channel.Id))
                {
                    Associations.Remove(arg.Channel.Id); 
                    await arg.Channel.SendMessageAsync("Done!");
                }
            }
            else
            {
                var UserToDm = Program.Client.GetUser(ulong.Parse(arg.Content.ToLower().Split(' ')[1]));
                try
                {
                    Associations.Add(arg.Channel.Id, UserToDm.Id);
                    if(!IsSet)
                    {
                        IsSet = true;
                        Program.Client.MessageReceived += MsgRec;
                    }
                    await arg.Channel.SendMessageAsync("Set!");
                }
                catch (Exception e)
                {
                    await arg.Channel.SendMessageAsync(e.Message);
                }
            }
        }
        private static async Task MsgRec(SocketMessage arg)
        {
            if(arg.Channel.GetType() == typeof(SocketDMChannel) && Associations.ContainsValue(arg.Author.Id) && !arg.Author.IsBot)
            {
                var Emb = new EmbedBuilder()
                {
                    Author = new EmbedAuthorBuilder()
                    {
                        Name = $"{arg.Author.Username}#{arg.Author.Discriminator}",
                        IconUrl = arg.Author.GetAvatarUrl(),
                    },
                    Color = ColorHolder.Parse(arg.Content),
                    Description = arg.Content,
                    Timestamp = arg.Timestamp
                };
                foreach(var Key in Associations.Keys)
                {
                    var Val = Associations[Key];
                    if (Val == arg.Author.Id)
                    {
                        await ((SocketTextChannel)Program.Client.GetChannel(Key)).SendMessageAsync("", false, Emb.Build());
                        break;
                    }
                }
            }
            else if(Associations.ContainsKey(arg.Channel.Id) && !arg.Author.IsBot && RankingManager.BotOwners.ContainsKey(arg.Author.Id) && RankingManager.BotOwners[arg.Author.Id] >= 20 &&!arg.Content.Contains($"{Program.Prefix}dm0"))
            {
                try
                {
                    var UserToDm = Program.Client.GetUser(Associations[arg.Channel.Id]);
                    await UserToDm.SendMessageAsync(arg.Content);
                    //await arg.AddReactionAsync(Emote.Parse("<:PleadingCold:752119782660374576>"));
                    var Ems = ((SocketTextChannel)arg.Channel).Guild.Emotes;
                    await arg.AddReactionAsync(Ems.ElementAt(new Random().Next(0, Ems.Count - 1)));
                }
                catch(Exception e)
                {
                    await arg.Channel.SendMessageAsync(e.Message);
                }
            }
        }
    }

}

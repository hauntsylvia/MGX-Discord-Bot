using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

namespace MGX_Discord_Bot.EModules.Entities
{
    class Prompt
    {
        private ISocketMessageChannel channelForPrompt;
        public ISocketMessageChannel ChannelForPrompt
        {
            set { channelForPrompt = value; }
            get { return channelForPrompt; }
        }

        public string Title;

        public EmbedBuilder Embed;

        public Emoji[] Options;

        public SocketUser Target;

        /// <summary>
        /// In seconds.
        /// </summary>
        public int MaxTime = 10;

        public RestUserMessage Message;

        public async Task<Emoji> ShowPrompt()
        {
            Emoji CompletedWith = null;
            string ToSend = $"{Title}\n`react with ";

            foreach (Emoji Option in Options)
            {
                ToSend += $"'{Option.Name}', ";
            }

            ToSend = ToSend.Substring(0, ToSend.Length - 2) + "`";

            RestUserMessage MsgToReact;

            if(Embed != null)
            {
                MsgToReact = await ChannelForPrompt.SendMessageAsync("", false, Embed.Build());
            }
            else
            {
                MsgToReact = await ChannelForPrompt.SendMessageAsync(ToSend);
            }

            Message = MsgToReact;

            foreach(Emoji Em in Options)
            {
                try
                {
                    await MsgToReact.AddReactionAsync(Em);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            try
            {
                async Task Ev(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
                {
                    if (arg3.User.Value.Id == Target.Id && Options.Contains(arg3.Emote) && arg1.Value.Id == MsgToReact.Id)
                    {
                        CompletedWith = (Emoji)arg3.Emote;
                        await arg1.Value.RemoveAllReactionsAsync();
                        Program.Client.ReactionAdded -= Ev;
                    }
                }
                Program.Client.ReactionAdded += Ev;
                while (CompletedWith == null && MaxTime > 0)
                {
                    Thread.Sleep(1000);
                    MaxTime -= 1;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return CompletedWith;
        }
    }
}

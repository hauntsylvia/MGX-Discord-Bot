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
    class setup
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "utils",
            Premium = false,
            RequiredPermissions = 20
        };
        public static async Task Maindo(SocketMessage arg)
        {
            try
            {
                var GuildId = ((SocketTextChannel)arg.Channel).Guild.Id;
                int Ran = new Random().Next(1, CustomResponses.WordResp.Count);
                CustomResponses.UpdateResponses();
                bool CusResp = await SetupX(arg, $"Would you like me to send occasional replies to certain messages?\n\n`Reply with 'yes' or 'no'`");
                await arg.Channel.TriggerTypingAsync();
                Thread.Sleep(1250);
                bool GloblCht = await SetupX(arg, $"Do you want to participate in MGX globalchat?\nMGX globalchat allows your server to communicate with other servers through a specific channel. If you'd like more information about it, please friend and message {Program.MyAccount.Username}#{Program.MyAccount.Discriminator}.\n\n`Reply with 'yes' or 'no'`");
                if (GloblCht)
                {
                    await globalchat.DefSet(arg, true);
                }
                await arg.Channel.TriggerTypingAsync();
                Thread.Sleep(2000);
                bool QuarantineCmd = await SetupX(arg, $"{Program.Prefix}quarantine is a command that can be used by anyone, and can mute people (it was created more as a meme than for genuine usefullness). Would you like this command to be usable?\n\n`Reply with 'yes' or 'no'`");
                GuildStuff.NewGuildSetting(GuildStuff.GuildSettings.CustomResponses, GuildId, CusResp);
                GuildStuff.NewGuildSetting(GuildStuff.GuildSettings.CanUseQuarantine, GuildId, QuarantineCmd);
                string Desc = $"**Applied New Settings!**\nCongrats! You've successfully setup your server for use with {Program.Client.CurrentUser.Username}.\nCurrent settings:\n```\nCustom Responses: {ResolveBool(CusResp)}\nMGX Globalchat: {ResolveBool(GloblCht)}\nQuarantine Command: {ResolveBool(QuarantineCmd)}\n```";
                var Emb = CreateEmbed($"{Program.Client.CurrentUser.Username}", "<3", Desc, null);
                await arg.Channel.SendMessageAsync("", false, Emb);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private static Emoji ResolveBool(bool Arg)
        {
            return Arg ? new Emoji("✅") : new Emoji("❌");
        }
        private static async Task<bool> SetupX(SocketMessage arg, string Message)
        {
            try
            {
                var WFR = new WaitForResponse() { TimeLimitS = 60, ChannelId = arg.Channel.Id, UserId = arg.Author.Id };
                await arg.Channel.SendMessageAsync(Message);
                var CusRespReply = await WFR.Start();
                if (CusRespReply != null && CusRespReply.Content.ToLower().Contains("yes"))
                {
                    await CusRespReply.AddReactionAsync(new Emoji("✅"));
                }
                else
                {
                    await CusRespReply.AddReactionAsync(new Emoji("❌"));
                }
                return CusRespReply == null ? false : CusRespReply.Content.ToLower().Contains("yes");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
        private static Embed CreateEmbed(string AuthorName, string FooterText, string Description, string ImageUrl = null, Color? Color = null)
        {
            Color = Color ?? new Color(255, 110, 110);
            var Emb = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder()
                {
                    Name = AuthorName,
                },
                Color = Color,
                Timestamp = DateTime.UtcNow,
                Description = Description,
                ImageUrl = ImageUrl,
                Footer = new EmbedFooterBuilder()
                {
                    Text = FooterText
                },
            };
            return Emb.Build();
        }
    }
}

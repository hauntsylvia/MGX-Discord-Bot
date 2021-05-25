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
using System.Net.Http.Headers;
using Discord;
using Discord.API;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;

namespace MGX_Discord_Bot.EModules._X
{
    class Secondary
    {
        //run on separate thread
        private static DiscordSocketClient Cl;
        public static async Task Secondary_Thread()
        {
            //Cl = new DiscordSocketClient();
            //await Cl.LoginAsync(0, Token);
            Program.Client.UserJoined += Client_UserJoined;
            Console.WriteLine("Secondary thread login executed");
        }
        //private static string Token = "redacted";
        public static async Task MyClient_MessageReceived(SocketMessage arg)
        {
               
            //if(arg.Content.ToLower().Contains("discord.gift") && arg.Author.Id != Program.Client.CurrentUser.Id)
            //{
            //    try
            //    {
            //        DateTime Bef = DateTime.UtcNow;
            //        string Url = $"https://discordapp.com/api/v8/entitlements/gift-codes/{arg.Content.Split(new[] { "gift/" }, StringSplitOptions.None)[1].Split(' ')[0]}/redeem";
            //        HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Post, Url);
            //        RequestMessage.Headers.Add("authorization", "mfa.LuaPRqOJkSbIFnKqs5ZGk37wYQnJ4QoO-J8pBAqUMIt32VCCaSn-fnfF-SYZV60IgurybhnjDxJoPcdw951G");
            //        using (HttpClient HttpC = new HttpClient())
            //        {
            //            var Res = await HttpC.SendAsync(RequestMessage);
            //            await Program.MyAccount.SendMessageAsync($"{arg.Content}");
            //            await Program.MyAccount.SendMessageAsync($"POST to {Url} - {await Res.Content.ReadAsStringAsync()}\n\nop done in `{(DateTime.UtcNow - Bef).TotalMilliseconds}ms`");
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //}
        }

        private static async Task Client_UserJoined(SocketGuildUser arg)
        {
            if (arg.Guild.Id == Program.VoidServer.Id)
            {
                try
                {
                    var RoleToGive = Program.VoidServer.GetRole(717297724529049720);

                    await arg.AddRoleAsync(RoleToGive);
                }
                catch(Exception e) 
                {
                    Console.WriteLine($"Check role to give, check permissions, check thread-safe vars, check Program.MyAccount (SocketUser) - {e}");
                }
            }
        }
    }
}

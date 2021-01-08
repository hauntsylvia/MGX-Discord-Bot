using System;
using System.Threading.Tasks;
using Discord.WebSocket;

using MGX_Discord_Bot.EModules.Scheduler;
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Commands.General
{
    class daily
    {
        public static CommandInformation Info = new CommandInformation()
        {
            Alias = false,
            Category = "currency",
            Premium = false,
            RequiredPermissions = 1
        };
        public static async Task Maindo(SocketMessage arg)
        {
            string Key = $"DAILY-CURR";
            if(StoreHandler.LoadData(arg.Author.Id, Key) != null && (DateTime.UtcNow - DateTime.Parse(StoreHandler.LoadData(arg.Author.Id, Key))).TotalHours <= 0)
            {
                await arg.Channel.SendMessageAsync($"you have to wait `{(DateTime.Parse(StoreHandler.LoadData(arg.Author.Id, Key)) - DateTime.UtcNow).Hours}` hours before you can do this again!");
            }
            else
            {
                long ToAdd = 1200;
                StoreHandler.SaveData(arg.Author.Id, Key, DateTime.UtcNow + TimeSpan.FromDays(1));
                ShopClass.AddCurrency(arg.Author.Id, ToAdd);
                await arg.Channel.SendMessageAsync($"added ${ToAdd} to your balance ~~noob~~");
            }
        }
    }
}

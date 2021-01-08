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

using Discord.API;
using Discord.Commands;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;

using MGX_Discord_Bot.EModules.Entities;
using Discord;

namespace MGX_Discord_Bot.EModules.Scheduler
{

    class Receipt
    {
        public long Cost;
        public long Amount;
        public bool Success;
    }
    public enum Product
    {
        pills,
        padlock,
        flower,
        phone,
    };
    class Economy
    {
        /// <summary>
        /// Value (object array) is structured as such: [0]string = description , [1]long = cost, [2] = emoji
        /// </summary>
        public static Dictionary<Product, object[]> Definitions = new Dictionary<Product, object[]>()
        {
            {Product.pills, new object[]{"Increase that pp size you got there. ;)", 950, new Emoji("💊")}},
            {Product.padlock, new object[]{"Keep people from your money! (One-time use)", 20000, new Emoji("🔒")}},
            {Product.flower, new object[]{"Flowers to give someone, they'll love it!", 100, new Emoji("🌹")}},
            {Product.phone, new object[]{"A block tricked into thinking with electricity.", 10000, new Emoji("📱")}},
        };
        public static string _eco_dir = @".\Eco\";
        public static Receipt Buy(Product arg, long Count, ulong UserId)
        {
            if(!Directory.Exists(_eco_dir))
            {
                Directory.CreateDirectory(_eco_dir);
            }
            long Cost = long.Parse(Definitions[arg][1].ToString()) * Count;
            long UserBalance = ShopClass.ReadCurrency(UserId);
            if(UserBalance < Cost)
            {
                return new Receipt()
                {
                    Amount = Count,
                    Success = false,
                    Cost = Cost,
                };
            }
            string UserInventory = _eco_dir + UserId;
            string UserProductFile = UserInventory + $@"\{arg}.txt";
            if (!Directory.Exists(UserInventory))
            {
                Directory.CreateDirectory(UserInventory);
            }
            long ProductCountOfUser = File.Exists(UserProductFile) ? long.Parse(File.ReadAllText(UserProductFile)) : 0;
            using(StreamWriter sw = new StreamWriter(UserProductFile))
            {
                sw.Write(ProductCountOfUser + Count);
            }
            ShopClass.AddCurrency(UserId, -Cost);
            return new Receipt()
            {
                Amount = Count,
                Success = true,
                Cost = Cost,
            };
        }
        public static void ChangeProductCountForUser(Product arg, long Count, ulong UserId)
        {
            if (!Directory.Exists(_eco_dir))
            {
                Directory.CreateDirectory(_eco_dir);
            }
            string UserInventory = _eco_dir + UserId;
            string UserProductFile = UserInventory + $@"\{arg}.txt";
            if (!Directory.Exists(UserInventory))
            {
                Directory.CreateDirectory(UserInventory);
            }
            long ProductCountOfUser = File.Exists(UserProductFile) ? long.Parse(File.ReadAllText(UserProductFile)) : 0;
            using(StreamWriter sw = new StreamWriter(UserProductFile))
            {
                sw.Write(ProductCountOfUser + Count);
            }

        }
        public static long CountProductForUser(Product arg, ulong UserId)
        {
            if(!Directory.Exists(_eco_dir))
            {
                Directory.CreateDirectory(_eco_dir);
            }
            string UserInventory = _eco_dir + UserId;
            string UserProductFile = UserInventory + $@"\{arg}.txt";
            if (!Directory.Exists(UserInventory))
            {
                Directory.CreateDirectory(UserInventory);
            }

            if(File.Exists(UserProductFile))
            {
                return long.Parse(File.ReadAllText(UserProductFile));
            }
            else
            {
                return 0;
            }
        }
    }
}

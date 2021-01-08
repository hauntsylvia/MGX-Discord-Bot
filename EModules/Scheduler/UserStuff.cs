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
using MGX_Discord_Bot.EModules.Entities;

namespace MGX_Discord_Bot.EModules.Scheduler
{
    class UserStuff
    {
        public static long GetCmdCountOfUser(ulong UserId)
        {
            string Key = $"CMD-COUNT";
            if (StoreHandler.LoadData(UserId, Key) == null)
            {
                StoreHandler.SaveData(UserId, Key, 0);
                return 0;
            }
            else if(long.TryParse(StoreHandler.LoadData(UserId, Key), out long IfSuccess))
            {
                return IfSuccess;
            }
            else
            {
                StoreHandler.SaveData(UserId, Key, 0);
                return 0;
            }
        }
        public static job GetJob(ulong UserId)
        {
            if(StoreHandler.LoadData(UserId, $"JOB") != null)
            {
                var Job = new job()
                {
                    Name = StoreHandler.LoadData(UserId, $"JOB"),
                    Pay = long.Parse(StoreHandler.LoadData(UserId, $"JOB-PAY")),
                };
                Job.StillAvailable = JobsList.ContainsKey(Job.Name);
                DateTime.TryParse(StoreHandler.LoadData(UserId, "JOB-TIMESTAMP"), out Job.Timestamp);

                return Job;
            }
            else
            {
                return null;
            }
        }

        public static void SaveJob(ulong UserId, job arg)
        {
            StoreHandler.SaveData(UserId, "JOB", arg.Name);
            StoreHandler.SaveData(UserId, "JOB-PAY", arg.Pay);
            StoreHandler.SaveData(UserId, "JOB-TIMESTAMP", DateTime.UtcNow);
        }


        private static Dictionary<string, long> JobsList = new Dictionary<string, long>()
        {
            {"CosTan", 100},
            {"ess kay", 222},
            {"im sad help", 1},
            {"no ♥", 50},
            {"XDDDDDDDDDDDDDDDDDDDDD", 45},
            {"pls no", 200},
            {"Pillow Smasher", 500},
            {"covid19", 160},
            {"oop", 3},
            {"HackerMan", 100},
            {"NZXT Associate", 1000},
            {"doggo :D", 1000},
            {"sigh", 5},
            {"idek what im writing these arent actual jobs lol", 176},
            {"Developer", 75},
            {"well yes but actually no", 100},
            {"<3", 2},
            {"UTF-8 Encoder", 180},
            {"McDonlads Werkr", 325},
            {"S I N N E R", 400},
        };
        private static Random R = new Random();
        public static job GenerateRandomJob(bool RandomPay)
        {
            var RandomChosen = JobsList.Keys.ToArray()[R.Next(0, JobsList.Count)];
            var Job = new job()
            {
                Name = RandomChosen,
                Pay = RandomPay ? R.Next(1, 750) : JobsList[RandomChosen],
                StillAvailable = true,
            };

            return Job;
        }
    }
}

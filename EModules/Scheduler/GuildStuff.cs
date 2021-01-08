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
    class GuildStuff
    {
        private static readonly string Base = @".\GuildVals";
        public static void SetRank(ulong GuildId, ulong UserId, int Permissions)
        {
            if (!Directory.Exists(Base))
            {
                Directory.CreateDirectory(Base);
            }

            if (!Directory.Exists($@"{Base}\{GuildId}"))
            {
                Directory.CreateDirectory($@"{Base}\{GuildId}");
            }

            while (!CanReadWrite($@"{Base}\{GuildId}\{UserId}.txt")) 
            { 

            }

            using (StreamWriter SW = new StreamWriter($@"{Base}\{GuildId}\{UserId}.txt"))
            {
                SW.WriteLine(Permissions);
                SW.Close();
            }
        }
        private static readonly Dictionary<int, string> RankNames = new Dictionary<int, string>()
        {
            {20, "Owner"},
            {19, "Admin"},
            {18, "Mod"},
            {2, "Trusted User"},
            {1, "User"},
        };
        public static string GetNameOfRank(int Rank)
        {
            return RankNames[Rank];
        }

        public static int ReadRank(ulong GuildId, ulong UserId)
        {
            if (!Directory.Exists(Base))
            {
                Directory.CreateDirectory(Base);
            }

            if (!Directory.Exists($@"{Base}\{GuildId}"))
            {
                Directory.CreateDirectory($@"{Base}\{GuildId}");
            }

            var Guild = Program.Client.GetGuild(GuildId);
            var User = Guild.GetUser(UserId);

            while (!CanReadWrite($@"{Base}\{GuildId}\{UserId}.txt"))
            {

            }

            if (!File.Exists($@"{Base}\{GuildId}\{UserId}.txt"))
            {
                var Fi = File.Create($@"{Base}\{GuildId}\{UserId}.txt");
                Fi.Close();

                using (StreamWriter SW = new StreamWriter($@"{Base}\{GuildId}\{UserId}.txt"))
                {
                    if (Guild.OwnerId == User.Id)
                    {
                        SW.WriteLine(20);
                    }
                    else if (User.GuildPermissions.Administrator || User.GuildPermissions.ManageGuild)
                    {
                        SW.WriteLine(19);
                    }
                    else
                    {
                        SW.WriteLine(1);
                    }
                    SW.Close();
                }
            }

            using (StreamReader SR = new StreamReader($@"{Base}\{GuildId}\{UserId}.txt"))
            {
                int ret = Int32.Parse(SR.ReadToEnd());
                SR.Close();
                return ret;
            }
        }

        public enum GuildSettings
        {
            GlobalChannelForOthers,
            CustomResponses,
            CanUseQuarantine,
            ReplaceInvites,
        }
        public static bool NewGuildSetting(GuildSettings Arg, ulong GuildId, bool SetTo, object ExtraSaveData = null)
        {
            string SettingsDir = $@"{Base}\{GuildId}\{Arg}";
            if (!Directory.Exists($@"{Base}\{GuildId}"))
            {
                Directory.CreateDirectory($@"{Base}\{GuildId}");
            }
            if (!Directory.Exists(SettingsDir))
            {
                Directory.CreateDirectory(SettingsDir);
            }
            string ToRead = $@"{SettingsDir}\ret.txt";
            string ToSaveExt = $@"{SettingsDir}\strsave.txt";

            while (!CanReadWrite(ToRead))
            {

            }

            if (ExtraSaveData != null)
            {
                using (StreamWriter SW = new StreamWriter(ToSaveExt))
                {
                    SW.WriteLine(ExtraSaveData.ToString());
                    SW.Close();
                }
            }

            using (StreamWriter SW = new StreamWriter(ToRead))
            {
                SW.Write(SetTo);
                SW.Close();
                return SetTo;
            }
        }

        public static string ReturnExtraSaved(GuildSettings Arg, ulong GuildId)
        {
            string SettingsDir = $@"{Base}\{GuildId}\{Arg}";
            if (!Directory.Exists($@"{Base}\{GuildId}"))
            {
                Directory.CreateDirectory($@"{Base}\{GuildId}");
            }
            if (!Directory.Exists(SettingsDir))
            {
                Directory.CreateDirectory(SettingsDir);
            }

            string ToSaveExt = $@"{SettingsDir}\strsave.txt";

            while (!CanReadWrite(ToSaveExt))
            {

            }

            if (File.Exists(ToSaveExt))
            {
                return File.ReadAllText(ToSaveExt);
            }
            else
            {
                return " ";
            }
        }

        public static bool ReadSetting(GuildSettings Arg, ulong GuildId)
        {
            string SettingsDir = $@"{Base}\{GuildId}\{Arg}";
            if (!Directory.Exists($@"{Base}\{GuildId}"))
            {
                Directory.CreateDirectory($@"{Base}\{GuildId}");
            }
            if (!Directory.Exists(SettingsDir))
            {
                Directory.CreateDirectory(SettingsDir);
            }
            string ToRead = $@"{SettingsDir}\ret.txt";
            while (!CanReadWrite(ToRead))
            {

            }

            if (File.Exists(ToRead))
            {
                return bool.Parse(File.ReadAllText(ToRead));
            }
            else
            {
                return false;
            }
        }
        public static void ResetData(ulong GuildId)
        {
            string SettingsDir = $@"{Base}\{GuildId}";
            foreach (var Dir in Directory.GetDirectories(SettingsDir))
            {
                foreach(var Fi in Directory.GetFiles(Dir))
                {
                    File.Delete(Fi);
                }
            }
            foreach(var Dir in Directory.GetDirectories(SettingsDir))
            {
                Directory.Delete(Dir);
            }
            foreach(var Fi in Directory.GetFiles(SettingsDir))
            {
                File.Delete(Fi);
            }

            var Embed = new EmbedBuilder()
            {
                Color = Color.DarkerGrey,
                Description = $"Reset data for guild of {GuildId}.",
                Timestamp = DateTime.UtcNow,
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"{GuildId}"
                },
            };
            LogEvent.SendLog(Embed);
        }
        private static bool CanReadWrite(string Path)
        {
            try
            {
                if(File.Exists(Path))
                {
                    using (StreamReader sr = new StreamReader(Path))
                    {
                        sr.ReadToEnd();

                        sr.Dispose();
                        sr.Close();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

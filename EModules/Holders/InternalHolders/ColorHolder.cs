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

namespace MGX_Discord_Bot.EModules.Holders.InternalHolders
{
    class ColorHolder
    {
        private static Dictionary<string, Color> InternalCols = new Dictionary<string, Color>()
        {
            {"dark teal", Color.DarkTeal},
            {"dark blue", Color.DarkBlue},
            {"dark gray", Color.DarkerGrey},
            {"dark green", Color.DarkGreen},
            {"dark magenta", Color.DarkMagenta},
            {"dark orange", Color.DarkOrange},
            {"dark purple", Color.DarkPurple},
            {"dark red", Color.DarkRed},
            {"light gray", Color.LightGrey},
            {"light orange", Color.LightOrange},
            {"magenta", Color.Magenta},
            {"orange", Color.Orange},
            {"purple", Color.Purple},
            {"red", Color.Red},
            {"blue", Color.Blue},
            {"teal", Color.Teal},
            {"gold", Color.Gold},
            {"green", Color.Green},
        };

        public static Color Parse(string ColorToConvert)
        {
            int Smallest = 500;
            Color ToUse = Color.DarkGrey;
            foreach(string ColStr in InternalCols.Keys)
            {
                int Distance = Algorithms.LevenshteinDistance.Compute(ColorToConvert, ColStr);
                if(Distance < Smallest)
                {
                    Smallest = Distance;
                    ToUse = InternalCols[ColStr];
                }
            }

            return ToUse;
        }
    }
}

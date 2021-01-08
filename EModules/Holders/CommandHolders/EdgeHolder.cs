using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGX_Discord_Bot.EModules.Holders.CommandHolders
{
    class EdgeHolder
    {
        private List<string> Edge = new List<string>()
        {
            {"Let me pierce your dark, unforgiving veins with my cold blade."},
            {"They've been watching us for awhile now."},
            {"MGX is plagued, don't trust her."},
            {"Relying on them won't work."},
            {"Efficiency will keep you alive, let's just hope you apply it at the right time."},
            {"Please, don't leave me here!"},
            {"It'll keep happening!"},
            {"I don't want to die, please!"},
            {"Let it feed on the carcass of the dead and/or dying."},
            {"Imagine being sewed to your soul mate. Is that exciting or chilling to you? Genuine question."},
            {"Relaxation puts your guard down."},
            {"It's cold..."},
            {"No!"},
            {"I. Don't. Care."},
            {"Why are you asking?"},
            {"Helf mir, bitte.."},
            {"..."}
        };

        public string ReturnRandomResponse()
        {
            Random Rand = new Random();
            int ChosenWord = Rand.Next(0, Edge.Count());
            return Edge[ChosenWord];
        }
    }
}

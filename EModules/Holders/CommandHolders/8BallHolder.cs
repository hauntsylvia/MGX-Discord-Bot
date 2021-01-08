using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGX_Discord_Bot.EModules.Holders.CommandHolders
{
    class _8BallHolder
    {
        private List<string> Conf = new List<string>()
        {
            {"It is bound to happen."},
            {"It is decidedly so."},
            {"Most likely."},
            {"Relying on it isn't a bad idea."},
            {"Signs point to 'yes'"},
            {"Undoubtedly"}
        };

        private List<string> Neutral = new List<string>()
        {
            {"*h a z e*"},
            {"Ask later."},
            {"Concentrate and ask again."}
        };

        private List<string> Negate = new List<string>()
        {
            {"No. Stop asking."},
            {"All sources say no."},
            {"The outlook of this isn't very good."},
            {"Don't rely on it."},
        };

        public string ReturnRandomResponse()
        {
            Random Rand = new Random();
            int ChosenDictionary = Rand.Next(0, 3);
            if(ChosenDictionary == 0)
            {
                int ChosenResponse = Rand.Next(0, Conf.Count());
                return Conf[ChosenResponse];
            }
            if (ChosenDictionary == 1)
            {
                int ChosenResponse = Rand.Next(0, Neutral.Count());
                return Neutral[ChosenResponse];
            }
            else
            {
                int ChosenResponse = Rand.Next(0, Negate.Count());
                return Negate[ChosenResponse];
            }
        }
    }
}

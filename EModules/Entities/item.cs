using MGX_Discord_Bot.EModules.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGX_Discord_Bot.EModules.Entities
{
    class shoppage
    {
        public item[] Items;
    }
    class item
    {
        public long Cost;
        public string Definition;
        public string Name;
    }
}

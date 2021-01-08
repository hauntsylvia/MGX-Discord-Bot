using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MGX_Discord_Bot.EModules.Scheduler.UserStuff;

namespace MGX_Discord_Bot.EModules.Entities
{
    class job
    {
        public long Pay;
        public string Name;
        public bool StillAvailable;
        public DateTime Timestamp;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGX_Discord_Bot.EModules.Entities
{
    class CommandInformation
    {
        public bool Alias = true;
        public bool Premium = false;
        public int RequiredPermissions = 20;
        public string Category;
    }
}

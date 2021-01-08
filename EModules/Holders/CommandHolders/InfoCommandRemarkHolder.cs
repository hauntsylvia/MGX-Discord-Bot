using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGX_Discord_Bot.EModules.Holders.CommandHolders
{
    class InfoCommandRemarkHolder
    {
        private static Dictionary<ulong, string> CustomRemarks = new Dictionary<ulong, string>()
        {
            //{541094792550154251, "Motivation For MGX\nJasie's Jellybean\n(After breakup but it's cool)My kit kat"}, // my kit kat <3
            {541094792550154251, "A genuinely cool person\n10/10 tbh\n- (tell Omar if you want this removed) -"}, // my kit kat </3
            {421126160726884353, "Creator Of MGX\n~~Kat's Tree~~"}, // me (old)
            {528750326107602965, "Creator Of MGX\n~~Kat's Tree~~"}, // me
            {456936285848862722, "Single and ready to pringle"}, // whats his face (omar)
            {268502538637148172, "get jordan co-owner rank"}, // jordan
            {534193192329805854, $"[Invite me](https://discordapp.com/oauth2/authorize?client_id={Program.Client.CurrentUser.Id}&scope=bot&permissions=8) \n [For support](https://discord.gg/G6beAPs)"}, // MGX
            {573480755787988993, "Sad excuse for a brother"}, // Raakin's brother
            {453698396075851777, "jordan and omar is a cutie"}, // Pamela [pam spam spickity spam]
        };

        public static string ReturnRemarkOfUser(ulong Id)
        {
            if (CustomRemarks.ContainsKey(Id))
            {
                return CustomRemarks[Id];
            }
            else
            {
                return "N/A";
            }
        }
    }
}

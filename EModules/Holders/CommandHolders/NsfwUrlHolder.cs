using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGX_Discord_Bot.EModules.Holders.CommandHolders
{
    class NsfwUrlHolder
    {
        private List<string> BoobsURLs = new List<string>()
        {
            {"https://uncensored-hentai.com/wp-content/uploads/2018/11/Kyonyuu-JK-ga-Ojisan-Chinpo-to-Jupo-Jupo-Iyarashii-Sex-Shitemasu-Episode-2-1.jpg"},
            {"https://hentaid.tv/wp-content/uploads/2018/07/Marshmallow-Imouto-Succubus-japanese-monster-hentai-demon-pussy-teens-milf.jpg"},
            {"https://pornogifs.net/wp-content/uploads/2019/10/58466934.gif"},
            {"https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcS8fA14Fwdf9DSlOEJVuOw4iVsrlN9R5V1zsSIzKwr6bL92JNWi"},
            {"https://www.youngporno.com/images/galleries/0035/1951/3a880a63e418d9566a1045d607889133.jpg"},
            {"https://external-preview.redd.it/JOwM5gXuzimLFtnSk2UiQD5X3bRSsETmdLE04MQ3WNg.png?auto=webp&s=f66afd5f0a07bad9bcdff89258c4cf4451531f6f"},
            {"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT8RSm0RpsJFXwjWYaHvIRKNBVnte9h6SZQ4luqq75bDDVNzCzl&s"},
            {"https://external-preview.redd.it/iIzx3xvw8qTHmv_FL0YPhwxrSW5le8AgBQFV2-HEkpY.jpg?auto=webp&s=23f6d35e15d86bcc04f79043afad16d0e792ed14"},
            {"https://erowall.com/wallpapers/large/14248.jpg"},
            {"https://w.wallhaven.cc/full/48/wallhaven-48pqmy.jpg"},
            {"http://cdn.pichunter.com/173/3/1733447/1733447_2_o.jpg"},
            {"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS0otNoowdPhM9JRON4v7fUX3TE2z0zkNs1lz7oolhaAUTY3-P9eg&s"},
            {"https://media0.giphy.com/media/UNXJpIkLr8m0o/source.gif"},
            {"https://i.kym-cdn.com/photos/images/original/001/043/444/1aa.gif"},
            {"https://tubewolf.com/contents/videos_screenshots/126000/126217/preview.jpg"},
            {"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSiTcVWPi6cSQLLxsOIzJkXRUCAzO5TemvUCMfDwgFiAeJeqblVxA&s"},
            {"https://erowall.com/wallpapers/large/24709.jpg"},
        };

        private List<string> AnalURLs = new List<string>()
        {
            {"https://images.sex.com/images/pinporn/2018/07/10/620/19705214.jpg?site=sex&user=mahex"},
            {"https://lh3.googleusercontent.com/proxy/gQHvpaM5YRB0r8O6J-mEnS5_GuO6vMGLbSAIbWPa1bGmyLa4DU-8NG_0MuCODy6DsMTnUrrImsiqOWOJV6iLYA8a0TaSuuv3QPreTX4ilGyKe0JMlIG9_J5xdk9_VxYHeBdDK6Q"},
            {"https://konachan.com/sample/15016e40d3203344d2a4d9f0ff812cc6/Konachan.com%20-%20110474%20sample.jpg"},
            {"https://konachan.com/jpeg/0231d726e78a750309d08cb9be3d8fd5/Konachan.com%20-%20187485%20aimai_renai%20anal%20ass%20bed%20black_hair%20blush%20censored%20game_cg%20long_hair%20nude%20penis%20purple_eyes%20pussy%20saeki_emi%20satofuji_masato%20sex%20socks%20spread_legs.jpg"},
            {"https://konachan.com/jpeg/f74e93f054783769a4fdfc799fd6b9ec/Konachan.com%20-%20187484%20aimai_renai%20anus%20ass%20ass_grab%20bed%20black_hair%20blonde_hair%20blue_eyes%20blush%20cat_smile%20censored%20game_cg%20long_hair%20nude%20purple_eyes%20pussy%20saeki_emi%20socks.jpg"},
            {"https://konachan.com/jpeg/d281dff55ce1b6226fdb5a345e4e2587/Konachan.com%20-%20187384%20aimai_renai%20ass%20black_hair%20breasts%20censored%20game_cg%20long_hair%20nude%20penis%20purple_eyes%20pussy%20saeki_emi%20satofuji_masato%20sex.jpg"},
            {"http://cdn.pichunter.com/303/5/3035119/3035119_2_o.jpg"},
        };
        public string BoobsReturnRandomURL()
        {
            Random Rand = new Random();
            int ChosenWord = Rand.Next(0, BoobsURLs.Count());
            return BoobsURLs[ChosenWord];
        }

        public string AnalReturnRandomURL()
        {
            Random Rand = new Random();
            int ChosenWord = Rand.Next(0, AnalURLs.Count());
            return AnalURLs[ChosenWord];
        }
    }
}

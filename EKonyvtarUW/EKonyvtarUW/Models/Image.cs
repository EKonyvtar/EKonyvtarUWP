using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKonyvtarUW.Models
{
    // http://dka.oszk.hu/rss/rsscat.xml 
    // curl "http://mek.oszk.hu/kozoskereso/mobil/dkakepala.php?id=DKA-44753" -H "Cookie: mek_user=fsujgt0puipg3lfbulnf7lqe76; __utmz=235078173.1456747099.1.1.utmccn=(direct)|utmcsr=(direct)|utmcmd=(none); mekepadka=mjsag83avkoca36ps2lirpnh36; mekepadka_mobil=oml03lqutqvlpomcnl4djpoc87; __atuvc=3"%"7C9"%"2C4"%"7C10; __utma=235078173.167594218.1456747099.1457816891.1458103017.9; __utmc=235078173" -H "DNT: 1" -H "Accept-Encoding: gzip, deflate, sdch" -H "Accept-Language: en-US,en;q=0.8,hu;q=0.6" -H "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.57 Safari/537.36" -H "Accept: */*" -H "Referer: http://mek.oszk.hu/kozoskereso/mobil/mek_epa_dka_kereso/build/" -H "X-Requested-With: XMLHttpRequest" -H "Connection: keep-alive" --compressed

    public class Image
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string FullImage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EKonyvtarUW.Models
{
    public static class TextManipulation
    {
        const string HTML_TAG_PATTERN = "<.*?>";
        static Regex escapeFix = new Regex("\\\\[\"]");
        //TODO move mek specific to MekWebService

        public static string StripDoubleEscape(string inputString)
        {
            return escapeFix.Replace(inputString, "\"");
        }
        public static string GetImageUrl(string inputString)
        {
            var imageRegexp = new Regex("(<img\\s+src=\"(\\S*)\"[^<]*>)"); //2

            inputString = StripDoubleEscape(inputString);
            Match match = imageRegexp.Match(inputString);
            if (match.Success)
            {
                return match.Groups[2].Value;
            }
            else
            {
                return null;
            }
        }

        public static string StripHTML(string inputString)
        {
            //TODO: implement proper HTML Text conversion 
            return Regex.Replace
              (inputString.Replace("&quot;", "'"), HTML_TAG_PATTERN, string.Empty);
        }

        public static string StripJson(string json)
        {
            var paddingRegex = new Regex("^\\s*[\"]\\s*(.*)[\"]\\s*$");
            var escapeFix = new Regex("\\\\[\"]");

            Match match = paddingRegex.Match(json);
            if (match.Success)
            {
                var text = escapeFix.Replace(match.Groups[1].Value, "\"");
                return text;
            }
            else
            {
                return null;
            }
        }
    }
}

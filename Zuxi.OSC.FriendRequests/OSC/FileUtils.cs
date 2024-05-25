using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace VRChatFriendRequestTool.OSC
{
    internal class FileUtils
    {

        internal static  string GetAppFolder()
        {
            string text = Path.Combine(GetMainFolder(), "Apps" ,"VRCFRTool");

            bool flag = !Directory.Exists(text);
            if (flag)
            {
                Directory.CreateDirectory(text);
            }






            return text;
        }




        internal static string GetMainFolder()
        {
            string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Zuxi");
            bool flag = !Directory.Exists(text);
            if (flag)
            {
                Directory.CreateDirectory(text);
            }
            return text;
        }
    }
}

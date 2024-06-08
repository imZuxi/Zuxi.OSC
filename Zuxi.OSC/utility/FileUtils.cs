namespace Zuxi.OSC.utility
{
    internal class FileUtils
    {
        public static string GetAppFolder()
        {
            string text = Path.Combine(GetMainFolder(), "Apps", "Zuxi.OSC");

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


using System.IO;

namespace Handler.FileHandler
{
    static public class IOFunc
    {
        public static string GetDirectoryName(string APath)
        {
            return Path.GetDirectoryName(APath);
        }

        public static string GetFilenameWithExt(string APath)
        {
            return Path.GetFileName(APath);
        }
    }
}
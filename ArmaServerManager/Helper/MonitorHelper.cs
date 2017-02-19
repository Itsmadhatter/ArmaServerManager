using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ArmaServerManager.Helper
{
    class MonitorHelper
    {

        public static string GetRpt(String path)
        {
            var folder = new DirectoryInfo(path);
            FileInfo[] fileInfo = folder.GetFiles("*.rpt");
            List<string> fileNames = fileInfo.Select(info => info.Name).ToList();

            if (fileNames.Count == 0) return null;

            fileNames.Sort();

            return fileNames[fileNames.Count - 1];
        }

        public static string ReadFile(String path)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(fs, Encoding.Default);

            var text = sr.ReadToEnd();

            fs.Close();
            sr.Close();

            return text.ToString();
        }
    }
}

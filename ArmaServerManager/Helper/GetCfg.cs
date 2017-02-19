using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArmaServerManager.Properties;

namespace ArmaServerManager.Helper
{
    class GetCfg
    {
        public static Dictionary<string, string> getEntrys()
        {
            Dictionary<string, string> dictionary =
                new Dictionary<string, string>();

                var file = File
                    .ReadAllLines(Settings.Default.ServerCfgPath)
                    .Select(x => x.Split('='))
                    .Where(x => x.Length > 1)
                    .ToDictionary(x => x[0].Trim(), x => x[1]);

            dictionary.Add("hostname", file["hostName"]);

            return dictionary;
        }

    }
}

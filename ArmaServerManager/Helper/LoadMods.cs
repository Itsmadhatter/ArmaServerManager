using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaServerManager.Helper
{
    class LoadMods
    {
        public static List<string> GetMods(string path)
        {
            try
            {
                List<string> name = new List<string>();
                foreach (string s in Directory.GetDirectories(path, "@*", SearchOption.AllDirectories))
                {
                    string p = s.Remove(0, path.Length);
                    name.Add(p.Replace(@"\", ""));
                }
                return name;
            }
            catch
            {
                throw new Exception();
            }
        }

    }
}

using System.Collections.Generic;

namespace ArmaServerManager.Helper
{
    class ParameterBuilder
    {


        public static List<string> createParam()
        {
            Dictionary<string, object> settings = getSettings();
            List<string> param = new List<string>();

            if (!string.IsNullOrEmpty((string)settings["Port"]))
            {
                param.Add("-port=" + settings["Port"]);
            }
            else
            {
                param.Add("-port=2302");
            }
            if (!string.IsNullOrEmpty((string)settings["ConfigCfg"]))
            {
                param.Add("-config=\"" + settings["ConfigCfg"] + "\"");
            }
            if (!string.IsNullOrEmpty((string)settings["BasicCfg"]))
            {
                param.Add("-cfg=\"" + settings["BasicCfg"] + "\"");
            }
            if (!string.IsNullOrEmpty((string)settings["Profile"]))
            {
                param.Add("-profiles=\"" + settings["Profile"] + "\"");
            }
            if (!string.IsNullOrEmpty((string)settings["Mods"])) {
                param.Add("-mod=\"" + settings["Mods"] + "\"");
            }
            if (!string.IsNullOrEmpty((string)settings["ServerMods"]))
            {
                param.Add("-serverMod=\"" + settings["ServerMods"] + "\"");
            }
            if ((bool)settings["HT"] == true) 
            {
                param.Add("-enableHT");
            }
            if ((bool)settings["AutoInit"] == true)
            {
                param.Add("-autoInit");
            }
            if ((bool)settings["NoSound"] == true)
            {
                param.Add("-noSound");
            }
            if ((bool)settings["NoLogs"] == true)
            {
                param.Add("-noLogs");
            }
            if ((bool)settings["filePatching"] == true)
            {
                param.Add("-filePatching");
            }
            if ((bool)settings["NetLog"] == true)
            {
                param.Add("-netlog");
            }
            if ((bool)settings["MissionToMemory"] == true)
            {
                param.Add("-loadMissionToMemory");
            }
            if (!string.IsNullOrEmpty((string)settings["malloc"]))
            {
                param.Add("-malloc=" + settings["malloc"]);
            }
            if (!string.IsNullOrEmpty((string)settings["MaxMem"]))
            {
                param.Add("-maxMem=" + settings["MaxMem"]);
            }

            return param;
        }

        private static Dictionary<string, object> getSettings()
        {
            Dictionary<string, object> settings = new Dictionary<string, object>();
            Properties.Settings props = Properties.Settings.Default;

            settings.Add("HT", props.HT);
            settings.Add("AutoInit", props.AutoInit);
            settings.Add("NoSound", props.NoSound);
            settings.Add("NoLogs", props.NoLogs);
            settings.Add("NetLog", props.NetLog);
            settings.Add("filePatching", props.FilePatching);
            settings.Add("malloc", props.Malloc);
            settings.Add("MaxMem", props.Memory);
            settings.Add("Port", props.Port);
            settings.Add("Mods", props.Mods);
            settings.Add("ConfigCfg", props.ServerCfgPath);
            settings.Add("BasicCfg", props.BasicCfg);
            settings.Add("Profile", props.ProfilePath);
            settings.Add("ServerMods", props.ServerMods);
            settings.Add("MissionToMemory", props.MissionToMemory);

            return settings;
        }
    }
}

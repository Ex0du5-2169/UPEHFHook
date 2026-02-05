using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPEHFHook
{
    public class Config
    {
        public static Config Instance { get; private set; } = new Config();

        public ConfigEntry<bool> AllowAllPerfume { get; private set; }
        //public ConfigEntry<bool> LoadAltAssets { get; private set; }
        internal void Init(ConfigFile conf)
        {
            AllowAllPerfume = conf.Bind<bool>("General", "AllowAllPerfume", false, "Allow perfume to be used on any NPC.");
            //LoadAltAssets = conf.Bind<bool>("General", "LoadAltAssets", false, "Load assets from alternate file");
        }
    }
}

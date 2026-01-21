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


        //public ConfigEntry<bool> ChangeRapeRate { get; private set; }
        public ConfigEntry<bool> AllowAllPerfume { get; private set; }
        internal void Init(ConfigFile conf)
        {
            //ChangeRapeRate = conf.Bind<bool>("General", "ChangeRapeRate", true, "Increase the rate at which rapes occur.");
            AllowAllPerfume = conf.Bind<bool>("General", "AllowAllPerfume", false, "Allow perfume to be used on any NPC.");
        }
    }
}

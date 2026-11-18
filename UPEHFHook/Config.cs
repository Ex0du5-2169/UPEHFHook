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
        public ConfigEntry<bool> SkeletonMagentaKeyEnabled { get; private set; }
        public ConfigEntry<bool> PregnancyMedicineEnabled { get; private set; }
        internal void Init(ConfigFile conf)
        {
            AllowAllPerfume = conf.Bind<bool>("General", "AllowAllPerfume", true, "Allow perfume to be used on any NPC.");
            SkeletonMagentaKeyEnabled = conf.Bind<bool>("General", "SkeletonMagentaKeyEnabled", true, "Fix opaque magenta pixels around new textures");
            PregnancyMedicineEnabled = conf.Bind<bool>("General", "PregnancyMedicineEnabled", true, "Allow pregnancy charm use by more NPCs");
        }
    }
}

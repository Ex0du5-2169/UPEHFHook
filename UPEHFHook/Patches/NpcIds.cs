using System.Collections.Generic;
using UnityEngine;
using UPEHFHook;

namespace UPEHFHook.Patches
{
    // npcID values match indices in NPCManager.npcPrefab (from the NPC_DUMP log).
    // Tags: [Pregnant] = in GetPreg.AllowedIDs; [SpawnChild] = child ID in SpawnChild birth chart.
    public static class Gender
    {
        public const int Female = 0;
        public const int Male = 1;
    }

    public static class NpcID
    {
        public const int Yona = 0; // girl_01
        public const int Man = 1; // man_01
        public const int Riona = 2; // riona_01
        public const int LittleNickey = 3; // little_01
        public const int Reika = 5; // girlfriend_01 [Pregnant]
        public const int Nami = 6; // girlfriend_02 [Pregnant]
        public const int Takumi = 7; // boyfriend_01
        public const int Keigo = 8; // boyfriend_02
        public const int YoungLady = 9; // girl_02
        public const int MaleNative = 10; // gen_01 [SpawnChild]
        public const int BigNative = 11; // gen_02 [SpawnChild]
        public const int SmallNative = 12; // gen_03
        public const int Guard = 13; // gen_04
        public const int NativeBoy = 14; // genboy_01 [SpawnChild]
        public const int FemaleNative = 15; // gengirl_01 [SpawnChild]
        public const int NativeGirl = 16; // gengirl_02 [SpawnChild]
        public const int FemaleLargeNative = 17; // gengirl_03 [SpawnChild]
        public const int OldManNative = 18; // gengg_01
        public const int OldWomanNative = 19; // genbba_01
        public const int Bear = 20; // bear_01
        public const int Spider = 21; // spider_01 [SpawnChild]
        public const int Bat = 22; // bat_01
        public const int Crocodile = 23; // crocodile_01
        public const int Wolf = 24; // wolf_01 [SpawnChild]
        public const int Bigfoot = 25; // bigfoot_01
        public const int Nepenthes = 26; // tamaplant_01 [SpawnChild]
        public const int Mandrake = 27; // mandora_01 [SpawnChild]
        public const int Bee = 28; // bee_01
        public const int Wooper = 29; // wooper_01
        public const int Spider2 = 30; // spider_02 [SpawnChild]
        public const int Angler = 31; // angler_01
        public const int VampireScorpion = 32; // scorpion_01
        public const int Leech = 33; // leech_01
        public const int Eleceel = 34; // eleceel_01
        public const int Werewolf = 35; // werewolf_01 [SpawnChild]
        public const int Neck = 36; // ostrich_01
        public const int Sandworm = 37; // sandworm_01
        public const int NecksBaby = 38; // necksbaby_01
        public const int Skeleton = 39; // skeleton_01
        public const int Ghost = 40; // ghost_01
        public const int Eater = 41; // eater_01
        public const int Mummy = 42; // mummy_01
        public const int Daddy = 43; // mummy_02
        public const int UnderGroundWoman = 44; // genunder_01
        public const int UnderGroundOldWoman = 45; // genunder_02
        public const int UnderGroundMan = 46; // genunder_03
        public const int Rabbit = 50; // rabbit_01
        public const int Deer = 51; // deer_01
        public const int Bird = 52; // bird_01
        public const int Rooster = 53; // chicken_01
        public const int Frog = 54; // frog_01
        public const int Pig = 55; // pig_01
        public const int Butterfly = 56; // butterfly_01
        public const int Crab = 57; // crab_01
        public const int Hen = 58; // chicken_02
        public const int Chicken = 59; // chicken_03
        public const int Mouse = 60; // mouse_01
        public const int Sheep = 61; // sheep_01
        public const int Milky = 62; // milkcow_01
        public const int PinkMilky = 63; // milkcow_02
        public const int Pork = 64; // pig_02
        public const int CorruptTree = 65; // darktree_01
        public const int Ruck = 66; // ruck_01
        public const int DarkTree = 67; // darktree_02
        public const int Ent = 68; // ent_01
        public const int CorruptGoat = 69; // darkgoat_01
        public const int Shark = 70; // shark_01
        public const int Mermaid = 71; // mermaid_01
        public const int ChubbyGuy = 72; // boyfriend_01b
        public const int SlenderYoungLady = 73; // girlfriend_02b [Pregnant] [SpawnChild]
        public const int WhiteTiger = 78; // whitetiger_01
        public const int Turtle = 79; // turtle_01
        public const int SmallGuardian = 80; // ruinsguard_01
        public const int Specter = 81; // ghost_02
        public const int DogGuardian = 82; // ruinsguard_02
        public const int Scrap = 83; // golem_01
        public const int GuardianWarrior = 84; // boss_ruins_01
        public const int TypeA = 85; // failure_01
        public const int TypeB = 86; // failure_02
        public const int TypeC = 87; // failure_03
        public const int Cyborg = 88; // gengirl_01_cyborg
        public const int YoungMan = 89; // man_02
        public const int ElderSisterNative = 90; // genbba_02 [SpawnChild]
        public const int ElderBrotherNative = 91; // gengg_02 [SpawnChild]
        public const int SandCat = 92; // sandcat_01
        public const int Balphant = 93; // buffalo_01
        public const int Snail = 94; // snail_01
        public const int BloodSnail = 95; // snail_02
        public const int Firefly = 96; // firefly_01
        public const int BloodRuck = 97; // ruck_02
        public const int MaidRuck = 98; // ruck_03
        public const int Wingy = 99; // wing_01
        public const int OldGuy = 100; // boss_01
        public const int Spike = 101; // boss_spider_01
        public const int DrEd = 102; // boss_doctor_01
        public const int Planton = 103; // plant_base_01
        public const int BossNative = 104; // boss_gen_01
        public const int Scythe = 105; // boss_jigoku_01
        public const int Cassie = 106; // boss_cyborg_01
        public const int Necks = 107; // boss_bird_01
        public const int Bandana = 108; // boss_hunter_01
        public const int Hat = 109; // boss_hunter_02
        public const int Giant = 110; // gengiant_01 [Pregnant]
        public const int Trader = 111; // trader_01
        public const int Dalman = 112; // boss_daruman_01
        public const int Cassie2 = 113; // cassie_01 [Pregnant]
        public const int Shino = 114; // bakunyu_01 [Pregnant]
        public const int Sally = 115; // boss_prison_01 [Pregnant]
        public const int Merry = 116; // santa_01 [Pregnant]
        public const int Reindeer = 117; // santadeer_01
        public const int Mira = 118; // death_girl_01 [Pregnant]
        public const int EntKing = 120; // boss_entking_01
        public const int EntQueen = 121; // entqueen_01
        public const int EntWarrior = 122; // ent_02
        public const int Chief = 123; // boss_osa_01
        public const int Mako = 124; // boss_osa_02
        public const int Adon = 125; // boss_osa2_01
        public const int Jiera = 126; // boss_osa2_02
        public const int Mamoru = 127; // pastman_01
        public const int Naoto = 128; // pastman_02
        public const int Hannah = 129; // pastwoman_01
        public const int Franken = 139; // boss_franken_01
        public const int LargeNativeGirl = 140; // gengirl_03girl [SpawnChild]
        public const int LargeNativeBoy = 141; // gengirl_03boy [SpawnChild]
        public const int UndergroundGirl = 142; // genunder_01girl [SpawnChild]
        public const int UnderGroundBoy = 143; // genunder_01boy [SpawnChild]
        public const int Daughter = 144; // girl_01girl
        public const int Son = 145; // girl_01boy
        public const int GiantMama = 148; // gengiant_mom_01
        public const int GiantChild = 149; // gengiant_02 [SpawnChild]
        public const int BeeHive = 150; // hive_01
        public const int NecksEgg = 151; // necks_egg_01
        public const int Crayfish = 152; // crayfish_01
        public const int Chaperon = 153; // prisonguard_01
        public const int Snake = 154; // snake_01
        public const int Mogle = 155; // mole_01
        public const int CorruptLizard = 156; // lizard_01
        public const int GorillaBoss01 = 157; // boss_gorilla_01 (Yotan: Gorela)
        public const int GorillaBoss02 = 158; // boss_gorilla_02 (Yotan: Gorola)
        public const int EntRoot = 159; // entroot_01
        public const int Jack = 160; // hallo_jack_01
        public const int HalloweenGhost = 161; // hallo_boss_01
        public const int Kana = 162; // hanggirl_01 [Pregnant]
        public const int Lulu = 163; // hanggirl_02 [Pregnant]
        public const int PetCat02 = 167; // pet_cat_02
        public const int Centipede = 168; // centipede_01
        public const int CaveApe = 169; // ape_01
        public const int PresentBlue = 170; // presentbox_01 [SpawnChild]
        public const int PresentGreen = 171; // presentbox_02
        public const int PresentRed = 172; // presentbox_03 [SpawnChild]
        public const int Mothman = 173; // mothman_01
        public const int ServantOfDeath = 174; // death_01
        public const int Death = 175; // death_02
        public const int LargeYoungMan = 180; // man_04
        public const int UnderGroundYoungMan = 181; // man_05 [SpawnChild]
    }

    [System.Flags]
    public enum NpcTag
    {
        None = 0,
        Pregnant = 1,
        SpawnChild = 2,
    }

    // Runtime metadata for each NpcID, mirroring the comments above. Used for logs and startup checks, not in hot paths.
    // IDs are indices into NPCManager.npcPrefab, so a game update that reorders that array would silently shift them.
    // Verify() compares each prefab name against NPCManager to catch that at startup.
    public static class NpcIdInfo
    {
        private struct Entry
        {
            public readonly string Name;
            public readonly string Prefab;
            public readonly NpcTag Tags;

            public Entry(string name, string prefab, NpcTag tags)
            {
                Name = name;
                Prefab = prefab;
                Tags = tags;
            }
        }

        private static Entry N(string name, string prefab, NpcTag tags = NpcTag.None)
        {
            return new Entry(name, prefab, tags);
        }

        private static readonly Dictionary<int, Entry> Table = new Dictionary<int, Entry>
        {
            { NpcID.Yona, N(nameof(NpcID.Yona), "girl_01") },
            { NpcID.Man, N(nameof(NpcID.Man), "man_01") },
            { NpcID.Riona, N(nameof(NpcID.Riona), "riona_01") },
            { NpcID.LittleNickey, N(nameof(NpcID.LittleNickey), "little_01") },
            { NpcID.Reika, N(nameof(NpcID.Reika), "girlfriend_01", NpcTag.Pregnant) },
            { NpcID.Nami, N(nameof(NpcID.Nami), "girlfriend_02", NpcTag.Pregnant) },
            { NpcID.Takumi, N(nameof(NpcID.Takumi), "boyfriend_01") },
            { NpcID.Keigo, N(nameof(NpcID.Keigo), "boyfriend_02") },
            { NpcID.YoungLady, N(nameof(NpcID.YoungLady), "girl_02") },
            { NpcID.MaleNative, N(nameof(NpcID.MaleNative), "gen_01", NpcTag.SpawnChild) },
            { NpcID.BigNative, N(nameof(NpcID.BigNative), "gen_02", NpcTag.SpawnChild) },
            { NpcID.SmallNative, N(nameof(NpcID.SmallNative), "gen_03") },
            { NpcID.Guard, N(nameof(NpcID.Guard), "gen_04") },
            { NpcID.NativeBoy, N(nameof(NpcID.NativeBoy), "genboy_01", NpcTag.SpawnChild) },
            { NpcID.FemaleNative, N(nameof(NpcID.FemaleNative), "gengirl_01", NpcTag.SpawnChild) },
            { NpcID.NativeGirl, N(nameof(NpcID.NativeGirl), "gengirl_02", NpcTag.SpawnChild) },
            { NpcID.FemaleLargeNative, N(nameof(NpcID.FemaleLargeNative), "gengirl_03", NpcTag.SpawnChild) },
            { NpcID.OldManNative, N(nameof(NpcID.OldManNative), "gengg_01") },
            { NpcID.OldWomanNative, N(nameof(NpcID.OldWomanNative), "genbba_01") },
            { NpcID.Bear, N(nameof(NpcID.Bear), "bear_01") },
            { NpcID.Spider, N(nameof(NpcID.Spider), "spider_01", NpcTag.SpawnChild) },
            { NpcID.Bat, N(nameof(NpcID.Bat), "bat_01") },
            { NpcID.Crocodile, N(nameof(NpcID.Crocodile), "crocodile_01") },
            { NpcID.Wolf, N(nameof(NpcID.Wolf), "wolf_01", NpcTag.SpawnChild) },
            { NpcID.Bigfoot, N(nameof(NpcID.Bigfoot), "bigfoot_01") },
            { NpcID.Nepenthes, N(nameof(NpcID.Nepenthes), "tamaplant_01", NpcTag.SpawnChild) },
            { NpcID.Mandrake, N(nameof(NpcID.Mandrake), "mandora_01", NpcTag.SpawnChild) },
            { NpcID.Bee, N(nameof(NpcID.Bee), "bee_01") },
            { NpcID.Wooper, N(nameof(NpcID.Wooper), "wooper_01") },
            { NpcID.Spider2, N(nameof(NpcID.Spider2), "spider_02", NpcTag.SpawnChild) },
            { NpcID.Angler, N(nameof(NpcID.Angler), "angler_01") },
            { NpcID.VampireScorpion, N(nameof(NpcID.VampireScorpion), "scorpion_01") },
            { NpcID.Leech, N(nameof(NpcID.Leech), "leech_01") },
            { NpcID.Eleceel, N(nameof(NpcID.Eleceel), "eleceel_01") },
            { NpcID.Werewolf, N(nameof(NpcID.Werewolf), "werewolf_01", NpcTag.SpawnChild) },
            { NpcID.Neck, N(nameof(NpcID.Neck), "ostrich_01") },
            { NpcID.Sandworm, N(nameof(NpcID.Sandworm), "sandworm_01") },
            { NpcID.NecksBaby, N(nameof(NpcID.NecksBaby), "necksbaby_01") },
            { NpcID.Skeleton, N(nameof(NpcID.Skeleton), "skeleton_01") },
            { NpcID.Ghost, N(nameof(NpcID.Ghost), "ghost_01") },
            { NpcID.Eater, N(nameof(NpcID.Eater), "eater_01") },
            { NpcID.Mummy, N(nameof(NpcID.Mummy), "mummy_01") },
            { NpcID.Daddy, N(nameof(NpcID.Daddy), "mummy_02") },
            { NpcID.UnderGroundWoman, N(nameof(NpcID.UnderGroundWoman), "genunder_01") },
            { NpcID.UnderGroundOldWoman, N(nameof(NpcID.UnderGroundOldWoman), "genunder_02") },
            { NpcID.UnderGroundMan, N(nameof(NpcID.UnderGroundMan), "genunder_03") },
            { NpcID.Rabbit, N(nameof(NpcID.Rabbit), "rabbit_01") },
            { NpcID.Deer, N(nameof(NpcID.Deer), "deer_01") },
            { NpcID.Bird, N(nameof(NpcID.Bird), "bird_01") },
            { NpcID.Rooster, N(nameof(NpcID.Rooster), "chicken_01") },
            { NpcID.Frog, N(nameof(NpcID.Frog), "frog_01") },
            { NpcID.Pig, N(nameof(NpcID.Pig), "pig_01") },
            { NpcID.Butterfly, N(nameof(NpcID.Butterfly), "butterfly_01") },
            { NpcID.Crab, N(nameof(NpcID.Crab), "crab_01") },
            { NpcID.Hen, N(nameof(NpcID.Hen), "chicken_02") },
            { NpcID.Chicken, N(nameof(NpcID.Chicken), "chicken_03") },
            { NpcID.Mouse, N(nameof(NpcID.Mouse), "mouse_01") },
            { NpcID.Sheep, N(nameof(NpcID.Sheep), "sheep_01") },
            { NpcID.Milky, N(nameof(NpcID.Milky), "milkcow_01") },
            { NpcID.PinkMilky, N(nameof(NpcID.PinkMilky), "milkcow_02") },
            { NpcID.Pork, N(nameof(NpcID.Pork), "pig_02") },
            { NpcID.CorruptTree, N(nameof(NpcID.CorruptTree), "darktree_01") },
            { NpcID.Ruck, N(nameof(NpcID.Ruck), "ruck_01") },
            { NpcID.DarkTree, N(nameof(NpcID.DarkTree), "darktree_02") },
            { NpcID.Ent, N(nameof(NpcID.Ent), "ent_01") },
            { NpcID.CorruptGoat, N(nameof(NpcID.CorruptGoat), "darkgoat_01") },
            { NpcID.Shark, N(nameof(NpcID.Shark), "shark_01") },
            { NpcID.Mermaid, N(nameof(NpcID.Mermaid), "mermaid_01") },
            { NpcID.ChubbyGuy, N(nameof(NpcID.ChubbyGuy), "boyfriend_01b") },
            { NpcID.SlenderYoungLady, N(nameof(NpcID.SlenderYoungLady), "girlfriend_02b", NpcTag.Pregnant | NpcTag.SpawnChild) },
            { NpcID.WhiteTiger, N(nameof(NpcID.WhiteTiger), "whitetiger_01") },
            { NpcID.Turtle, N(nameof(NpcID.Turtle), "turtle_01") },
            { NpcID.SmallGuardian, N(nameof(NpcID.SmallGuardian), "ruinsguard_01") },
            { NpcID.Specter, N(nameof(NpcID.Specter), "ghost_02") },
            { NpcID.DogGuardian, N(nameof(NpcID.DogGuardian), "ruinsguard_02") },
            { NpcID.Scrap, N(nameof(NpcID.Scrap), "golem_01") },
            { NpcID.GuardianWarrior, N(nameof(NpcID.GuardianWarrior), "boss_ruins_01") },
            { NpcID.TypeA, N(nameof(NpcID.TypeA), "failure_01") },
            { NpcID.TypeB, N(nameof(NpcID.TypeB), "failure_02") },
            { NpcID.TypeC, N(nameof(NpcID.TypeC), "failure_03") },
            { NpcID.Cyborg, N(nameof(NpcID.Cyborg), "gengirl_01_cyborg") },
            { NpcID.YoungMan, N(nameof(NpcID.YoungMan), "man_02") },
            { NpcID.ElderSisterNative, N(nameof(NpcID.ElderSisterNative), "genbba_02", NpcTag.SpawnChild) },
            { NpcID.ElderBrotherNative, N(nameof(NpcID.ElderBrotherNative), "gengg_02", NpcTag.SpawnChild) },
            { NpcID.SandCat, N(nameof(NpcID.SandCat), "sandcat_01") },
            { NpcID.Balphant, N(nameof(NpcID.Balphant), "buffalo_01") },
            { NpcID.Snail, N(nameof(NpcID.Snail), "snail_01") },
            { NpcID.BloodSnail, N(nameof(NpcID.BloodSnail), "snail_02") },
            { NpcID.Firefly, N(nameof(NpcID.Firefly), "firefly_01") },
            { NpcID.BloodRuck, N(nameof(NpcID.BloodRuck), "ruck_02") },
            { NpcID.MaidRuck, N(nameof(NpcID.MaidRuck), "ruck_03") },
            { NpcID.Wingy, N(nameof(NpcID.Wingy), "wing_01") },
            { NpcID.OldGuy, N(nameof(NpcID.OldGuy), "boss_01") },
            { NpcID.Spike, N(nameof(NpcID.Spike), "boss_spider_01") },
            { NpcID.DrEd, N(nameof(NpcID.DrEd), "boss_doctor_01") },
            { NpcID.Planton, N(nameof(NpcID.Planton), "plant_base_01") },
            { NpcID.BossNative, N(nameof(NpcID.BossNative), "boss_gen_01") },
            { NpcID.Scythe, N(nameof(NpcID.Scythe), "boss_jigoku_01") },
            { NpcID.Cassie, N(nameof(NpcID.Cassie), "boss_cyborg_01") },
            { NpcID.Necks, N(nameof(NpcID.Necks), "boss_bird_01") },
            { NpcID.Bandana, N(nameof(NpcID.Bandana), "boss_hunter_01") },
            { NpcID.Hat, N(nameof(NpcID.Hat), "boss_hunter_02") },
            { NpcID.Giant, N(nameof(NpcID.Giant), "gengiant_01", NpcTag.Pregnant) },
            { NpcID.Trader, N(nameof(NpcID.Trader), "trader_01") },
            { NpcID.Dalman, N(nameof(NpcID.Dalman), "boss_daruman_01") },
            { NpcID.Cassie2, N(nameof(NpcID.Cassie2), "cassie_01", NpcTag.Pregnant) },
            { NpcID.Shino, N(nameof(NpcID.Shino), "bakunyu_01", NpcTag.Pregnant) },
            { NpcID.Sally, N(nameof(NpcID.Sally), "boss_prison_01", NpcTag.Pregnant) },
            { NpcID.Merry, N(nameof(NpcID.Merry), "santa_01", NpcTag.Pregnant) },
            { NpcID.Reindeer, N(nameof(NpcID.Reindeer), "santadeer_01") },
            { NpcID.Mira, N(nameof(NpcID.Mira), "death_girl_01", NpcTag.Pregnant) },
            { NpcID.EntKing, N(nameof(NpcID.EntKing), "boss_entking_01") },
            { NpcID.EntQueen, N(nameof(NpcID.EntQueen), "entqueen_01") },
            { NpcID.EntWarrior, N(nameof(NpcID.EntWarrior), "ent_02") },
            { NpcID.Chief, N(nameof(NpcID.Chief), "boss_osa_01") },
            { NpcID.Mako, N(nameof(NpcID.Mako), "boss_osa_02") },
            { NpcID.Adon, N(nameof(NpcID.Adon), "boss_osa2_01") },
            { NpcID.Jiera, N(nameof(NpcID.Jiera), "boss_osa2_02") },
            { NpcID.Mamoru, N(nameof(NpcID.Mamoru), "pastman_01") },
            { NpcID.Naoto, N(nameof(NpcID.Naoto), "pastman_02") },
            { NpcID.Hannah, N(nameof(NpcID.Hannah), "pastwoman_01") },
            { NpcID.Franken, N(nameof(NpcID.Franken), "boss_franken_01") },
            { NpcID.LargeNativeGirl, N(nameof(NpcID.LargeNativeGirl), "gengirl_03girl", NpcTag.SpawnChild) },
            { NpcID.LargeNativeBoy, N(nameof(NpcID.LargeNativeBoy), "gengirl_03boy", NpcTag.SpawnChild) },
            { NpcID.UndergroundGirl, N(nameof(NpcID.UndergroundGirl), "genunder_01girl", NpcTag.SpawnChild) },
            { NpcID.UnderGroundBoy, N(nameof(NpcID.UnderGroundBoy), "genunder_01boy", NpcTag.SpawnChild) },
            { NpcID.Daughter, N(nameof(NpcID.Daughter), "girl_01girl") },
            { NpcID.Son, N(nameof(NpcID.Son), "girl_01boy") },
            { NpcID.GiantMama, N(nameof(NpcID.GiantMama), "gengiant_mom_01") },
            { NpcID.GiantChild, N(nameof(NpcID.GiantChild), "gengiant_02", NpcTag.SpawnChild) },
            { NpcID.BeeHive, N(nameof(NpcID.BeeHive), "hive_01") },
            { NpcID.NecksEgg, N(nameof(NpcID.NecksEgg), "necks_egg_01") },
            { NpcID.Crayfish, N(nameof(NpcID.Crayfish), "crayfish_01") },
            { NpcID.Chaperon, N(nameof(NpcID.Chaperon), "prisonguard_01") },
            { NpcID.Snake, N(nameof(NpcID.Snake), "snake_01") },
            { NpcID.Mogle, N(nameof(NpcID.Mogle), "mole_01") },
            { NpcID.CorruptLizard, N(nameof(NpcID.CorruptLizard), "lizard_01") },
            { NpcID.GorillaBoss01, N(nameof(NpcID.GorillaBoss01), "boss_gorilla_01") },
            { NpcID.GorillaBoss02, N(nameof(NpcID.GorillaBoss02), "boss_gorilla_02") },
            { NpcID.EntRoot, N(nameof(NpcID.EntRoot), "entroot_01") },
            { NpcID.Jack, N(nameof(NpcID.Jack), "hallo_jack_01") },
            { NpcID.HalloweenGhost, N(nameof(NpcID.HalloweenGhost), "hallo_boss_01") },
            { NpcID.Kana, N(nameof(NpcID.Kana), "hanggirl_01", NpcTag.Pregnant) },
            { NpcID.Lulu, N(nameof(NpcID.Lulu), "hanggirl_02", NpcTag.Pregnant) },
            { NpcID.PetCat02, N(nameof(NpcID.PetCat02), "pet_cat_02") },
            { NpcID.Centipede, N(nameof(NpcID.Centipede), "centipede_01") },
            { NpcID.CaveApe, N(nameof(NpcID.CaveApe), "ape_01") },
            { NpcID.PresentBlue, N(nameof(NpcID.PresentBlue), "presentbox_01", NpcTag.SpawnChild) },
            { NpcID.PresentGreen, N(nameof(NpcID.PresentGreen), "presentbox_02") },
            { NpcID.PresentRed, N(nameof(NpcID.PresentRed), "presentbox_03", NpcTag.SpawnChild) },
            { NpcID.Mothman, N(nameof(NpcID.Mothman), "mothman_01") },
            { NpcID.ServantOfDeath, N(nameof(NpcID.ServantOfDeath), "death_01") },
            { NpcID.Death, N(nameof(NpcID.Death), "death_02") },
            { NpcID.LargeYoungMan, N(nameof(NpcID.LargeYoungMan), "man_04") },
            { NpcID.UnderGroundYoungMan, N(nameof(NpcID.UnderGroundYoungMan), "man_05", NpcTag.SpawnChild) },
        };

        private static bool verified;

        // Returns e.g. "Reika (5) [Pregnant]". Used by logs.
        public static string Describe(int id)
        {
            if (!Table.TryGetValue(id, out Entry entry))
                return "Unknown (" + id + ")";
            return entry.Name + " (" + id + ")" + (entry.Tags == NpcTag.None ? "" : " [" + entry.Tags + "]");
        }

        public static bool CanGetPregnant(int id)
        {
            return Table.TryGetValue(id, out Entry entry) && (entry.Tags & NpcTag.Pregnant) != 0;
        }

        public static bool IsSpawnChild(int id)
        {
            return Table.TryGetValue(id, out Entry entry) && (entry.Tags & NpcTag.SpawnChild) != 0;
        }

        internal static void Verify(GameObject[] npcPrefabs)
        {
            if (verified) return;
            verified = true;

            int matches = 0;
            foreach (KeyValuePair<int, Entry> pair in Table)
            {
                Entry entry = pair.Value;
                string actual = pair.Key < npcPrefabs.Length && npcPrefabs[pair.Key] != null
                    ? npcPrefabs[pair.Key].name.Replace("_prefab", "")
                    : null;
                if (actual == entry.Prefab)
                {
                    matches++;
                    continue;
                }

                // Lookup by name, so the log shows where the expected NPC actually is now.
                int foundAt = -1;
                for (int i = 0; i < npcPrefabs.Length && foundAt < 0; i++)
                {
                    if (npcPrefabs[i] != null && npcPrefabs[i].name.Replace("_prefab", "") == entry.Prefab)
                        foundAt = i;
                }
                string location = foundAt >= 0 ? "'" + entry.Prefab + "' is now at ID " + foundAt : "'" + entry.Prefab + "' was not found in NPCManager";
                UPEHFBase.Log.LogWarning("NPC ID " + Describe(pair.Key) + " expected '" + entry.Prefab + "' but found '" + (actual ?? "none") + "'; " + location + ". Hard-coded IDs may affect the wrong NPC.");
            }
            UPEHFBase.Log.LogInfo("NPC ID check: " + matches + "/" + Table.Count + " IDs match their expected prefab names.");
        }
    }
}


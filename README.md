# Mad Island Universal Pregnancy Expansion:
Allows more NPC chracters to get pregnant and give birth.

# What it does not do:
- The mod DOES NOT add any new animations (the delivery animations are just duplicates of existing character anims). There are no new sex scenes, there are no new delivery scenes, yet. My code simply duplicates animations they do have under a different name to trick the game.
- It DOES NOT add any new NPCs.
- Custom NPCs are NOT supported (well, not without you doing the extra art and animations yourself).

# Current Version:
1.0 Candidate - Test build

Changes:
- New Swapping script by Shurutsue
- Further simplified pregnancy by letting the game do the work
- Removed loads of old code and the old swapping scripts
- Optimisations for everyone

# Installation:
Place the included 'bepinex' folder in your Mad Island folder (install bepinex first, see below) and overwrite files if asked.

If you do not know how to install bepinex then I suggest looking at this guide - https://docs.bepinex.dev/articles/user_guide/installation/index.html

The current stable Bepinex build - https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.2

- Note - Yotan's HFramework and YotanModCore are REQUIRED for my mod to work.

# Warnings:
- Creating a brand new save is ALWAYS advisable when modding games. Please, for our combined sanity, make a new game and save separately when playing with mods.
- CombinedMeshDemosaic causes issues in the latest versions of the game. It could feasibly also cause conflicts with my mod. It is highly recommended to use the none.bat method and install Yotan's Core mod and Unofficial patches instead.

# Girls available for impregnation in the mod:
- Shino
- Sally
- Cassie
- Giant
- Large female native (removed due to official support)
- Elder sister native (removed as official support is expected soon)
- Reika
- Nami
- Merry

# For Mod-makers:
To add custom girls to UPE:
- Ensure you have added a belly asset and created suitable delivery animations (or duplicate and rename existing animations like I did)
- Make sure you have exported the .json, .atlas.txt and .png and changed the version number in the .json to read '3.8.99' (if using version 3.8.75)
- Copy those 3 files and place them in Bepinex/config/skeletonReplacers/NPC_Name (see the existing folder layout, you'll understand)
- All files and their folder MUST use the exact names the game uses. For example, Sally is boss_prison_01 (they need to match the prefabs after all)

# Planned updates:
- Custom birth chart (births calculated based on the NPCs rather than the current HFramework defaults).
- Cumflation effects + cum leaking?
- Hybrid Children NPCs?

# Join our Mad Island modding community:
For memes, updates or to get involved as a developer, animator or artist. Over 18's only.
https://discord.gg/fgSTzEHB8v

# Credits and Special Thanks:
Ex0du5_2169 - Me - Buy me a drink, if you want to - https://ko-fi.com/ex0du5_2169

nm088 - for the huge amount of help and advice.

Yotan - https://github.com/yotan-dev/mad-island-mods/releases - For his advice and his efforts in making YotanModCore & HFramework.

Kittii - for creating the new preg bellies (and boobs in some cases).

Shurutsue - For the new swapping script and loose file skeleton data loading.

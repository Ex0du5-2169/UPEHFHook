# Mad Island Universal Pregnancy Expansion:
Allows more NPC chracters to get pregnant and give birth.

# What it does not do:
The mod DOES NOT add any new animations. There are no new sex scenes, there are no new delivery scenes, yet. My code simply duplicates animations they do have under a different name to trick the game.
It DOES NOT add any new NPCs.

# Current Version:
0.9.9.2 - Beta - testing release.

# Installation:
- Place 'UPEHFHook.dll' into your Bepinex/plugins folder and put 'miassets' into Mad Island_Data/StreamingAssets/AssetBundles (if the latter doesn't exist, make it).
- If you downloaded the vortex pack of the mod and have already installed bepinex, simply unzip it to the root of your game folder like so ![image](https://github.com/user-attachments/assets/9f47215a-f792-4ee2-a2a3-cbaa95d12684)
- You can also mount the vortex pack as a mod in vortex. I believe an extension for Mad Island exists.


If you do not know how to install bepinex then I suggest looking at this guide - https://docs.bepinex.dev/articles/user_guide/installation/index.html

The current stable Bepinex build - https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.2

# Warnings:
- Creating a brand new save is ALWAYS advisable when modding games. Please, for our combined sanity, make a new game and save separately when playing with mods.
- The mod is currently NOT COMPATIBLE with HFramework. Please do not attempt to use both mods together. I will eventually release a patch, wait until then.
- Currently the mod will change the look of the NPCs that it affects. We're working on a fix. For now, just remember how they looked and use the 'operating table' workstation to change them back.
- At present, the mod will swap the skeletons of any compatible characters when pregnancy is detected. Due to the way the game is coded we had to be very round-about with the way we approached it. The mod gains more info on the skeletons that need swapping as those characters have sex. Let the scenes play out to max (pink bar for MC scenes).
- CombinedMeshDemosaic causes issues in the latest versions of the game. It could feasibly also cause conflicts with my mod. It is highly recommended to use the none.bat method and install Yotan's Core mod and Unofficial patches instead.

# Girls available for impregnation in the mod:
- Shino
- Sally
- Cassie
- Giant
- Large female native
- Elder sister native
- Reika
- Nami (she's not in the game yet, but she will be compatible as soon as she is introduced)
- Merry

# Planned updates:
- Store NPC skins/other aspects in a dictionary and load them onto the new skeleton.
- Add better preg belly textures - work in progress.
- Add compatibility for Yotan's HFramework.
- Cumflation effects + cum leaking?
- Hybrid Children NPCs?

# Debug keys, please don't break stuff by abusing these:
- F4 - switches any scanned and compatible NPCs to the new skin.
- F5 - switches all tracked skeletons back.

# Join our Mad Island modding community:
For memes, updates or to get involved as a developer, animator or artist. Over 18's only.
https://discord.gg/fgSTzEHB8v

# Credits:
Ex0du5_2169 - Me - Buy me a cuppa, if you want to - https://ko-fi.com/ex0du5_2169

# Special Thanks:
nm088 on the Mad Island modding discord for the huge amount of help and advice, plus his skeleton data swapping scripts that are crucial to this mod.

Yotan - https://github.com/yotan-dev/mad-island-mods/releases - For his advice, and his efforts in making HFramework, that this mod was originally intended to work with (soon).

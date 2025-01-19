[![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/banner2-direct.svg)](https://stand-with-ukraine.pp.ua)

![Zara Survival Engine](https://github.com/user-attachments/assets/095d4961-9e67-453a-898b-117fcdd86a73)

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/vagrod/zara/blob/master/LICENSE)
[![made-with-csharp](https://img.shields.io/badge/Made%20with-C%23%207.2-%23239120.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7)
[![Unity-tested](https://img.shields.io/badge/Tested%20with-Unity%20-%23000000.svg?&logo=unity)](https://unity.com)
[![cryengine-tested](https://img.shields.io/badge/Works%20with-CRYENGINE-blue?&logo=cryengine)](https://cryengine.com)
[![godot-tested](https://img.shields.io/badge/Works%20with-Godot-darkgreen?&logo=godot)](https://godotengine.org)
[![flax-tested](https://img.shields.io/badge/Works%20with-Flax-blue?&logo=flax)](https://flaxengine.com)

## Platforms

Zara is written in independent C#. Demos available for:
- ![Unity](https://github.com/user-attachments/assets/7c0c0e81-de55-4f8a-a125-961a95dcc8d3) Unity
- ![CRYENGINE](https://github.com/user-attachments/assets/89b55fb5-7b91-4cbf-a371-14c1e8138c2d) CRYENGINE ([more on CRYENGINE example](https://github.com/vagrod/zara/wiki/CryEngine-Demo-Notes))
- ![Godot](https://github.com/user-attachments/assets/5e93432b-2883-4f46-ace4-9f566dc75d6f) Godot ([more on Godot example](https://github.com/vagrod/zara/wiki/Godot-Demo-Notes))
- ![Flax](https://github.com/user-attachments/assets/ae163bec-0ebd-40e8-84f9-7d6858c0c28e) Flax ([more on Flax example](https://github.com/vagrod/zara/wiki/Flax-Demo-Notes))
- ![.Net](https://github.com/user-attachments/assets/1b4336b5-98e7-445d-9185-2922810a0d04).Net ([more on .Net example](https://github.com/vagrod/zara/wiki/.NetCore-Demo-Notes))

There is also full-featured Zara rewritten from scratch in [Rust](https://www.rust-lang.org): it's [here](https://github.com/vagrod/zara-rust) if you're interested :)

## Download

If you want to download latest Zara release without any demos and optional stuff, visit [Releases](https://github.com/vagrod/zara/releases) section.\
It is also available in the Unity Asset format: [here](https://assetstore.unity.com/packages/templates/systems/zara-survival-engine-182386#description).

The code in repo represents release `v1.06` with no changes on top.

All you need to do to include Zara into your project - is to grab `Zara` folder with its `cs` sources, and you're done!

## Description

Zara will be useful for you if you want your game to have weather-aware **health control** with ton of intertwined parameters, **sleeping**, **fatigue**, **diseases** (flu, food poisoning, venom poisoning, angina and so on), **injuries** (cuts, fractures), **food spoiling**, **water disinfecting**, **inventory**, **crafting**, **clothes** with different water/cold resistance levels and more. On a surface, it is really easy to [set up](https://github.com/vagrod/zara/wiki/Getting-Started) and use.

Code is open, so you can customize everything for your particular game needs.

Saving/Loading of the engine state is fully supported: more on it [here](https://github.com/vagrod/zara/wiki/How-To-Save-and-Load-Engine-State) and [here](https://github.com/vagrod/zara/wiki/Add-Stuff-to-State-Saving-and-Loading).
***
Zara is a complete package with full implementation of the inventory system with crafting. Everything, including built-in inventory, will be saved and loaded on your demand, you don't have to worry about this low-level stuff, and you can dedicate more time to the actual game making.
***
![Zara Demo app screen](https://github.com/user-attachments/assets/ce72d689-af6c-4b99-9424-3e25a2dcefbc)

See [wiki](https://github.com/vagrod/zara/wiki) for the detailed technical info.

<details>
<summary>
Click to show details
</summary>

Zara includes:
+ [Health Engine](https://github.com/vagrod/zara/wiki/Health-Controller) that controls dozen of parameters
+ [Disease Engine](https://github.com/vagrod/zara/wiki/Diseases) with treatment
+ [Hypothermia](https://github.com/vagrod/zara/wiki/How-Hypothermia-Works)
+ [Hyperthermia](https://github.com/vagrod/zara/wiki/How-Hyperthermia-Works)
+ [Disease Monitors](https://github.com/vagrod/zara/wiki/Disease-Monitors)
+ [Medical Agents](https://github.com/vagrod/zara/wiki/Medical-Agents)
+ [Injury Engine](https://github.com/vagrod/zara/wiki/Injuries) with treatment
+ [Pills](https://github.com/vagrod/zara/wiki/Consumables-(pills)-Treatment) and [injections](https://github.com/vagrod/zara/wiki/Appliances-(injections)-Treatment)
+ [Bandages, splints, etc.](https://github.com/vagrod/zara/wiki/How-To-Put-Bandages-and-Stuff)
+ [Inventory Engine](https://github.com/vagrod/zara/wiki/Inventory-Controller) with [crafting](https://github.com/vagrod/zara/wiki/How-to-Combine-Items)
+ [Clothes and Clothes Groups](https://github.com/vagrod/zara/wiki/Clothes)
+ Built-in [Sleeping](https://github.com/vagrod/zara/wiki/How-To-Sleep) mechanics

 and more.

 Supports the following causes of death:
 ~~~
 Drowning
 From Disease
 Hypothermia
 Hyperthermia
 From Bad Vitals (too low ot too high blood pressure, too low or high body temp. and so on)
 Medicine Overdose
 Heart Failure (can be caused by eating incompatible medicine)
 Blood Loss (injuries)
 Dehydration
 Starvation
 ~~~

Zara is talking to your outside game world via [events](https://github.com/vagrod/zara/wiki/Handling-Zara-Events) (you can subscribe and listen to them from anywhere).

Zara will not eat your game performance: it is re-evaluating health state and all needed internals only once a second (you can customize this to be any number).

Zara is aware of the [game surroundings](https://github.com/vagrod/zara/wiki/Setting-Up-Weather-Description) if provided (air temperature, wind speed, rain intensity, time of day), and will adjust its parameters dynamically (in a hot day water will drain faster; in rainy cold day it is more likely to get flu, and so on.)
</details>

## Links and Contacts

![YouTube](https://github.com/user-attachments/assets/fe24e4c6-a691-4e99-be4e-cb27c7511e5c) Video Tutorial (YouTube): [link to the playlist](https://youtube.com/playlist?list=PLfoHacQlYzziLtmwfMQiQkCwYztZ-FNMa)

If you have any technical questions about Zara, contact me: zara-survival (at) imw (dot) su.\
[Dedicated Unity thread](https://forum.unity.com/threads/zara-survival-engine-c.989233/)\
[Dedicated Godot thread](https://godotforums.org/discussion/25104/zara-survival-engine-c)

Project is supported by [JetBrains Opensource Licensing Program](https://www.jetbrains.com/opensource/?from=ZaraSurvivalEngine)\
![JetBrains](https://github.com/user-attachments/assets/df607c9d-3ce5-454d-bbb7-e0855a0ca466)


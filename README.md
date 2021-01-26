![Zara Survival Engine](http://imw.su/zaralogo_gh.png)

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/vagrod/zara/blob/master/LICENSE)
[![made-with-csharp](https://img.shields.io/badge/Made%20with-C%23%207.2-%23239120.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7)
[![Unity-tested](https://img.shields.io/badge/Tested%20with-Unity%20-%23000000.svg?&logo=unity)](https://unity.com)
[![cryengine-tested](https://img.shields.io/badge/Works%20with-CRYENGINE-blue?&logo=cryengine)](https://cryengine.com)
[![godot-tested](https://img.shields.io/badge/Works%20with-Godot-darkgreen?&logo=godot)](https://godotengine.org)
[![flax-tested](https://img.shields.io/badge/Works%20with-Flax-blue?&logo=flax)](https://flaxengine.com)

## Platforms

Zara is written in independent C#. Demos available for:
- ![Unity](http://imw.su/logo-unity.png) Unity
- ![CRYENGINE](http://imw.su/logo-cry-.png) CRYENGINE ([more on CRYENGINE example](https://github.com/vagrod/zara/wiki/CryEngine-Demo-Notes))
- ![Godot](http://imw.su/logo-godot.png) Godot ([more on Godot example](https://github.com/vagrod/zara/wiki/Godot-Demo-Notes))
- ![Flax](http://imw.su/logo-flax-.png) Flax ([more on Flax example](https://github.com/vagrod/zara/wiki/Flax-Demo-Notes))
- ![netcore](http://imw.su/logo-net-.png) .Net Core 3.1 ([more on .Net Core example](https://github.com/vagrod/zara/wiki/.NetCore-Demo-Notes))

## Download

If you want to download latest Zara release without any demos and optional stuff, visit [Releases](https://github.com/vagrod/zara/releases) section.\
It is also available in the Unity Asset format: [here](https://assetstore.unity.com/packages/templates/systems/zara-survival-engine-182386#description).

If you want to use latest bleeding-edge Zara, grab this folder from the repo:\
`/master/zara-demo-unity/Assets/Zara`\
[What are the differences between this and latest release?](https://github.com/vagrod/zara/wiki/Release-Notes)

All you need to do to include Zara into your project - is to grab `Zara` folder with its `cs` sources, and you're done!

## Description

Zara will be useful for you if you want your game to have weather-aware **health control** with ton of intertwined parameters, **sleeping**, **fatigue**, **diseases** (flu, food poisoning, venom poisoning, angina and so on), **injuries** (cuts, fractures), **food spoiling**, **water disinfecting**, **inventory**, **crafting**, **clothes** with different water/cold resistance levels and more. On a surface, it is really easy to [set up](https://github.com/vagrod/zara/wiki/Getting-Started) and use.

Code is open, so you can customize everything for your particular game needs.

Saving/Loading of the engine state is fully supported: more on it [here](https://github.com/vagrod/zara/wiki/How-To-Save-and-Load-Engine-State) and [here](https://github.com/vagrod/zara/wiki/Add-Stuff-to-State-Saving-and-Loading).
***
Zara is a complete package with full implementation of the inventory system with crafting. Everything, including built-in inventory, will be saved and loaded on your demand, you don't have to worry about this low-level stuff, and you can dedicate more time to the actual game making.
***
![Zara Demo app screen](http://imw.su/ZaraDemoScreen_06.png)

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

If you have any technical questions about Zara, contact me: zara-survival (at) imw (dot) su.\
[Dedicated Unity thread](https://forum.unity.com/threads/zara-survival-engine-c.989233/)\
[Dedicated Godot thread](https://godotforums.org/discussion/25104/zara-survival-engine-c)

Project is supported by [JetBrains Opensource Licensing Program](https://www.jetbrains.com/opensource/?from=ZaraSurvivalEngine)\
![JetBrains](http://imw.su/jetbrains-variant-3.png)

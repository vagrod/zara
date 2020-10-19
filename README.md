# Zara Survival Engine
Zara is written in C# (3D-engine-agnostic code: no specific 3D-engine references).

Zara will be useful for you if you want your game to have weather-aware health control with ton of intertwined parameters, sleeping, fatigue, diseases (flu, food poisoning, venom poisoning, angina and so on), injuries (cuts, fractures), food spoiling, water disinfecting, inventory, crafting, clothes with different water/cold resistance levels and more. On a surface, it is really easy to use. [Basic setup](https://github.com/vagrod/zara/wiki/Getting-Started) is very easy – and you have everything at your disposal). 

Code is open, so you can customize everything for your particular game needs.

Saving/Loading of the engine state is fully supported: more on it [here](https://github.com/vagrod/zara/wiki/How-To-Save-and-Load-Engine-State) and [here](https://github.com/vagrod/zara/wiki/Add-Stuff-to-State-Saving-and-Loading)

![Zara Demo app screen](http://imw.su/ZaraDemoScreen_06.png)

See [wiki](https://github.com/vagrod/zara/wiki) for the detailed technical info.

Zara includes:
+ [Health Engine](https://github.com/vagrod/zara/wiki/Health-Controller) that controls dozen of parameters
+ [Disease Engine](https://github.com/vagrod/zara/wiki/Diseases) with treatment
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
 From Bad Vitals (too low ot too high blood pressure, too low or high body temp. and so on)
 Medicine Overdose
 Heart Failure (can be caused by eating incompatible medicine)
 Blood Loss (injuries)
 Dehydration
 Starvation 
 ~~~
 
Zara is talking to your outside game world via events (you can subscribe and listen to them from anywhere).

Zara will not eat your game performance: it is re-evaluating health state and all needed internals only once a second (you can customize this to be any number).

Zara is aware of the game surroundings if provided (air temperature, wind speed, rain intensity, time of day), and will adjust its parameters dynamically (in a hot day water will drain faster; in rainy cold day it is more likely to get flu, and so on.)

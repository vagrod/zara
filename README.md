# Zara Survival Engine
Zara is written in C# (3D-engine-agnostic code: no specific 3D-engine references).

Zara will be useful for you if you want your game to have weather-aware health control with ton of intertwined parameters, sleeping, fatigue, diseases (flu, food poisoning, venom poisoning, angina and so on), injuries (cuts, fractures), food spoiling, water disinfecting, inventory, crafting, clothes with different water/cold resistance levels and more. On a surface, it is really easy to use. Basic setup is about 10 lines of code – and you have everything at your disposal). 

Code is open, so you can customize everything for your particular game needs.


Saving/Loading of the engine state is fully supported (including the entire health status, active diseases, injuries, treatment progress, entire inventory, clothes, appliances). Of course anything you add to your implementation (new diseases, inventory items, injuries, everything) can be saved the same way too.
The size of a serialized state object is about 14K (non-formatted JSON). Saving engine state is taking about 35ms, loading -- about 30.


To save the engine state and to completely restore it, you have to write two lines of code, like this:

``` csharp
var state = ZaraEngine.EngineState.GetState(gc);
// ........
ZaraEngine.EngineState.RestoreState(gc, state, savedWorldTime => _worldTime = savedWorldTime);
// ... where gc is an engine instance (IGameController)
```

![Zara Demo app screen](http://imw.su/ZaraDemoScreen_06.png)

It includes:
+ Health Engine that controls
  - Heart Rate
  - Body Temperature
  - Blood Pressure
  - Stamina (incl. fatigue and exhaustion)
  - Food Level
  - Water Level
  - Blood Level
  - Oxygen Level
  - Wetness level (how wet is your character)
  - Warmth level (incl. cloths with different warmth levels)
  - Sleep cycles
  - Side effects from running, carrying weight, swimming, freezing
  - Dizziness and blackouts as a result of an extreme exhaustion and/or diseases and/or medical effects
  - Sneezing
  - Caughing
  - Diseases and injuries and their treatment (see more below)
  
+ Inventory Engine
  - Support for a different item types (handheld, tools, medical items, food (incl. food spoiling), water (incl. disinfection and boiling))
  - Inventory controller with weight and overweight support
  - Crafting engine that uses simple fluent syntax. Any combinations of any number of items in any amounts are supported. "Stick"+"Rope"+"Pin"+"Worm(2)"="Fishing Rod With Bait", including custom actions like "Water"+"Lit Torch"="Unlit Torch"
  - Custom validation rules for any crafting recipe
  
+ Disease Engine
  - Create any disease with any effects on a player with any kind of treatment using simple fluent pattern
  - Support for self-healing chance for a particular disease
  - Support for disease scheduling (i.e. get flu after four in-game hours after being in a cold water, and so on)
  - Treatment with pills or injections in any order and with any number of stages
  - Timed treatment (to cure X, you need to take the Y pill every four hours, or else disease will become active again)
  - Every disease progresses gradually, and treatment inverts the effects, so they go in opposite order -- to the "healthy" state. If the treatment is missed mid-way, disease will invert back and will start to get worse, and you will need to start all the treatment steps from scratch
  - Any number of simultanious diseases, injuries and medicine effects: everything will be combined into a single resulted health state
  
 + Injury Engine
   - Cuts, Fractures, Open fratcures, or anything custom you like
   - Injuries can cause diseases (deep cut on a later stage can cause blood poisoning, and so on)
   - Injuries can cause blood loss, or any other effect on player's health
   - Treatment with bandages, splints, pills, injections in any order you like
   
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

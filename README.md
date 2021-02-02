# Planetarity v1.2 gameplay
Project was made from scratch in 2 days.

## Task
Create a planetary artillery arcade in pseudo 2D (i.e. 2D game in the 3D world).
The player is in control of a planet, rotating around Sun. A planet can shoot rockets, rockets
obey gravity (of Sun and of other planets). On collision with Sun, rocket disappears, on
collision with a planet - does damage.
Create a singleplayer game mode with AI players (random number from
configurable min/max values).

#### Rest of features:
* Planets should be visually different (and random)
* Implement 3 types of rockets with different base stats (acceleration, weight,
cooldown, etc.), which are distributed randomly amongst planets
* Pause/resume feature
* Save/load feature
* UI: main menu, HUD, planet HUD (HP bar, shooting cooldown)

#### Note
* You can find configurable min/max values for players count in Tools/EnemiesEditorManager window.
* Gravity of planets may not impact rockets very much, thats because planet's weight setups randomly. You can specify min/max range for it in Planet prefab/minWeight/maxWeight.

# Screenshots
## Main menu
![](https://github.com/IDmikael/Planetarity-v1.2-gameplay/blob/main/Screenshots/Screen1.png)

## Gameplay
<img src="https://github.com/IDmikael/Planetarity-v1.2-gameplay/blob/main/Screenshots/Screen2.png" width="620" height="350">
<img src="https://github.com/IDmikael/Planetarity-v1.2-gameplay/blob/main/Screenshots/Screen3.png" width="620" height="350">

## Pause and End Game windows
<img src="https://github.com/IDmikael/Planetarity-v1.2-gameplay/blob/main/Screenshots/Screen4.png" width="620" height="350">
<img src="https://github.com/IDmikael/Planetarity-v1.2-gameplay/blob/main/Screenshots/Screen5.png" width="620" height="350">

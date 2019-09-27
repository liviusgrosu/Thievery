# Thievery

##### Table of Contents  
[Objective](#mechanics)  
[Controls](#controls)  
[Mechanics](#mechanics)  
<a name="headers"/>

## Objective:

* Steal at least 1000 points of loot
* After stealing some loot, exit the house through the front door (where you started)
* Every time you get caught, you lose a life and begin back at the front door
* Lose all 3 lives and you lose the game


## Mechanics:

* Player Visibility 
 
  The players visibility to other entities in game is in relation to 2 things. 
  1. How close they are to a light source 
  2. If there is an obstruction between the light source and the player
  
  The players visibility variable is measured on a scale that ranges between 1 - 5 (not visible - visible)
  
  *Insert image of scale*
  
  Here we can see how that scale is represented in the UI 
  
  Depending on the relation between how far a player is to an enemy and their visibilty variable dictates the enemies ability to spot them

* Player Sound 

  The players sound detection to other entities in game is in relation to 2 things. 
  1. What material the player is standing on
  2. The stance the player has taken 
  3. How frequent the player is producing sound 
  
  The players sound production variable is measured on a scale that ranges between 1 - 5 (quiet - loud)
  
  *Insert image of scale*
  
  Here we can see how that scale is represented in the UI 
  
  Depending on the relation between how far a player is to an enemy and their sound detection variable dictates the enemies ability to hear them

* Movement

* Crouching

  Players can crouch in real life to execute the action of crouching in game 

  Crouching changes the players visibility and sound detection variables by half. However by performing this action, your teleportation distance is also decreased to half which means the player produces more sound when moving around. 
  
  *Show crouching changes the visibility and sound detection UI*
  
  A UI image indicates wether the player is crouching or not 

* Leaning 

  Players can lean in real life to execute the action of leaning in game.
  
  This allows the player to peek around corners without exposing their body to enemies 
  
  *Insert image of player leaning*
  
  *Insert me leaning in real life*

* Enemy AI 

* World Interaction 

## Controls:

| Actions            | Key                                                               |
| ------------------ | ----------------------------------------------------------------- |
| Snap Left Turn     | Thumbstick (Left)                                                 |
| Snap Right Turn    | Thumbstick (Right)                                                |
| Snap Forward Move  | Thumbstick (Up)                                                   |
| Snap Backward Move | Thumbstick (Down)                                                 |
| Teleport           | LT Index (Hold) + Point to floor until green + LT Index (Release) |
| Grab               | LT (Hold) + Grab object + LT (Release)                            |
| Crouch             | (Physically crouch in real life)                                  |

Installation:

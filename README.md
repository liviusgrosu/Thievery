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

 Movement is done in 2 ways 
 1. Snap Movement 
 2. Body Movement + Teleportation 
 
 Snap movement allows players to move and rotate by small increments with the use of the thumbstick. This is used if the player wants to sit on a chair and experience the game without turning their body. 
 
 Body movement and teleportation allows the player to use their body to rotate and pointing to teleport. This is used if the player wants to experience a more immersive game but replicating bodily movement to virtual movement 
 
 *Insert image of me pointing the ground*
 
 Here we see the player pointing to the ground to indicate on where they want to move 
 1. Green means that they are allowed to move there 
 2. Red means that they are NOT allowed to move there
 
 As mentioned before, crouching reduces the range of movement
 
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

  The enemy entities have 4 states:
  1. Patroling
     - Enemies assume their patrol path by walking in it or just standing and rotating every a specified amount of seconds
     - Enemies have no knowledge of player precense
  2. Suspicious 
     - Enemy barely hears or barely sees the player but are unsure about their presence 
     - If the player continues to put the enemy in this state but remaining barely seen or barely heard then the enemy will investitage
     - If not, then the enemy will remain in this state until they calm down and go back to their patrolling state
  3. Investigative
     - The enemy breaks out of there patrolling route/pattern and investigates the last know player position 
     - If the player continues to remain in the enemys barely seen viewing cone or hearing sphere then this updates the enemies last player position 
     - Once the enemy sees the player then they will transition to the alert state 
     - If the player escapes the viewing cone or hearing sphere and the enemy reaches the last player position, then they go back to patrolling
  4. Alert 
     - The player will speed walk to the last player position regardless if they barely see or barely hear the player
     - If the player escapes the viewing cone or hearing sphere and the enemy reaches the last player position, then they go back to the investigative state 
     
  Of course, if the player is caught by one of the enemies then they are sent back to the start of the level with 1 less life     
    
* World Interaction 

  Players can interact with 3 things in the virtual world:
  1. Doors 
  2. Loot 
  3. Chests 
  
  With doors and chests, player imitate the action of opening them like you would in real life:
  1. Grab the handle with the left trigger and hold 
  2. Swing it open (doors go through you so you dont have to step back)
  3. Releasing the handle by releasing the left trigger 
  
  *Insert image of opening a door and chest*
  
  Doors can used as a form of cover as you can open it ajar, lean into the opening, and observer 
  
  *Insert image of peaking through a door*

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

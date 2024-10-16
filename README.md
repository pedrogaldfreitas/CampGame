<h1>About This Project</h1>

This repository is for a WIP Unity game I plan on releasing in the coming years. It's a top-down 2D game based on a summer camp which is haunted by a unique, lost indigenous tribe.

<b>There are many mechanics implemented, including but <i>definitely</i> not limited to:</b>

* A player/object height, floor height system in 2D space.
  * This is for determining where an object's shadow should be based on the height of the platform it's standing on.
  * This allows for jumping mechanics, platforming, accurate projectile hit detection, etc.
  * See FakeHeightObject.cs and LandTargetScript.cs for some of the implementation.
  * Slopes implemented, where player can walk along it to rise to a higher platform.
  * Jumping from a platform to a slope will also calculate the exact height the player should land in depending on where the player lands along the slope.
  * Very polished, objects never clip through platforms, and collisions work perfectly!
* Player running + braking
  * Animations for player running and braking after running.
  * "Drifting" when running and suddenly turning to a perpendicular direction.
* Combat system
  * Player can swing stick to hit enemies.
  * If an enemy is hit particularly hard, they may be sent flying and spinning.
  * Player can take and receive damage, with a visible health bar for both the player and all its enemies.
* Cutscene + Dialogue System
  * Fully functional dialogue system where text appears on screen letter by letter.
  * Letters can be warped, colored, scaled, etc. with ease.
  * Cutscenes can be made where the mouths, eyes, arms, legs of the characters are swapped, and more.
  * The versatility and ease of making cutscenes allows for VERY lively and charming cutscenes!
  * Dialogue options available with branching dialogue and cutscenes depending on player's response.
  * Sometimes there is a timer visible on screen for responding. Once it runs out, the player will respond randomly.
* Character Animation System
  * Character animations are very easy to implement since the system is so robust.
  * Can swap eyes, nose, mouth, head, torso, legs, and arms individually to create animations using fewer resources (using animator state trees).
  * Walking animation uses only Red, Green, and Blue colors. <b>This allows the walking animation for the legs to be re-used</b> between similar characters while swapping RGB colors for the characters' skin, pants, and shoe colors on demand.
* State Panel
  * For testing individual "missions" and game states, a state panel is implemented where the player can select a specific part of the game to test.
* Sprites + Art
  * With a XP Pen drawing tablet and Paint Tool SAI, drew hundreds of sprites overall.
  * Drew characters (not all implemented in the repository yet), foliage, objects, animations, facial expressions, etc.

There is certainly more, and I will be providing video soon once I have a simple stage up and running to showcase the mechanics. I can't wait to show it!

<h2>For Reference</h2>
The scripts I've written are in Assets/Scripts.
The sprites I've drawn are in Assets/Sprites.
Animations are in Assets/Animation.

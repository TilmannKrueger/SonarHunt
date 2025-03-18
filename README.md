# SonarHunt
Very simple console game: Find an enemy submarine in dephts of the ocean, using pings only!

I had a similar game, written in BASIC, when I was a kid. It was one of the first programs I really understood,
and I learned a lot modifying it. So I figured it would be a good choice for others to start programming.

Have fun!

## Attributions

I'm a programmer, not an artist. So, I have to build upon the work of others. For this game I used the 
following 3rd party resources:

* The teletyper sound is based on
  DIGITAL TYPING.mp3 by sidadrumbum -- https://freesound.org/s/42977/ -- License: Attribution 3.0

* And the ping sound is based on
  https://pixabay.com/sound-effects/sonar-ping-95840/ by SamsterBirdies (Freesound)

Thank you very much! This little project would have been rally awful but for you!

## Platforms
Unfortunately, Windows only, at the moment. The game relies heavily on sound, and the simplest way to play sound 
was using the system.extensions.SoundPlayer, which is only present on Windows. If you successfully port it to 
another platform, feel free to contact me to open a pull request.

## Purpose
This is a simple game to pick up game programming in C#. Actually, it doesn't take much to get something
really cool going!

## Features
* Console game interface
* ASCII splash screen
* User input
* Sound output
* Special effect: Teletype'd text (it looks so cool with a green-on-black Windows Terminal with retro terminal effects enabled!)

## Compiling
* Make sure you use VS 2022 or later (Community Edition should be fine), and have the language support for C# installed
* The resources (sound files) live in the project, so a launchSettings.json has been added to make sure they are found

## How it is played
The game takes place in a grid (10x10 by default). The player starts at (0, 0), which is the lower left corner. A (scaled down)
example starting situation looks like this:

       +---+---+---+---+---+
       |   |   |   |   |   |
       +---+---+---+---+---+
       |   |   |   |   |   |
       +---+---E---+---+---+  <- Enemy
       |   |   |   |   |   |
       +---+---+---+---+---+
    ^  |   |   |   |   |   |
    |  +---+---+---+---+---+
    y  |   |   |   |   |   |
       P---+---+---+---+---+
     (0|0)  x ->

The enemy submarine starts at a random position, but not on any of the left or lower edges, to avoid the sub starting at the player 
position, or the game becoming too easy. Player movements are restricted to this grid, and the enemy doesn't move.

The player, however doesnt see the x/y grid, nor can he navigate the grid using x/y references. Instead, a all player movements 
happen in geographic reference frame, or in north, east, south, west directions:

              North
                ^
                |
       West <-------> East
                |
                v
              South

Note that the Y axis of the grid aligns with the north-south axis, and the X axis aligns with east-west.

The player sees only his current position, and can issue commands, e.g. 'n' for a move in northern (positive Y) direction. 'e', 's', 
and 'w' work accordingly. Another command is 'p', which sends out a "ping", which is reflected by the enemy sub. As in the real world,
sound travels with a finite speed, so the echo is delayed more the further the player is away from the target.

The player has to listen carefully how the time between ping and echo is changing as he moves around the grid. To close in on the 
target, the player must move towards shorter delays. Once the player has moved to the same grid location as the target, the game ends
and the player score (how many steps it took him to find the orther sub) is displayed.

## How it works

### Basic structure

Execution starts - as in all serious programs - in a static method named "Main". Here, the basic execution plan is visible: 

1. Render the splash screen
1. Initialize the game and load all resources (in our case, the sound files)
1. Print the game introduction story
1. Enter the game loop
1. Print the game results

#### The game state

The state consists out of the player x/y position, the target x/y position, and the game grid dimensions, with each of this six values 
stored as an int. Another int is used to store the step count, which acts as the player score.

#### The game loop

The game loop is the heart of the actual game: Here, the player input is transformed into updates of the game state. Most of the 
work in the game loop is done in the Sonar.SonarHuntGame.MakeTurn() method. Here, a prompt is printed, informing the player about 
his current position, and options. When a keystroke is detected, the respective key is evaluated according to the associated 
command. E.g. if the 'n' key was pressed, which means "move north", the Y position of the player is incremented by 1 (remember 
that the northern direction corresponds with the positive Y axis). Of course, the update of player position is skipped if the 
player was already on the northern edge of the playing field. And note that the step count is only updated when the player 
input actually resulted in a player move.
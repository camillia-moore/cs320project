# cs320project

Update: April 17th, 2023

## Compilation
To compile, open either GameStateTesting.sln, or GameStateTesting\GameStateTesting.csproj in Visual Studio, and use the run button at the top

A .exe file should be found at GameStateTesting\bin\Debug\net6.0\GameStateTesting.exe to run the code on Windows withough compilation

## Instructions
- On the main menu, use the arrow keys to select a state, use either enter or space to go to that state
- On the character creation screen, the arrow keys and the mouse can be used to customize your character. Hit the green button to save and go to the story state
- On the Story State, use the buttons with your mouse to advance thru
	- you may press up to go to the title screen, left to go to the character creation screen, and right to go straight to a battle
- On the battle state, the arrow keys are used to select options. Press z to confirm you selection, and to advance the text.
	- when the game says "Kitkat Attacked!", if yopu time your next z press to beat 4 of the music, you will gain 1 mana
	- you may press the d key to toggle a debug menu in battle
		- while this menu is up, pressing m will give you one mana, pressing w will automatically win the battle, and pressing l will automatically lose the battle

## Credits
- Visual Studio was the primary IDE used
- GitHub was used for version control
- MonoGame, developed under Microsoft's XNA Platform, was the main game engine used
- Most of the content was divided up in the States folder under GameStateTesting
	- BattleState and MenuState were mainly coded by Camillia
	- CharacterCreationState was mainly coded by June
	- StoryState, StateBeforeBoss, and EndState were mainly coded by Lyndsey
	- State.cs is an abstract parent state, made by Camillia following the following tutorial
- The concept of using states was borrowed from this tutorial: https://github.com/GrayShadoz/MonoGameRaceTrack
- The BattleClasses Folder was mainly coded by Camillia
- The Customization Folder, and the cs320tests Folder were mainly coded by June
- The Story Folder, and Character.cs were mainly coded by Lyndsey
- Game1.cs and Program.cs are framwork code files
- MockBattleScreen.cs and MockCharCreationUI.cs I belive are test files made by Lyndsey
- All the audio files were created using FL Studio by Camillia
- All the image files were created by June
- All the font files were created by Lyndsey
- This Readme file was typed mostly by Camillia
- Any other file in here is either structure code, or compiled code, or code that Camillia forgot about while typing this Readme.md file




## old readme below

Code for the project is in the folder GameStateTesting
wasn't sure which stuff needed to be left out, so I just included everything
Currently, code is a hodgepodge from 2 tutorials
	- https://docs.monogame.net/articles/getting_started/5_adding_basic_code.html
		used for keyboard input
	- https://github.com/GrayShadoz/MonoGameRaceTrack
		small snippets taken to get game states between multiple class files working
Aside from these two tutorials, rest of code is either the monogame template file, or me choosing class names and keyboard keys

Currently, after running thru Visual Studio, you can use the arrow keys to change the state
	Up: Menu
	Left: Character Creation
	Down: Story
	Right: Battle

Based on other tutorials, it seems good practice is to have a .cs file for each class, and to have them stored in folders on the root directory of the project

README probably needs formating, and this repo unfortunatly probably needs restructuring

	- Camillia Moore, February 10th, 2023

update on 2/12
	Lyndsey's attempts at button making watching a tutorial to see if something like it could be implemented. Didn't get it to work but hopeful.

update: Feb - 13
	Currently, we are using Myra to display text boxes. Myra is distrubuted with an MIT liscense 
	https://github.com/rds1983/Myra
	 -Camillia Moore
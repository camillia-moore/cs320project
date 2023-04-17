using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameStateTesting.States;
using GameStateTesting.BattleClasses;
using System.Text.Json;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace GameStateTesting.States
{
    public class BattleState : State
    {
        private Combatant player;
        private Combatant enemy;
        //private Spell fireball;
        //private Spell iceStorm;
        //private Spell diacute;
        //private Spell healing;
        private Random rand;
        private Spell[] spellbook = new Spell[10];
        private int numSpells;
        private Boolean returnToMenu;
        private int damageDelt;
        private string textToShow;
        private Spell spellCasted;
        private Boolean DEBUG = false;

        //graphics assets
        private Texture2D KitkatSprite;
        private Texture2D EnemySprite;
        private Texture2D TextBox;
        private Texture2D SubMenu;
        private Texture2D HPBarBase;
        private Texture2D HPBarFull;

        //animation variables
        private int playerHPCurrentWidth;
        private int enemyHPCurrentWidth;

        //audio
        private SoundEffectInstance battleMusicInstance;

        //control vars
        private KeyboardState oldKstate;
        private int[] focusedArea;
        private int[] menuSize;
        private int battleState; //probably should be enum
        private SpriteFont font;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public BattleState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            //player = new Combatant("Kit-Kat", "The Default Hero", 30, 9, 5);
            //enemy = new Combatant("Monster", "Generic Enemy", 20, 8, 4);
            numSpells = 0;
            //spellbook[0] = new Spell("Fireball", "Deals damage to the opponent", new BattleClasses.Effect(-10, 0, 0, 1));
            //spellbook[1] = new Spell("Ice Storm", "Uses Ice to Weaken the enemy", new BattleClasses.Effect(0, -2, -2, 1));
            //spellbook[2] = new Spell("Diacute", "Buffs the user's stats", new BattleClasses.Effect(0, +2, +2, 0));
            //spellbook[3] = new Spell("Healing", "Heals the user", new BattleClasses.Effect(+5, 0, 0, 0));
            //numSpells = 4;

            //fireball = new Spell("Fireball", "Deals damage to the opponent", new BattleClasses.Effect(-10, 0, 0, 1));
            //iceStorm = new Spell("Ice Storm", "Uses Ice to Weaken the enemy", new BattleClasses.Effect(0, -2, -2, 1));
            //diacute = new Spell("Diacute", "Buffs the user's stats", new BattleClasses.Effect(0, +2, +2, 0));
            //healing = new Spell("Healing", "Heals the user", new BattleClasses.Effect(+5, 0, 0, 0));
            returnToMenu = false;
            createPlayer("Kitkat", "The Default Hero", 30, 9, 5, 10);
            setEnemy(0);
            rand = new Random();
            focusedArea = new int[2];
            focusedArea[0] = 0; //x
            focusedArea[1] = 0; //y
            menuSize = new int[2];
            menuSize[0] = 4; //x
            menuSize[1] = 1; //y
            battleState = 0;
            textToShow = "Text not shown yet";
        }

        public void createPlayer(String name, String description, int hp, int atk, int def, int mana)
        {
            //function for outside states to create stats for the player
            player = new Combatant(name, description, hp, atk, def);
        }

        public void createEnemy(String name, String description, int hp, int atk, int def)
        {
            //function for outside states to create stats for the enemy
            enemy = new Combatant(name, description, hp, atk, def);
        }

        public void setEnemy(int id)
        {
            //function for outside states to define which preset enemy to fight
            switch (id)
            {
                case 1:
                    createEnemy("Slime", "Just a little slimey boy", 10, 6, 2);
                    break;
                case 2:
                    createEnemy("Jellyfish", "Oooh, spooky jelly", 20, 8, 4);
                    break;
                case 3:
                    createEnemy("Dragon", "I FUDGING LOVBE BOWSDER, I WANMT TO BREAMTHE FIRE!!!!!!", 30, 10, 6);
                    break;
                case 4:
                    createEnemy("Grim Reaper", "Not so evil, he has a design", 40, 12, 8);
                    break;
                default:
                    createEnemy("Monster", "Generic Enemy", 20, 8, 4);
                    break;
            }

        }

        public void addSpell(String name, String description, int HP, int atk, int def, int hd, int manaCost)
        {
            //function for other states to give Kitkat spells
            //please do not add more than 10 spells
            spellbook[numSpells] = new Spell(name, description, new BattleClasses.Effect(HP, atk, def, hd));
            numSpells++;
        }

        public void fromMenu(Boolean fromMenu)
        {
            //function to store whether we should return to the story or the menu
            returnToMenu = fromMenu;
        }

        public void buffPlayer(int HP, int atk, int def, int hd)
        {
            //function to buff the stats of the player
        }

        public void buffEnemy(int HP, int atk, int def, int hd)
        {
            //function to buff the stats of the enemy
        }

        public override void LoadContent()
        {
            //graphics assets
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            KitkatSprite = _content.Load<Texture2D>("cough-story-draft-mc");
            TextBox = _content.Load<Texture2D>("cough-story-box-wide-2");
            SubMenu = _content.Load<Texture2D>("cough-story-box-small-2");
            HPBarBase = _content.Load<Texture2D>("hp-bar-base");
            HPBarFull = _content.Load<Texture2D>("hp-bar-full");

            font = _content.Load<SpriteFont>("TestFont");

            //load correct enemy sprite
            switch (enemy.Name) {
                case "Slime":
                    EnemySprite = _content.Load<Texture2D>("cough-story-draft-slime");
                    break;
                case "Jellyfish":
                    EnemySprite = _content.Load<Texture2D>("covjelly");
                    break;
                case "Dragon":
                    EnemySprite = _content.Load<Texture2D>("covdragon");
                    break;
                case "Grim Reaper":
                    EnemySprite = _content.Load<Texture2D>("covreaper");
                    break;
                default:
                    EnemySprite = _content.Load<Texture2D>("covreaper");
                    break;
            }

            //audio
            SoundEffect battleMusic = _content.Load<SoundEffect>("battle_music_test");
            battleMusicInstance = battleMusic.CreateInstance();
            battleMusicInstance.IsLooped = true;
            battleMusicInstance.Play();
            
        }

        public override void Update(GameTime gameTime)
        {
            var newKstate = Keyboard.GetState();

            if (newKstate.IsKeyDown(Keys.Up) && oldKstate.IsKeyUp(Keys.Up))
            {
                focusedArea[1] += -1;
            }
            if (newKstate.IsKeyDown(Keys.Down) && oldKstate.IsKeyUp(Keys.Down))
            {
                focusedArea[1] += +1;
            }
            if (newKstate.IsKeyDown(Keys.Left) && oldKstate.IsKeyUp(Keys.Left))
            {
                focusedArea[0] += -1;
            }
            if (newKstate.IsKeyDown(Keys.Right) && oldKstate.IsKeyUp(Keys.Right))
            {
                focusedArea[0] += +1;
            }
            //resize selection into the menu size
            if (focusedArea[0] < 0) { focusedArea[0] = menuSize[0] - 1; }
            if (focusedArea[0] > menuSize[0] - 1) { focusedArea[0] = 0; }
            if (focusedArea[1] < 0) { focusedArea[1] = menuSize[1] - 1; }
            if (focusedArea[1] > menuSize[1] - 1) { focusedArea[1] = 0; }

            if (newKstate.IsKeyDown(Keys.Z) && oldKstate.IsKeyUp(Keys.Z)) { doOption(); initBattleState(); }

            oldKstate = newKstate;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(new Color(60, 60, 60));

            _spriteBatch.Begin();
            _spriteBatch.Draw(KitkatSprite, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(EnemySprite, new Vector2(682, 0), Color.White);
            _spriteBatch.Draw(HPBarBase, new Vector2(49, 475), Color.White);
            _spriteBatch.Draw(HPBarBase, new Vector2(831, 475), Color.White);

            //draw HP bar based on how much hp both sides have
            int hpAnimationSpeed = 4;
            int[] playerHP = player.getHP();
            int playerHPTargetWidth = HPBarFull.Width * playerHP[0] / playerHP[1];
            if (playerHPCurrentWidth > playerHPTargetWidth) { playerHPCurrentWidth -= hpAnimationSpeed; }    //decrease size of bar
            else if (playerHPCurrentWidth < playerHPTargetWidth) { playerHPCurrentWidth += hpAnimationSpeed; }   //increase size of bar
            if ((playerHPCurrentWidth <= playerHPTargetWidth + hpAnimationSpeed - 1) //edge case of really close
                && (playerHPCurrentWidth >= playerHPTargetWidth - hpAnimationSpeed + 1))
                { playerHPCurrentWidth = playerHPTargetWidth; }
            Rectangle playerHPBarLength = new Rectangle(0, 0, playerHPCurrentWidth, HPBarFull.Height);
            _spriteBatch.Draw(HPBarFull, new Vector2(54, 480), playerHPBarLength, Color.White);

            //same for enemy
            int[] enemyHP = enemy.getHP();
            int enemyHPTargetWidth = HPBarFull.Width * enemyHP[0] / enemyHP[1];
            if (enemyHPCurrentWidth > enemyHPTargetWidth) { enemyHPCurrentWidth -= hpAnimationSpeed; } //decrease size of bar
            else if (enemyHPCurrentWidth < enemyHPTargetWidth) { enemyHPCurrentWidth += hpAnimationSpeed; }  //increase size of bar
            if ((enemyHPCurrentWidth <= enemyHPTargetWidth + hpAnimationSpeed - 1)  //edge case
                && (enemyHPCurrentWidth >= enemyHPTargetWidth - hpAnimationSpeed + 1))
                { enemyHPCurrentWidth = enemyHPTargetWidth; }
            Rectangle enemyHPBarLength = new Rectangle(0, 0, enemyHPCurrentWidth, HPBarFull.Height);
            _spriteBatch.Draw(HPBarFull, new Vector2(836 + HPBarFull.Width - enemyHPCurrentWidth, 480), enemyHPBarLength, Color.White);

            _spriteBatch.Draw(TextBox, new Vector2(0, 485), Color.White);

            if (DEBUG)
            {
                _spriteBatch.DrawString(font, "Focused Area: " + focusedArea[0] + ", " + focusedArea[1], new Vector2(50, 225), Color.Red);
                _spriteBatch.DrawString(font, "Battle State: " + battleState, new Vector2(50, 250), Color.Red);
                _spriteBatch.DrawString(font, textToShow, new Vector2(50, 275), Color.Red);
            }
            int[] textStates = { 1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14 };
            if (battleState == 0)
            {
                //display the main battle menu

                //get color for the text
                Color fightColor = Color.White;
                Color spellsColor = Color.White;
                Color statsColor = Color.White;
                Color fleeColor = Color.White;
                switch (focusedArea[0])
                {
                    case 0:
                        fightColor = Color.Blue;
                        break;
                    case 1:
                        spellsColor= Color.Blue;
                        break;
                    case 2:
                        statsColor= Color.Blue;
                        break;
                    case 3:
                        fleeColor = Color.Blue;
                        break;
                    default:
                        break;
                }

                //print the text
                _spriteBatch.DrawString(font, "FIGHT!", new Vector2(50, 575), fightColor);
                _spriteBatch.DrawString(font, "SPELLS!", new Vector2(250, 575), spellsColor);
                _spriteBatch.DrawString(font, "STATS!", new Vector2(450, 575), statsColor);
                _spriteBatch.DrawString(font, "FLEE!", new Vector2(650, 575), fleeColor);
            }
            else if (textStates.Contains(battleState) )
            {
                //just display text in the text box
                _spriteBatch.DrawString(font, textToShow, new Vector2(50, 600), Color.White);
            }
            else  if (battleState == 7)
            {
                //display the stats subscreen
                _spriteBatch.DrawString(font, "Here's your stats ^-^", new Vector2(50, 600), Color.White);
                _spriteBatch.Draw(SubMenu, new Vector2(467, 67), Color.White);
                int[] playerStats = player.getStats();
                _spriteBatch.DrawString(font, player.Name + 
                    "\nAttack: " + playerStats[0] + " + " + playerStats[1] +
                    "\nDefense: " + playerStats[2] + " + " + playerStats[3], new Vector2(500, 95), Color.White);
            }
            else if (battleState == 8)
            {
                //display the spells subscreen
                _spriteBatch.Draw(SubMenu, new Vector2(467, 67), Color.White);

                //get colors for text
                Color unfocusedColor = Color.White;
                Color focusedColor = Color.Blue;
                Color textColor;

                for (int i = 0; i < numSpells; i++)
                {
                    if (focusedArea[1] == i) { textColor = focusedColor; }
                    else { textColor = unfocusedColor; }
                    _spriteBatch.DrawString(font, spellbook[i]._name, new Vector2(500, 95 + i * 25), textColor);
                }
                if (focusedArea[1] == numSpells) { textColor = focusedColor; }
                else { textColor = unfocusedColor; }
                _spriteBatch.DrawString(font, "Cancel", new Vector2(500, 95 + numSpells * 25), textColor);
            }
            _spriteBatch.End();
        }

        private void doOption()
        {
            //function to handle game flow between differrent game states
            switch (battleState)
            {
                case 0:
                    //current state: menu
                    //find out where the player wants to go, go there
                    switch (focusedArea[0])
                    {
                        case 0:
                            //attack
                            battleState = 1;
                            break;
                        case 1:
                            //spells
                            battleState = 8;
                            break;
                        case 2:
                            //stats
                            battleState = 7;
                            break;
                        case 3:
                            //flee
                            battleState = 12;
                            break;
                    }
                    break;
                case 1:
                    //just displayed that the player attacks, go to displaying damage
                    battleState = 2;
                    break;
                case 2:
                    //just displayed player dealt damage, figure out if enemy gets turn or was defeated
                    if (enemy.isDefeated())
                    {
                        battleState = 6;
                    }
                    else { battleState = 3; }
                    break;
                case 3:
                    //just said that enemy attacks, go to displaying their damage
                    battleState = 4;
                    break;
                case 4:
                    //just displayed the damage the enemy dealt, determine if player lived
                    if (player.isDefeated())
                    {
                        battleState = 5;
                    }
                    else { battleState = 0; }
                    break;
                case 5:
                    //just showed that player died, go to the state before the boss, or the menu state
                    battleMusicInstance.Stop();
                    if (returnToMenu)
                    {
                        _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
                    }
                    else if (enemy.Name == "Grim Reaper")
                    {
                        _game.ChangeState(new EndState(_game, _graphicsDevice, _content));
                    }
                    else
                    {
                        _game.ChangeState(new StateBeforeBoss(_game, _graphicsDevice, _content));
                    }
                    break;
                case 6:
                    //just showed that the enemy died, go back to either the menu, story, or before boss state
                    battleMusicInstance.Stop();
                    if (returnToMenu)
                    {
                        _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
                    }
                    else if (enemy.Name == "Dragon")
                    {
                        _game.ChangeState(new StateBeforeBoss(_game, _graphicsDevice, _content));
                    }
                    else if (enemy.Name == "Grim Reaper")
                    {
                        _game.ChangeState(new EndState(_game, _graphicsDevice, _content));
                    }
                    else
                    {
                        _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
                    }
                    break;
                case 7:
                    //just showed the stats subscreen, go back to the main battle menu
                    battleState = 0;
                    break;
                case 8:
                    //player just chose a spell, determine if they can cast it, and either cast it, or display not enough mana
                    //also, determine which spell they casted
                    //TODO: need to also have way to cancel casting a spell
                    if (focusedArea[1] == numSpells)
                    {
                        //player selected cancel
                        battleState = 0;
                    }
                    else
                    {
                        spellCasted = spellbook[focusedArea[1]]; //set spellCasted to be the spell that was chosen
                        Boolean sufficentMana = true; //will eventually implement this
                        if (sufficentMana)
                        {
                            battleState = 9;
                        }
                        else
                        {
                            battleState = 11;
                        }
                    }
                    break;
                case 9:
                    //just showed which spell kit kat casted, go to displaying the effects
                    battleState = 10;
                    break;
                case 10:
                    //just showed the spell effects, determine if enemy gets turn
                    if (enemy.isDefeated())
                    {
                        battleState = 6;
                    }
                    else { battleState = 3; }
                    break;
                case 11:
                    //just showed that there was not enough mana, go back to the spell screen
                    battleState = 8;
                    break;
                case 12:
                    //player just tried to flee, determine if successful or not
                    Boolean fleeSuccesful = false; //will implement this eventually
                    if (fleeSuccesful)
                    {
                        battleState = 13;
                    }
                    else
                    {
                        battleState = 14;
                    }
                    break;
                case 13:
                    //just showed that flee was successfull, exit to the correct state
                    battleMusicInstance.Stop();
                    if (returnToMenu)
                    {
                        _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
                    }
                    else
                    {
                        _game.ChangeState(new StateBeforeBoss(_game, _graphicsDevice, _content));
                    }
                    break;
                case 14:
                    //just showed the flee failed, go to the enemy's turn
                    battleState = 3;
                    break;
                default:
                    //placeholder
                    break;

            }
        }
    
        private void initBattleState()
        {
            //code to run when starting new battle state
            switch (battleState)
            {
                case 0:
                    //just arrived at main battle menu, need to size it and set starting point
                    menuSize[0] = 4;
                    menuSize[1] = 1;
                    focusedArea[0] = 0;
                    focusedArea[1] = 0;
                    break;
                case 1:
                    //just displaying text
                    menuSize[0] = 1;
                    menuSize[1] = 1;
                    textToShow = player.Name + " Attacks!";
                    break;
                case 2:
                    //displaying damage from player, need to calc damage
                    damageDelt = enemy.TakeDamage(player.DealDamage());
                    textToShow = player.Name + " deals " + damageDelt + " damage!";
                    break;
                case 3:
                    //enemy's turn, just displaying text that the enemy attacks
                    textToShow = enemy.Name + " Attacks!";
                    break;
                case 4:
                    //displaying damage from enemy, need to calc it
                    damageDelt = player.TakeDamage(enemy.DealDamage());
                    textToShow = enemy.Name + " deals " + damageDelt + " damage!";
                    break;
                case 5:
                    //just showing text that says player died
                    textToShow = player.Name + " was defeated!";
                    break;
                case 6:
                    //just showing text that says enemy died
                    textToShow = enemy.Name + " was defeated!";
                    break;
                case 7:
                    //showing the stats screen
                    menuSize[0] = 1; //setting these because of a hack Imma do
                    menuSize[1] = 2;
                    break;
                case 8:
                    //showing spell chosing screen
                    menuSize[0] = 1;
                    menuSize[1] = numSpells + 1;
                    focusedArea[0] = 0;
                    focusedArea[1] = 0;
                    break;
                case 9:
                    //displaying text that shows which spell player casts
                    menuSize[0] = 1;
                    menuSize[1] = 1;
                    textToShow = player.Name + " casts " + spellCasted._name + "!";
                    break;
                case 10:
                    //displaying the spell effect, need to actually cast the spell
                    int[] spellEffect = spellCasted.cast();
                    if (spellEffect[3] == 0)
                    {
                        //buff player
                        player.ModifyStats(spellEffect[0], spellEffect[1], spellEffect[2]);
                        textToShow = player.Name + " casts " + spellCasted._name + " on themselves!\n" + spellCasted._description;
                    }
                    else //spellEffect[3] == 1
                    {
                        //nerf enemy
                        enemy.ModifyStats(spellEffect[0], spellEffect[1], spellEffect[2]);
                        textToShow = player.Name + " casts " + spellCasted._name + " on " + enemy.Name + "!\n" + spellCasted._description;
                    }
                    break;
                case 11:
                    //display that there was not enough mana
                    menuSize[0] = 1;
                    menuSize[1] = 1;
                    textToShow = "Not Enough Mana!";
                    break;
                case 12:
                    //display that player tries to flee
                    menuSize[0] = 1;
                    menuSize[1] = 1;
                    textToShow = player.Name + " tries to flee";
                    break;
                case 13:
                    //display that the player was successful in fleeing
                    textToShow = player.Name + " fled";
                    break;
                case 14:
                    //display that the player failed to flee
                    textToShow = player.Name + " fails";
                    break;
                default:
                    break;
            }
        }
    }
}



/*
 old flee code
                int[] hp = player.getHP();
                double fleeChance = hp[0] / hp[1];
                double fleeSuccess = rand.Next(0, hp[1]) / hp[1];
                string messagePrinted;

                //if (fleeChance > fleeSuccess) //flee was successful
                if (false) //no fleeing for you
                {
                    messagePrinted = player.Name + " got away!";
                    _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));

                }
                else  //couldn't flee
                {
                    messagePrinted = "Couldn't get away!";
                }
old stats code
                int[] hp = player.getHP();
                int[] stats = player.getStats();
                int[] enemyHP = enemy.getHP();
                string toDisplay = enemy.Name + " HP: " + enemyHP[0] + "/" + enemyHP[1] + "\n" +
                                    player.Name + " HP: " + hp[0] + "/" + hp[1] + "\n" +
                                    "Atk: " + stats[0] + " + " + stats[1] + "\n" +
                                    "Def: " + stats[2] + " + " + stats[3];
*/
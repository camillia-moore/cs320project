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
using GameStateTesting.Customization;
using GameStateTesting.Story;
using System.Text.Json;
using Microsoft.Xna.Framework.Audio;

namespace GameStateTesting.States
{
    public class BattleState : State
    {
        private Combatant player;
        private Combatant enemy;
        private Random rand;
        private Spell[] spellbook = new Spell[10];
        private int numSpells;
        private Boolean returnToMenu;
        private int damageDelt;
        private string textToShow;
        private Spell spellCasted;
        private int currentMana;
        private int maxMana;
        private Boolean spellBookMade;
        private Boolean DEBUG = false;

        //graphics assets
        private Texture2D KitkatSprite;
        private Texture2D EnemySprite;
        private Texture2D TextBox;
        private Texture2D SubMenu;
        private Texture2D HPBarBase;
        private Texture2D HPBarFull;
        private Texture2D ManaBarBase;
        private Texture2D ManaBarFull;
        private Texture2D DebugBackground;

        //animation variables
        private int playerHPCurrentWidth;
        private int enemyHPCurrentWidth;
        private int manaBarCurrentWidth;

        //audio
        private SoundEffectInstance battleMusicInstance;
        private int bpm;
        private int millisecondsPerBeat;
        private int beatNumber;
        private int beatError;
        private int msInBeat;
        private Boolean onBeat;
        int lowerBound;
        int upperBound;
        private TimeSpan startTime;
        private TimeSpan elapsedTime;
        private Boolean startTimeInitialized;

        //sound effects
        private SoundEffect SE_1;
        private SoundEffect SE_2;
        private SoundEffect SE_3;
        private SoundEffect SE_4;
        private SoundEffect SE_5;

        //control vars
        private KeyboardState oldKstate;
        private int[] focusedArea;
        private int[] menuSize;
        private int battleState; //probably should be enum
        private SpriteFont font;

        //displaying June's custom characters, this block was by June
        private Texture2D charBase;
        private Texture2D charHead;
        private Texture2D charFace;
        private Texture2D charBody;
        private Rectangle[] charHeadSource;
        private Rectangle[] charFaceSource;
        private Rectangle[] charBodySource;
        //back to my code -Camillia

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public BattleState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            numSpells = 0;
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
            startTimeInitialized = false;
            currentMana = 0;
            maxMana = 6;
            spellBookMade = false;
            createSpellBook();
            spellBookMade = true;
        }

        public void createPlayer(String name, String description, int hp, int atk, int def, int mana)
        {
            //function for outside states to create stats for the player
            //should deprecate
            player = new Combatant(name, description, hp, atk, def);
        }

        public void createEnemy(String name, String description, int hp, int atk, int def)
        {
            //function for outside states to create stats for the enemy
            //should deprecate
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
                    createEnemy("Dragon", "I FUCKING LOVE BOWDER, \nI WANT TO BREATHEF FIRRE!!!!!!", 30, 10, 6);
                    //description is reference to https://youtu.be/6qZp_LoNnso?t=287
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
            //function should be made private, and this if should be removed, along with it's boolean
            if(!spellBookMade)
            {
                spellbook[numSpells] = new Spell(name, description, new BattleClasses.Effect(HP, atk, def, hd), manaCost);
                numSpells++;
            }
        }

        private void createSpellBook()
        {
            //function to create the default spellbook
            addSpell("Fireball", "Deals damage to the opponent", -12, 0, 0, 1, 4);
            addSpell("Ice Storm", "Uses Ice to Weaken the enemy", 0, -2, -2, 1, 2);
            addSpell("Diacute", "Buffs the user's stats", 0, +3, +3, 0, 3);
            addSpell("Healing", "Heals the user", +20, 0, 0, 0, 5);
        }

        public void fromMenu(Boolean fromMenu)
        {
            //function to store whether we should return to the story or the menu
            returnToMenu = fromMenu;
        }

        public void buffPlayer(int HP, int atk, int def, int _)
        {
            //function to buff the stats of the player before the battle
            //last parameter should be removed
            player.ModifyStats(HP, atk, def);
        }

        public void buffEnemy(int HP, int atk, int def, int _)
        {
            //function to buff the stats of the enemy before the battle
            //last parameter should be removed
            enemy.ModifyStats(HP, atk, def);
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
            ManaBarBase = _content.Load<Texture2D>("mana-bar");
            ManaBarFull = _content.Load<Texture2D>("mana-bar-full-2");
            DebugBackground = _content.Load<Texture2D>("debug-background");

            //loading content june made, next 4 blocks are june's code
            charBase = _content.Load<Texture2D>("char-base-new");
            charHead = _content.Load<Texture2D>("char-head");
            charFace = _content.Load<Texture2D>("char-face");
            charBody = _content.Load<Texture2D>("char-body");

            charHeadSource = new Rectangle[4];
            charHeadSource[0] = new Rectangle(0, 0, 1, 1);
            charHeadSource[1] = new Rectangle(0, 0, 183, 133);
            charHeadSource[2] = new Rectangle(233, 46, 123, 103);
            charHeadSource[3] = new Rectangle(0, 206, 199, 144);

            charFaceSource = new Rectangle[4];
            charFaceSource[0] = new Rectangle(0, 0, 287, 205);
            charFaceSource[1] = new Rectangle(288, 0, 287, 205);
            charFaceSource[2] = new Rectangle(0, 206, 287, 205);
            charFaceSource[3] = new Rectangle(288, 206, 287, 205);

            charBodySource = new Rectangle[4];
            charBodySource[0] = new Rectangle(0, 0, 1, 1);
            charBodySource[1] = new Rectangle(0, 0, 364, 322);
            charBodySource[2] = new Rectangle(480, 0, 307, 243);
            charBodySource[3] = new Rectangle(98, 398, 198, 227);
            //back to my code -Camillia


            font = _content.Load<SpriteFont>("TestFont");

            SoundEffect battleMusic;

            //load correct enemy sprite and music
            switch (enemy.Name) {
                case "Slime":
                    EnemySprite = _content.Load<Texture2D>("cough-story-draft-slime");
                    battleMusic = _content.Load<SoundEffect>("battle_music_test_125_bpm");
                    bpm = 125;
                    break;
                case "Jellyfish":
                    EnemySprite = _content.Load<Texture2D>("covjelly");
                    battleMusic = _content.Load<SoundEffect>("battle_music_test_125_bpm");
                    bpm = 125;
                    break;
                case "Dragon":
                    EnemySprite = _content.Load<Texture2D>("covdragon");
                    battleMusic = _content.Load<SoundEffect>("battle_music_test_125_bpm");
                    bpm = 125;
                    break;
                case "Grim Reaper":
                    EnemySprite = _content.Load<Texture2D>("covreaper");
                    battleMusic = _content.Load<SoundEffect>("battle_song_2");
                    bpm = 160;
                    break;
                default:
                    EnemySprite = _content.Load<Texture2D>("covreaper");
                    battleMusic = _content.Load<SoundEffect>("battle_song_2");
                    bpm = 160;
                    break;
            }

            //audio control variable initialization
            battleMusicInstance = battleMusic.CreateInstance();
            battleMusicInstance.IsLooped = true;
            battleMusicInstance.Play();
            millisecondsPerBeat = 60000 / bpm;
            beatNumber = 1;
            onBeat = true;
            beatError = 45;
            lowerBound = millisecondsPerBeat - beatError;
            upperBound = beatError;

            //sound effects
            SE_1 = _content.Load<SoundEffect>("SE-1");
            SE_2 = _content.Load<SoundEffect>("SE-2");
            SE_3 = _content.Load<SoundEffect>("SE-3");
            SE_4 = _content.Load<SoundEffect>("SE-4");
            SE_5 = _content.Load<SoundEffect>("SE-5");

        }

        public override void Update(GameTime gameTime)
        {
            var newKstate = Keyboard.GetState();

            //direction keys
            if (newKstate.IsKeyDown(Keys.Up) && oldKstate.IsKeyUp(Keys.Up))
            {
                focusedArea[1] += -1;
                SE_1.Play();
            }
            if (newKstate.IsKeyDown(Keys.Down) && oldKstate.IsKeyUp(Keys.Down))
            {
                focusedArea[1] += +1;
                SE_1.Play();
            }
            if (newKstate.IsKeyDown(Keys.Left) && oldKstate.IsKeyUp(Keys.Left))
            {
                focusedArea[0] += -1;
                SE_1.Play();
            }
            if (newKstate.IsKeyDown(Keys.Right) && oldKstate.IsKeyUp(Keys.Right))
            {
                focusedArea[0] += +1;
                SE_1.Play();
            }

            //resize selection into the menu size
            if (focusedArea[0] < 0) { focusedArea[0] = menuSize[0] - 1; }
            if (focusedArea[0] > menuSize[0] - 1) { focusedArea[0] = 0; }
            if (focusedArea[1] < 0) { focusedArea[1] = menuSize[1] - 1; }
            if (focusedArea[1] > menuSize[1] - 1) { focusedArea[1] = 0; }
            
            if(newKstate.IsKeyDown(Keys.X) && oldKstate.IsKeyUp(Keys.X))
            {
                //back button on sub menus
                switch (battleState)
                {
                    case 7:
                        //stats to main
                        battleState = 0;
                        initBattleState();
                        break;
                    case 8:
                        //spells to main
                        battleState = 0;
                        initBattleState();
                        break;
                    default:
                        break;
                }
            }

            //main function key, selects current option
            if (newKstate.IsKeyDown(Keys.Z) && oldKstate.IsKeyUp(Keys.Z)) { elapsedTime = gameTime.TotalGameTime;  doOption(); initBattleState(); }

            if (DEBUG)
            {
                //keystrokes only accepted in debug mode
                if(newKstate.IsKeyDown(Keys.M) && oldKstate.IsKeyUp(Keys.M)) { 
                    //give player 1 mana
                    currentMana += 1; 
                    if (currentMana > maxMana) { currentMana = maxMana; }
                }
                if(newKstate.IsKeyDown(Keys.W) && oldKstate.IsKeyUp(Keys.W))
                {
                    //trigger a win
                    enemy.ModifyStats(-10000, 0, 0);
                    battleState = 6;
                    initBattleState();
                }
                if (newKstate.IsKeyDown(Keys.L) && oldKstate.IsKeyUp(Keys.L))
                {
                    //trigger a loss
                    player.ModifyStats(-10000, 0, 0);
                    battleState = 5;
                    initBattleState();
                }
            }
            if(newKstate.IsKeyDown(Keys.D) && oldKstate.IsKeyUp(Keys.D)) { DEBUG = !DEBUG; }  //swaps debug state


            //grabs the start time on the first frame
            if (!startTimeInitialized) { startTime = gameTime.TotalGameTime; startTimeInitialized = true;  }

            //calculates how many milliseconds it's been since the last music beat
            msInBeat = (int)((Math.Truncate(gameTime.TotalGameTime.TotalMilliseconds - startTime.TotalMilliseconds)) % millisecondsPerBeat);
            
            if ((msInBeat < upperBound) || (msInBeat > lowerBound))  //if the current time is within the error bounds
            {
                if (!onBeat)  //if we were not already on the beat
                {
                    //increment beat number, handle wrap around, and say we are on the beat
                    beatNumber += 1;
                    if (beatNumber == 5) { beatNumber = 1; }
                    onBeat = true;
                }
            }
            else { onBeat = false; }   //else, we are not on the beat, so say we are not on the beat

            //put keyboard state into old keyboard state
            oldKstate = newKstate;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //clear screen
            _graphicsDevice.Clear(new Color(60, 60, 60));

            //sprite offset for player and enemy for the on beats
            int musicSpriteOffset = 0;
            if (onBeat) { musicSpriteOffset = 2; }

            //sprite offsets for june's code
            int xOffset = -75;
            int yOffset = 0;

            _spriteBatch.Begin();

            //drawing main sprites
            //_spriteBatch.Draw(KitkatSprite, new Vector2(0, 0 + musicSpriteOffset), Color.White);
            drawKitKat(xOffset, yOffset + musicSpriteOffset);
            _spriteBatch.Draw(EnemySprite, new Vector2(682, 0 + musicSpriteOffset), Color.White);
            _spriteBatch.Draw(ManaBarBase, new Vector2(49, 448), Color.White);
            _spriteBatch.Draw(HPBarBase, new Vector2(49, 475), Color.White);
            _spriteBatch.Draw(HPBarBase, new Vector2(831, 475), Color.White);
            
            //these next three blocks could probably be three function calls
            //draw mana bar based on how much mana is left, with animations
            int manaSpeed = 4;
            int manaBarTargetWidth = ManaBarFull.Width * currentMana / maxMana;
            if(manaBarCurrentWidth > manaBarTargetWidth) { manaBarCurrentWidth -= manaSpeed; }   //decrease bar size
            else if (manaBarCurrentWidth < manaBarTargetWidth) { manaBarCurrentWidth += manaSpeed; }   //increase bar size
            if ((manaBarCurrentWidth <= manaBarTargetWidth + manaSpeed - 1) //edge case of really close
                && (manaBarCurrentWidth >= manaBarTargetWidth - manaSpeed + 1))
            { manaBarCurrentWidth = manaBarTargetWidth; }
            Rectangle manaBarLength = new Rectangle(0, 0, manaBarCurrentWidth, (ManaBarFull.Height - 3));  //calculate rectangle to display bar in
            _spriteBatch.Draw(ManaBarFull, new Vector2(54, 452), manaBarLength, Color.White);

            //draw HP bar based on how much hp both sides have
            int hpAnimationSpeed = 4;
            int[] playerHP = player.getHP();
            int playerHPTargetWidth = HPBarFull.Width * playerHP[0] / playerHP[1];
            if (playerHPCurrentWidth > playerHPTargetWidth) { playerHPCurrentWidth -= hpAnimationSpeed; }    //decrease size of bar
            else if (playerHPCurrentWidth < playerHPTargetWidth) { playerHPCurrentWidth += hpAnimationSpeed; }   //increase size of bar
            if ((playerHPCurrentWidth <= playerHPTargetWidth + hpAnimationSpeed - 1) //edge case of really close
                && (playerHPCurrentWidth >= playerHPTargetWidth - hpAnimationSpeed + 1))
                { playerHPCurrentWidth = playerHPTargetWidth; }
            Rectangle playerHPBarLength = new Rectangle(0, 0, playerHPCurrentWidth, HPBarFull.Height);   //calculate rectangle to display bar in
            _spriteBatch.Draw(HPBarFull, new Vector2(54, 480), playerHPBarLength, Color.White);

            //same for enemy
            int[] enemyHP = enemy.getHP();
            int enemyHPTargetWidth = HPBarFull.Width * enemyHP[0] / enemyHP[1];
            if (enemyHPCurrentWidth > enemyHPTargetWidth) { enemyHPCurrentWidth -= hpAnimationSpeed; } //decrease size of bar
            else if (enemyHPCurrentWidth < enemyHPTargetWidth) { enemyHPCurrentWidth += hpAnimationSpeed; }  //increase size of bar
            if ((enemyHPCurrentWidth <= enemyHPTargetWidth + hpAnimationSpeed - 1)  //edge case
                && (enemyHPCurrentWidth >= enemyHPTargetWidth - hpAnimationSpeed + 1))
                { enemyHPCurrentWidth = enemyHPTargetWidth; }
            Rectangle enemyHPBarLength = new Rectangle(0, 0, enemyHPCurrentWidth, HPBarFull.Height);   //calculate rectangle to display bar in
            _spriteBatch.Draw(HPBarFull, new Vector2(836 + HPBarFull.Width - enemyHPCurrentWidth, 480), enemyHPBarLength, Color.White);

            //draw the textbox
            _spriteBatch.Draw(TextBox, new Vector2(0, 485), Color.White);

            if (DEBUG)
            {   //print out a little debug menu
                _spriteBatch.Draw(DebugBackground, new Vector2(0, 0), new Rectangle(0, 0, 300, 300), Color.White);
                _spriteBatch.DrawString(font, "Focused Area: " + focusedArea[0] + ", " + focusedArea[1], new Vector2(50, 25), Color.Red);
                _spriteBatch.DrawString(font, "Battle State: " + battleState, new Vector2(50, 50), Color.Red);
                _spriteBatch.DrawString(font, textToShow, new Vector2(50, 75), Color.Red);
                _spriteBatch.DrawString(font, "Start Time: " + startTime.TotalMilliseconds, new Vector2(50, 100), Color.Red);
                _spriteBatch.DrawString(font, "Elapsed Time: " + elapsedTime.TotalMilliseconds, new Vector2(50, 125), Color.Red);
                _spriteBatch.DrawString(font, "Mana: " + currentMana + "/" + maxMana, new Vector2(50, 150), Color.Red);
                _spriteBatch.DrawString(font, "Beat: " + beatNumber, new Vector2(50, 175), Color.Red);
                _spriteBatch.DrawString(font, "MS in Beat: " + msInBeat, new Vector2(50, 200), Color.Red);
                _spriteBatch.DrawString(font, "On Beat: " + onBeat, new Vector2(50, 225), Color.Red);
                _spriteBatch.DrawString(font, "Upper Bound: " + upperBound, new Vector2(50, 250), Color.Red);
                _spriteBatch.DrawString(font, "Lower Bound: " + lowerBound, new Vector2(50, 275), Color.Red);
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
                    //switch case to find the focused area
                    case 0:
                        fightColor = Color.CornflowerBlue;
                        break;
                    case 1:
                        spellsColor= Color.CornflowerBlue;
                        break;
                    case 2:
                        statsColor= Color.CornflowerBlue;
                        break;
                    case 3:
                        fleeColor = Color.CornflowerBlue;
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
                    "\nDefense: " + playerStats[2] + " + " + playerStats[3], new Vector2(500, 95), Color.CornflowerBlue);
                int[] enemyStats = player.getStats();
                _spriteBatch.DrawString(font, enemy.Name + "\n" + enemy.Description +
                    "\nAttack: ? + " + enemyStats[1] +
                    "\nDefense: ? + " + enemyStats[3], new Vector2(500, 195), Color.PaleVioletRed);
            }
            else if (battleState == 8)
            {
                //display the spells subscreen
                _spriteBatch.Draw(SubMenu, new Vector2(467, 67), Color.White);

                //get colors for text
                Color unfocusedColor = Color.White;
                Color focusedColor = Color.CornflowerBlue;
                Color textColor;

                for (int i = 0; i < numSpells; i++)  //loop thru each spell in the spell book
                {
                    if (focusedArea[1] == i) { textColor = focusedColor; }   //if else to determin focus
                    else { textColor = unfocusedColor; }
                    _spriteBatch.DrawString(font, spellbook[i]._name + ": " + spellbook[i]._manaCost + " Mana", new Vector2(500, 95 + i * 25), textColor);
                }
                if (focusedArea[1] == numSpells) { textColor = focusedColor; }   //if else to determine focus for cancel option
                else { textColor = unfocusedColor; }
                _spriteBatch.DrawString(font, "Cancel", new Vector2(500, 95 + numSpells * 25), textColor);
            }

            _spriteBatch.End();
        }

        private void drawKitKat(int xOffset, int yOffset)
        {
            //function code from June, used to draw the player's sprite
            // draw character base
            int[] charCustomization = CharacterCustom.getCustomization();
            switch (charCustomization[3])
            {
                case 1:
                    _spriteBatch.Draw(charBase, new Vector2(0 + xOffset, 0 + yOffset), Color.OrangeRed);
                    break;
                case 2:
                    _spriteBatch.Draw(charBase, new Vector2(0 + xOffset, 0 + yOffset), Color.HotPink);
                    break;
                case 3:
                    _spriteBatch.Draw(charBase, new Vector2(0 + xOffset, 0 + yOffset), Color.Coral);
                    break;
                case 4:
                    _spriteBatch.Draw(charBase, new Vector2(0 + xOffset, 0 + yOffset), Color.Blue);
                    break;
                default:
                    _spriteBatch.Draw(charBase, new Vector2(0 + xOffset, 0 + yOffset), Color.White);
                    break;
            }

            // draw head area sprites
            switch (charCustomization[0])
            {
                case 1:
                    _spriteBatch.Draw(charHead, new Vector2(263 + xOffset, 30 + yOffset), charHeadSource[1], Color.White);
                    break;
                case 2:
                    _spriteBatch.Draw(charHead, new Vector2(238 + xOffset, 125 + yOffset), charHeadSource[2], Color.White);
                    break;
                case 3:
                    _spriteBatch.Draw(charHead, new Vector2(278 + xOffset, 53 + yOffset), charHeadSource[3], Color.White);
                    break;
                default:
                    _spriteBatch.Draw(charHead, new Vector2(0 + xOffset, 0 + yOffset), charHeadSource[0], Color.White);
                    break;
            }

            // draw face area sprites
            _spriteBatch.Draw(charFace, new Vector2(228 + xOffset, 166 + yOffset), charFaceSource[charCustomization[1]], Color.White);

            // draw body area sprites
            switch (charCustomization[2])
            {
                case 1:
                    _spriteBatch.Draw(charBody, new Vector2(197 + xOffset, 364 + yOffset), charBodySource[1], Color.White);
                    break;
                case 2:
                    _spriteBatch.Draw(charBody, new Vector2(223 + xOffset, 364 + yOffset), charBodySource[2], Color.White);
                    break;
                case 3:
                    _spriteBatch.Draw(charBody, new Vector2(291 + xOffset, 364 + yOffset), charBodySource[3], Color.White);
                    break;
                default:
                    _spriteBatch.Draw(charBody, new Vector2(0 + xOffset, 0 + yOffset), charBodySource[0], Color.White);
                    break;
            }

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
                        CheckString.revertLost();
                        _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
                    }
                    else if (enemy.Name == "Grim Reaper")
                    {
                        CheckString.changeLostBoss();
                        _game.ChangeState(new EndState(_game, _graphicsDevice, _content));
                    }
                    else
                    {
                        CheckString.changeLostBoss();
                        _game.ChangeState(new StateBeforeBoss(_game, _graphicsDevice, _content));
                    }
                    break;
                case 6:
                    //just showed that the enemy died, go back to either the menu, story, or before boss state
                    battleMusicInstance.Stop();
                    if (returnToMenu)
                    {
                        CheckString.revertLost();
                        _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
                    }
                    else if (enemy.Name == "Dragon")
                    {
                        CheckString.revertLost();
                        _game.ChangeState(new StateBeforeBoss(_game, _graphicsDevice, _content));
                    }
                    else if (enemy.Name == "Grim Reaper")
                    {
                        CheckString.revertLost();
                        _game.ChangeState(new EndState(_game, _graphicsDevice, _content));
                    }
                    else
                    {
                        CheckString.revertLost();
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
                        Boolean sufficentMana = spellCasted._manaCost <= currentMana;
                        if (sufficentMana)
                        {
                            battleState = 9;
                            currentMana -= spellCasted._manaCost; //deplete the mana
                        }
                        else
                        {
                            battleState = 11;
                        }
                    }
                    break;
                case 9:
                    //just showed which spell the player casted, go to displaying the effects
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
                    SE_2.Play();
                    menuSize[0] = 4;
                    menuSize[1] = 1;
                    focusedArea[0] = 0;
                    focusedArea[1] = 0;
                    break;
                case 1:
                    //just displaying text
                    SE_2.Play();
                    menuSize[0] = 1;
                    menuSize[1] = 1;
                    textToShow = player.Name + " Attacks!";
                    break;
                case 2:
                    //displaying damage from player, need to calc damage
                    //also need to calc if player get mana or not
                    if(onBeat && (beatNumber == 4))
                    {
                        currentMana += 1;
                        if (currentMana > maxMana) { currentMana = maxMana; }
                        SE_5.Play();
                    }
                    else { SE_4.Play(); }
                    damageDelt = enemy.TakeDamage(player.DealDamage());
                    textToShow = CharacterCustom.getPronouns() + " deals " + damageDelt + " damage!";
                    break;
                case 3:
                    //enemy's turn, just displaying text that the enemy attacks
                    SE_2.Play();
                    textToShow = enemy.Name + " Attacks!";
                    break;
                case 4:
                    //displaying damage from enemy, need to calc it
                    SE_4.Play();
                    damageDelt = player.TakeDamage(enemy.DealDamage());
                    textToShow = enemy.Name + " deals " + damageDelt + " damage!";
                    break;
                case 5:
                    //just showing text that says player died
                    SE_2.Play();
                    textToShow = player.Name + " was defeated!";
                    break;
                case 6:
                    //just showing text that says enemy died
                    SE_2.Play();
                    textToShow = enemy.Name + " was defeated!";
                    break;
                case 7:
                    //showing the stats screen
                    SE_2.Play();
                    menuSize[0] = 1; //setting these because of a hack Imma do
                    menuSize[1] = 2; //I never did this hack
                    break;
                case 8:
                    //showing spell chosing screen
                    SE_2.Play();
                    menuSize[0] = 1;
                    menuSize[1] = numSpells + 1;
                    focusedArea[0] = 0;
                    focusedArea[1] = 0;
                    break;
                case 9:
                    //displaying text that shows which spell player casts
                    SE_2.Play();
                    menuSize[0] = 1;
                    menuSize[1] = 1;
                    textToShow = player.Name + " casts " + spellCasted._name + "!";
                    break;
                case 10:
                    //displaying the spell effect, need to actually cast the spell
                    SE_2.Play();
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
                    SE_2.Play();
                    menuSize[0] = 1;
                    menuSize[1] = 1;
                    textToShow = "Not Enough Mana!";
                    break;
                case 12:
                    //display that player tries to flee
                    SE_2.Play();
                    menuSize[0] = 1;
                    menuSize[1] = 1;
                    textToShow = player.Name + " tries to flee";
                    break;
                case 13:
                    //display that the player was successful in fleeing
                    SE_2.Play();
                    textToShow = player.Name + " fled";
                    break;
                case 14:
                    //display that the player failed to flee
                    SE_2.Play();
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
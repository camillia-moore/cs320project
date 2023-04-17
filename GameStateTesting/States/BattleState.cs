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
using Myra;
using Myra.Graphics2D.UI;
using GameStateTesting.BattleClasses;

namespace GameStateTesting.States
{
    public class BattleState : State
    {
        private Desktop _desktop;
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

        //graphics assets
        private Texture2D KitkatSprite;
        private Texture2D EnemySprite;
        private Texture2D TextBox;
        private Texture2D HPBarBase;
        private Texture2D HPBarFull;

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
        }

        public void createPlayer(String name, String description, int hp, int atk, int def, int mana)
        {
            //function for outside states to create stats for the player
            player = new Combatant(name, description, hp, atk, def);
        }

        public void createEnemy(String name, String description, int hp, int atk, int def)
        {
            //function for outside states to create stats for the enemy
            enemy = new Combatant(name, description,hp, atk, def);
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
            HPBarBase = _content.Load<Texture2D>("hp-bar-base");
            HPBarFull = _content.Load<Texture2D>("hp-bar-full");

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


            MyraEnvironment.Game = _game;
            

            var grid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8
            };

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

            // Button to go back to the Main Menu
            var buttonMenu = new TextButton
            {
                GridColumn = 0,
                GridRow = 0,
                Text = "Back to Menu"
            };

            buttonMenu.Click += (s, a) =>
            {
                Story.CheckString.MakeOriginalString(); //this is temperary
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            };

            grid.Widgets.Add(buttonMenu);

            // Button to Fight
            var buttonFight = new TextButton
            {
                GridColumn = 0,
                GridRow = 1,
                Text = "Fight"
            };

            buttonFight.Click += (s, a) =>
            {
                //code to handle damage
                int damageFromPlayer = player.DealDamage();
                int damageFromEnemy = enemy.DealDamage();
                int[] playerStats = player.getStats();
                int[] enemyStats = enemy.getStats();
                enemy.TakeDamage(damageFromPlayer);
                player.TakeDamage(damageFromEnemy);
                if (enemy.isDefeated())
                {
                    String messagePrinted = player.Name + " deals " + damageFromPlayer + " damage!\n" +
                                        enemy.Name + " deals " + damageFromEnemy + " damage!\n" +
                                        enemy.Name + " has defeated " + player.Name + "!\n";
                    var messageBox = Dialog.CreateMessageBox("Fight", messagePrinted);
                    messageBox.ShowModal(_desktop);
                    if(returnToMenu)
                    {
                        _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
                    }
                    else
                    {
                        _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
                    }
                }
                else if (player.isDefeated())
                {
                    String messagePrinted = player.Name + " deals " + damageFromPlayer + " damage!\n" +
                                        enemy.Name + " deals " + damageFromEnemy + " damage!\n" +
                                        player.Name + " has defeated  " + enemy.Name + "!\n";
                    var messageBox = Dialog.CreateMessageBox("Fight", messagePrinted);
                    messageBox.ShowModal(_desktop);
                    if (returnToMenu)
                    {

                        _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
                    }
                    else
                    {
                        _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
                    }
                }
                else
                {
                    String messagePrinted = player.Name + " deals " + damageFromPlayer + " damage!\n" +
                                        enemy.Name + " deals " + damageFromEnemy + " damage!\n";
                    var messageBox = Dialog.CreateMessageBox("Fight", messagePrinted);
                    messageBox.ShowModal(_desktop);
                }
            };

            grid.Widgets.Add(buttonFight);



            // Menu to Choose Spells

            var container = new VerticalStackPanel
            {
                GridColumn = 1,
                GridRow = 2,
                Spacing = 4
            };

            var titleContainer = new Panel
            {
                Background = DefaultAssets.UITextureRegionAtlas["button"],
            };

            var titleLabel = new Label
            {
                Text = "Spells",
                HorizontalAlignment= HorizontalAlignment.Center
            };

            titleContainer.Widgets.Add(titleLabel);
            container.Widgets.Add(titleContainer);

            var verticalMenu = new VerticalMenu();

            for(int i = 0; i < numSpells; i++)
            {
                verticalMenu.Items.Add(spellToMenuItem(spellbook[i]));
            }
            
            //verticalMenu.Items.Add(spellToMenuItem(fireball));
            //verticalMenu.Items.Add(spellToMenuItem(iceStorm));
            //verticalMenu.Items.Add(spellToMenuItem(diacute));
            //verticalMenu.Items.Add(spellToMenuItem(healing));

            container.Widgets.Add(verticalMenu);

            grid.Widgets.Add(container);


            // Button to View Stats
            var buttonStats = new TextButton
            {
                GridColumn = 0,
                GridRow = 3,
                Text = "Stats"
            };

            buttonStats.Click += (s, a) =>
            {
                int[] hp = player.getHP();
                int[] stats = player.getStats();
                int[] enemyHP = enemy.getHP();
                string toDisplay =  enemy.Name + " HP: " + enemyHP[0] + "/" + enemyHP[1] + "\n" +
                                    player.Name + " HP: " + hp[0] + "/" + hp[1] + "\n" +
                                    "Atk: " + stats[0] + " + " + stats[1] + "\n" +
                                    "Def: " + stats[2] + " + " + stats[3];
                var messageBox = Dialog.CreateMessageBox("Stats", toDisplay);
                messageBox.ShowModal(_desktop);
            };

            grid.Widgets.Add(buttonStats);


            // Button to Flee
            var buttonFlee = new TextButton
            {
                GridColumn = 0,
                GridRow = 4,
                Text = "Flee"
            };

            buttonFlee.Click += (s, a) =>
            {
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

                var messageBox = Dialog.CreateMessageBox("Flee", messagePrinted);
                messageBox.ShowModal(_desktop);
            };

            grid.Widgets.Add(buttonFlee);

            // Add it to the desktop
            _desktop = new Desktop();
            _desktop.Root = grid;
            

        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            //I'm not implementing keyboard controls rn

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
            int[] playerHP = player.getHP();
            Rectangle playerHPBarLength = new Rectangle(0, 0, HPBarFull.Width * playerHP[0] / playerHP[1], HPBarFull.Height);
            _spriteBatch.Draw(HPBarFull, new Vector2(54, 480), playerHPBarLength, Color.White);

            int[] enemyHP = enemy.getHP();
            //Rectangle enemyHPBarLength = new Rectangle(((HPBarFull.Width * (enemyHP[1] - enemyHP[0])) / enemyHP[1]) + 0, 0, HPBarFull.Width, HPBarFull.Height);
            Rectangle enemyHPBarLength = new Rectangle(0, 0, HPBarFull.Width * enemyHP[0] / enemyHP[1], HPBarFull.Height);
            _spriteBatch.Draw(HPBarFull, new Vector2(836 + (HPBarFull.Width * (enemyHP[1] - enemyHP[0]) / enemyHP[1]), 480), enemyHPBarLength, Color.White);

            _spriteBatch.Draw(TextBox, new Vector2(0, 485), Color.White);
            _spriteBatch.End();

            _desktop.Render();
        }

        private MenuItem spellToMenuItem(Spell spellToCast)
        {
            MenuItem menuSpell = new MenuItem
            {
                Text = spellToCast._name
            };
            menuSpell.Selected += (s, a) =>
            {
                int[] spellEffect = spellToCast.cast();
                string messagePrinted;
                if (spellEffect[3] == 0)
                {
                    //buff player
                    player.ModifyStats(spellEffect[0], spellEffect[1], spellEffect[2]);
                    messagePrinted = player.Name + " casts " + spellToCast._name + " on themselves!\n" + spellToCast._description;
                }
                else //spellEffect[3] == 1
                {
                    //nerf enemy
                    enemy.ModifyStats(spellEffect[0], spellEffect[1], spellEffect[2]);
                    messagePrinted = player.Name + " casts " + spellToCast._name + " on " + enemy.Name + "!\n" + spellToCast._description;
                }
                var messageBox = Dialog.CreateMessageBox("Spells", messagePrinted);
                messageBox.ShowModal(_desktop);
            };

            return menuSpell;
        }
    }
}
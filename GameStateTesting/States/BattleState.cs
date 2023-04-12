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
        private int playerHP = 30;
        private Combatant player;
        private Combatant enemy;
        private Spell fireball;
        private Spell iceStorm;
        private Spell diacute;
        private Spell healing;
        private Random rand;
        
        public BattleState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            //player = new Combatant("Kit-Kat", "The Default Hero", 30, 9, 5);
            enemy = new Combatant("Monster", "Generic Enemy", 20, 8, 4);
            fireball = new Spell("Fireball", "Deals damage to the opponent", new BattleClasses.Effect(-10, 0, 0, 1));
            iceStorm = new Spell("Ice Storm", "Uses Ice to Weaken the enemy", new BattleClasses.Effect(0, -2, -2, 1));
            diacute = new Spell("Diacute", "Buffs the user's stats", new BattleClasses.Effect(0, +2, +2, 0));
            healing = new Spell("Healing", "Heals the user", new BattleClasses.Effect(+5, 0, 0, 0));

            rand = new Random();
        }

        public void createPlayer(String name, String description, int hp, int atk, int def)
        {
            player = new Combatant(name, description, hp, atk, def);
        }

        public override void LoadContent()
        {
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
                    _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
                }
                else if (player.isDefeated())
                {
                    String messagePrinted = player.Name + " deals " + damageFromPlayer + " damage!\n" +
                                        enemy.Name + " deals " + damageFromEnemy + " damage!\n" +
                                        player.Name + " has defeated  " + enemy.Name + "!\n";
                    var messageBox = Dialog.CreateMessageBox("Fight", messagePrinted);
                    messageBox.ShowModal(_desktop);
                    _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
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
            
            verticalMenu.Items.Add(spellToMenuItem(fireball));
            verticalMenu.Items.Add(spellToMenuItem(iceStorm));
            verticalMenu.Items.Add(spellToMenuItem(diacute));
            verticalMenu.Items.Add(spellToMenuItem(healing));

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

                if (fleeChance > fleeSuccess) //flee was successful
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
            _graphicsDevice.Clear(Color.LightSlateGray);
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
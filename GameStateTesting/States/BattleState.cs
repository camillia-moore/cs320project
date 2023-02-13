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
        private Spell iceStorm;
        
        public BattleState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            player = new Combatant("KitKat", "The Default Hero", 30, 9, 5);
            enemy = new Combatant("Monster", "Generic Enemy", 20, 8, 4);
            iceStorm = new Spell("Ice Storm", "Uses Ice to Weaken the enemy", new BattleClasses.Effect(0, -2, -2, 1));
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
                /*var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
                messageBox.ShowModal(_desktop);*/
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
                String messagePrinted = player.Name + " deals " + damageFromPlayer + " damage!\n" +
                                        enemy.Name + " deals " + damageFromEnemy + " damage!\n";
                var messageBox = Dialog.CreateMessageBox("Fight", messagePrinted);
                messageBox.ShowModal(_desktop);
            };

            grid.Widgets.Add(buttonFight);

            // Button to Choose Spells
            var buttonSpells = new TextButton
            {
                GridColumn = 0,
                GridRow = 2,
                Text = "Spells"
            };

            buttonSpells.Click += (s, a) =>
            {
                Spell spellToCast = iceStorm;
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

            grid.Widgets.Add(buttonSpells);

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

            var playerHPLabel = new Label
            {
                GridColumn = 0,
                GridRow = 4,
                Id = "label",
                Text = "KitKat HP: " + playerHP + "/" + 30
            };
            grid.Widgets.Add(playerHPLabel);

            // Add it to the desktop
            _desktop = new Desktop();
            _desktop.Root = grid;


        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.P))
            {  //return back to the manu state
                //_game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
                player.TakeDamage(15);
            }

            /*if (kstate.IsKeyDown(Keys.Down))
            {
                _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                _game.ChangeState(new CharacterCreationState(_game, _graphicsDevice, _content));
            }*/

            /*if (kstate.IsKeyDown(Keys.Right))
            {
                _game.ChangeState(new BattleState(_game, _graphicsDevice, _content));
            }*/

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
            _graphicsDevice.Clear(Color.LightSlateGray);
            _desktop.Render();
        }
    }
}
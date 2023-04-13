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

namespace GameStateTesting.States
{
    public class MenuState : State
    {
        private Desktop _desktop;
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            
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

            // Button to Character Creation
            var buttonCCS = new TextButton
            {
                GridColumn = 0,
                GridRow = 0,
                Text = "Character Creation State"
            };

            buttonCCS.Click += (s, a) =>
            {
                /*var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
                messageBox.ShowModal(_desktop);*/
                _game.ChangeState(new CharacterCreationState(_game, _graphicsDevice, _content));
            };

            grid.Widgets.Add(buttonCCS);

            // Button to Story State
            var buttonSS = new TextButton
            {
                GridColumn = 0,
                GridRow = 1,
                Text = "Story State"
            };

            buttonSS.Click += (s, a) =>
            {
                /*var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
                messageBox.ShowModal(_desktop);*/
                _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
            };

            grid.Widgets.Add(buttonSS);

            // Button to Battle State
            var buttonBS = new TextButton
            {
                GridColumn = 0,
                GridRow = 2,
                Text = "Battle State"
            };

            buttonBS.Click += (s, a) =>
            {
                /*var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
                messageBox.ShowModal(_desktop);*/
                BattleState nextState = new BattleState(_game, _graphicsDevice, _content);
                nextState.createPlayer("Menu's KitKat", "The Default Hero", 30, 9, 5, 10);
                nextState.createEnemy("Spooky Monster", "Generic Enemy", 20, 8, 4);
                nextState.addSpell("Menu's Fireball", "Deals damage to the opponent", -10, 0, 0, 1, 3);
                nextState.addSpell("Ice Storm", "Uses Ice to Weaken the enemy", 0, -2, -2, 1, 4);
                nextState.addSpell("Diacute", "Buffs the user's stats", 0, +2, +2, 0, 5);
                nextState.addSpell("Menu's Healing", "Heals the user", +5, 0, 0, 0, 6);
                nextState.fromMenu(true);
                _game.ChangeState(nextState);

            };

            grid.Widgets.Add(buttonBS);

            // Add it to the desktop
            _desktop = new Desktop();
            _desktop.Root = grid;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            var kstate = Keyboard.GetState();

            /*if (kstate.IsKeyDown(Keys.Up))
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            }*/

            if (kstate.IsKeyDown(Keys.Down))
            {
                _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                _game.ChangeState(new CharacterCreationState(_game, _graphicsDevice, _content));
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                _game.ChangeState(new BattleState(_game, _graphicsDevice, _content));
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
            _graphicsDevice.Clear(Color.CornflowerBlue);
            _desktop.Render();
        }


    }
}

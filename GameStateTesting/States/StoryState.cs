using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;

namespace GameStateTesting.States
{


    public class StoryState : State
    {
        private Desktop _desktop;

        //Trying to create sprite boxes for the story to be printed.
        //private Rectangle[] PrintStory;
        //private Rectangle[] StoryGood;
        //private Rectangle[] StoryBad;

        SpriteFont TestFont;
        public StoryState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
        }

        public override void LoadContent()
        {
            //Console.WriteLine("This is a test.");
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
            var buttonGood = new TextButton
            {
                GridColumn = 0,
                GridRow = 8,
                Text = "Good Button Test"
            };

            buttonGood.Click += (s, a) =>
            {
                //currently sends back to main menu but temperary
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            };

            grid.Widgets.Add(buttonGood);

 /*           var buttonMid = new TextButton
            {
                GridColumn = 0,
                GridRow = 9,
                Text = "Neutral Button Test"
            };

            buttonMid.Click += (s, a) =>
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            };

            grid.Widgets.Add(buttonMid);*/


            var buttonBad = new TextButton
            {
                GridColumn = 0,
                GridRow = 9,
                Text = "Click me I'm bad!"
            };

            buttonBad.Click += (s, a) =>
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            };

            grid.Widgets.Add(buttonBad);


            _desktop = new Desktop();
            _desktop.Root = grid;

            TestFont = _content.Load<SpriteFont>("Fonts/TestFont");
        }


        //overide from story to get to the other states while testing.
        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
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
            _graphicsDevice.Clear(Color.LightGreen);

            //Draw test to the screen
            spriteBatch.Begin();
            spriteBatch.DrawString(TestFont, "Hello World!", new Vector2(0, 0), Color.Black);
            spriteBatch.End();

            _desktop.Render();
        }

    }
}

using GameStateTesting.Story;
using GameStateTesting.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace GameStateTesting.States
{
    public class StateBeforeBoss : State

    {
        private Desktop _desktop;

        SpriteFont TestFont; //create sprite for font
        public StateBeforeBoss(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
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
            var goToBattle = new TextButton
            {
                GridColumn = 0,
                GridRow = 8,
                Text = "The Boss is the way."
            };

            goToBattle.Click += (s, a) =>
            {
                BattleState nextState = new BattleState(_game, _graphicsDevice, _content);
                nextState.setEnemy(4);
                Story.CheckString.MakeZeroMonCount(); //This lowers the monster count to zero again because end game
                nextState.createPlayer("KitKat", "The Default Hero", 30, 9, 5, 10);
                nextState.addSpell("Fireball", "Deals damage to the opponent", -10, 0, 0, 1, 3);
                nextState.addSpell("Ice Storm", "Uses Ice to Weaken the enemy", 0, -2, -2, 1, 4);
                nextState.addSpell("Diacute", "Buffs the user's stats", 0, +2, +2, 0, 5);
                nextState.addSpell("Healing", "Heals the user", +5, 0, 0, 0, 6);

                nextState.buffPlayer(0, 0, 0, 0); ///dEPENDING ON WHATS PLACED IN HERE WILL BUFF THE 
                nextState.buffEnemy(0, 0, 0, 0);
                _game.ChangeState(nextState); 
            };
            grid.Widgets.Add(goToBattle);
        }

        public override void Update(GameTime gameTime)
        {
            //putting this in just for us to get through if need be.
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Up))
            {
                Story.CheckString.MakeOriginalString();
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            }
            if (kstate.IsKeyDown(Keys.Left))
            {
                Story.CheckString.MakeOriginalString();
                _game.ChangeState(new CharacterCreationState(_game, _graphicsDevice, _content));
            }
            if (kstate.IsKeyDown(Keys.Right))
            {
                Story.CheckString.MakeOriginalString();
                _game.ChangeState(new BattleState(_game, _graphicsDevice, _content));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Red);
            //List<Message> message = JsonUtility.GetJsonStringMessageFromJSON("Story/Part3.json");
            //Draw test to the screen
            spriteBatch.Begin();
            spriteBatch.DrawString(TestFont, text: $"The peak of the fight is here. Moving forward is a big fight.", new Vector2(50, 50), Color.Black); //draw the font 
            spriteBatch.DrawString(TestFont, text: $"The sickness has reached a peak. Get ready to fight for your life.", new Vector2(100, 50), Color.Black);
            //spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Bad}", new Vector2(100, 640), Color.Black);
            spriteBatch.End();
            _desktop.Render();

        }
    }
}

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

namespace GameStateTesting.States
{
    public class BattleState : State
    {
        public BattleState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
        }

        public override void LoadContent()
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                _game.ChangeState(new CharacterCreationState(_game, _graphicsDevice, _content));
            }

            /*if (kstate.IsKeyDown(Keys.Right))
            {
                _game.ChangeState(new BattleState(_game, _graphicsDevice, _content));
            }*/

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
            _graphicsDevice.Clear(Color.LightSlateGray);
        }
    }
}

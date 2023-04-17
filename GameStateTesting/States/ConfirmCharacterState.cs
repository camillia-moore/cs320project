using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using GameStateTesting.Customization;

namespace GameStateTesting.States
{
    public class ConfirmCharacterState : State
    {
        /*
        private Texture2D charBase;
        private Texture2D charHead;
        private Texture2D charFace;
        private Texture2D charBody;
        */

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private GraphicsDevice _graphicsDevice;
        public ConfirmCharacterState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, CharacterCustom customHero) : base(game, graphicsDevice, content)
        {
        }

        public override void LoadContent()
        {
            /*
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            charBase = _content.Load<Texture2D>("char-base");
            charHead = _content.Load<Texture2D>("char-head");
            charFace = _content.Load<Texture2D>("char-face");
            charBody = _content.Load<Texture2D>("char-body");
            */
        }

        public override void Update(GameTime gameTime)
        {
            var newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Back))
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);
        }
    }

}

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
    public class CharacterCreationState : State
    {
        private Texture2D gameTitle;
        private Texture2D nameInput;
        private Texture2D buttonContinue;
        private Texture2D selectionBox1;
        private Texture2D selectionBox2;
        private Texture2D selectionBox3;
        private Texture2D buttonArrow1;
        private Texture2D buttonArrow2;
        private Texture2D buttonArrow3;
        private Texture2D buttonArrow4;
        private Texture2D buttonArrow5;
        private Texture2D buttonArrow6;
        private Texture2D charBase;
        private Texture2D charHead;
        private Texture2D charFace;
        private Texture2D charBody;

        /**
         Image position references:
            arrow01 63, 44
            arrow02 607, 44
            arrow03 63, 205
            arrow04 607, 205
            arrow05 63, 362
            arrow06 607, 362

            title 781, 58
            nameinput 851, 433
            continue 784, 547

            bgbox01 35, 26
            bgbox02 35, 185
            bgbox03 35, 344
         */

        public CharacterCreationState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
        }

        public override void LoadContent()
        {
            bgElementsBatch = new SpriteBatch(GraphicsDevice);
            Vector2 bgPos = new Vector2();
            gameTitle = this._content.Load<Texture2D>("create-character-title");
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

            /*if (kstate.IsKeyDown(Keys.Left))
            {
                _game.ChangeState(new CharacterCreationState(_game, _graphicsDevice, _content));
            }*/

            if (kstate.IsKeyDown(Keys.Right))
            {
                _game.ChangeState(new BattleState(_game, _graphicsDevice, _content));
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
            _graphicsDevice.Clear(Color.LightPink);
        }
    }
}

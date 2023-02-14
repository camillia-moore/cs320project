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
        Texture2D charBase;
        Texture2D charTitle;
        Texture2D selectionBox1;
        Texture2D selectionBox2;
        Texture2D selectionBox3;
        Texture2D nameInputBox;
        Texture2D buttonContinue;
        Texture2D buttonArrow1;
        Texture2D buttonArrow2;
        Texture2D buttonArrow3;
        Texture2D buttonArrow4;
        Texture2D buttonArrow5;
        Texture2D buttonArrow6;
        Texture2D charHead;
        Texture2D charFace;
        Texture2D charBody;

        // sprite sheet source areas for each changeable image
        Rectangle[] selectionBoxSource;
        Rectangle[] arrowRightSource;
        Rectangle[] arrowLeftSource;
        Rectangle[] charHeadSource;
        Rectangle[] charFaceSource;
        Rectangle[] charBodySource;

        // cycles up/down between 0-5 for box1>box2>box3>name>pronouns>continue
        private int focusArea = 0;

        // cycles left/right in box1-3 for selecting custom parts
        private int headArea = 0;
        private int faceArea = 0;
        private int bodyArea = 0;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        public CharacterCreationState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            // load all image textures
            selectionBox1 = _content.Load<Texture2D>("selection-box");
            selectionBox2 = _content.Load<Texture2D>("selection-box");
            selectionBox3 = _content.Load<Texture2D>("selection-box");
            charBase = _content.Load<Texture2D>("char-base");
            charTitle = _content.Load<Texture2D>("create-character-title");
            nameInputBox = _content.Load<Texture2D>("input-name-box");
            buttonContinue = _content.Load<Texture2D>("button-continue-up");

            // load selection areas for sprite sheets
            selectionBoxSource = new Rectangle[2];
            selectionBoxSource[0] = new Rectangle(0, 0, 669, 138);
            selectionBoxSource[1] = new Rectangle(0, 139, 669, 138);
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
            {
                if (focusArea > 0)
                {
                    focusArea--;
                }
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                if (focusArea < 5)
                {
                    focusArea++;
                }
            }

            if (kstate.IsKeyDown(Keys.Right))
            {

            }

            if (kstate.IsKeyDown(Keys.Left))
            {

            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.LightPink);

            _spriteBatch.Begin();
            _spriteBatch.Draw(charTitle, new Vector2(781, 58), Color.White);
            _spriteBatch.Draw(nameInputBox, new Vector2(851, 433), Color.White);
            _spriteBatch.Draw(buttonContinue, new Vector2(784, 547), Color.White);

            // selection box color logic for all three boxes based on focusArea
            switch (focusArea)
            {
                case 0:
                    _spriteBatch.Draw(selectionBox1, new Vector2(35, 26), selectionBoxSource[1], Color.White);
                    _spriteBatch.Draw(selectionBox2, new Vector2(35, 185), selectionBoxSource[0], Color.White);
                    _spriteBatch.Draw(selectionBox3, new Vector2(35, 344), selectionBoxSource[0], Color.White);
                    break;
                case 1:
                    _spriteBatch.Draw(selectionBox1, new Vector2(35, 26), selectionBoxSource[0], Color.White);
                    _spriteBatch.Draw(selectionBox2, new Vector2(35, 185), selectionBoxSource[1], Color.White);
                    _spriteBatch.Draw(selectionBox3, new Vector2(35, 344), selectionBoxSource[0], Color.White);
                    break;
                case 3:
                    _spriteBatch.Draw(selectionBox1, new Vector2(35, 26), selectionBoxSource[0], Color.White);
                    _spriteBatch.Draw(selectionBox2, new Vector2(35, 185), selectionBoxSource[0], Color.White);
                    _spriteBatch.Draw(selectionBox3, new Vector2(35, 344), selectionBoxSource[1], Color.White);
                    break;
                default:
                    _spriteBatch.Draw(selectionBox1, new Vector2(35, 26), selectionBoxSource[0], Color.White);
                    _spriteBatch.Draw(selectionBox2, new Vector2(35, 185), selectionBoxSource[0], Color.White);
                    _spriteBatch.Draw(selectionBox3, new Vector2(35, 344), selectionBoxSource[0], Color.White);
                    break;
            }

            _spriteBatch.Draw(charBase, new Vector2(0,0), Color.White);
            _spriteBatch.End();
        }
    }
}

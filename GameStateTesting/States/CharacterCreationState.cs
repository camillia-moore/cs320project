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
using GameStateTesting.Customization;

namespace GameStateTesting.States
{
    public class CharacterCreationState : State
    {
        private Texture2D charBase;
        private Texture2D charTitle;
        private Texture2D selectionBox1;
        private Texture2D selectionBox2;
        private Texture2D selectionBox3;
        private Texture2D buttonContinue;
        private Texture2D buttonArrow1;
        private Texture2D buttonArrow2;
        private Texture2D buttonArrow3;
        private Texture2D buttonArrow4;
        private Texture2D buttonArrow5;
        private Texture2D buttonArrow6;
        private Texture2D charHead;
        private Texture2D charFace;
        private Texture2D charBody;

        // sprite sheet source areas for each changeable image
        private Rectangle[] selectionBoxSource;
        private Rectangle[] arrowLeftSource;
        private Rectangle[] arrowRightSource;
        private Rectangle[] pronounBoxSource;
        private Rectangle[] charHeadSource;
        private Rectangle[] charFaceSource;
        private Rectangle[] charBodySource;

        // cycles up/down between 0-5 for box1>box2>box3>pronouns>continue
        private int focusArea = 0;

        // cycles left/right in box1-3 for selecting custom parts
        private int headArea = 0;
        private int faceArea = 0;
        private int bodyArea = 0;

        // color palette swap , 0 is default, 1 is alt color
        private int baseAlt = 0;
        private int headAlt = 0;
        private int faceAlt = 0;
        private int bodyAlt = 0;

        private int pronounsArea = 0;

        // used to add delay to keypresses
        private KeyboardState oldState;

        private bool isLeftArrowDown = false;
        private bool isRightArrowDown = false;

        private string entryPronouns;

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
            charTitle = _content.Load<Texture2D>("create-character-title");
            buttonContinue = _content.Load<Texture2D>("button-continue-up");
            buttonArrow1 = _content.Load<Texture2D>("arrow-left");
            buttonArrow2 = _content.Load<Texture2D>("arrow-right");
            buttonArrow3 = _content.Load<Texture2D>("arrow-left");
            buttonArrow4 = _content.Load<Texture2D>("arrow-right");
            buttonArrow5 = _content.Load<Texture2D>("arrow-left");
            buttonArrow6 = _content.Load<Texture2D>("arrow-right");
            charBase = _content.Load<Texture2D>("char-base");
            charHead = _content.Load<Texture2D>("char-head");
            charFace = _content.Load<Texture2D>("char-face");
            charBody = _content.Load<Texture2D>("char-body");

            // load selection areas for sprite sheets
            selectionBoxSource = new Rectangle[2];
            selectionBoxSource[0] = new Rectangle(0, 0, 669, 138); // selection box unfocused (default)
            selectionBoxSource[1] = new Rectangle(0, 139, 669, 138); // selection box focused

            arrowLeftSource = new Rectangle[3];
            arrowLeftSource[0] = new Rectangle(137, 0, 68, 100); // left arrow default/up
            arrowLeftSource[1] = new Rectangle(69, 0, 68, 100); // left arrow hover
            arrowLeftSource[2] = new Rectangle(0, 0, 68, 100); // left arrow down

            arrowRightSource= new Rectangle[3];
            arrowRightSource[0] = new Rectangle(0, 0, 68, 100); // right arrow default/up
            arrowRightSource[1] = new Rectangle(69, 0, 68, 100); // right arrow hover
            arrowRightSource[2] = new Rectangle(137, 0, 68, 100); // right arrow down

            charHeadSource = new Rectangle[4];
            charHeadSource[0] = new Rectangle(0, 0, 1, 1);
            charHeadSource[1] = new Rectangle(0, 0, 183, 133);
            charHeadSource[2] = new Rectangle(233, 46, 123, 103);
            charHeadSource[3] = new Rectangle(0, 206, 199, 144);

            charFaceSource = new Rectangle[4];
            charFaceSource[0] = new Rectangle(0, 0, 287, 205);
            charFaceSource[1] = new Rectangle(288, 0, 287, 205);
            charFaceSource[2] = new Rectangle(0, 206, 287, 205);
            charFaceSource[3] = new Rectangle(288, 206, 287, 205);

            charBodySource = new Rectangle[4];
            charBodySource[0] = new Rectangle(0, 0, 1, 1);
            charBodySource[1] = new Rectangle(0, 0, 364, 322);
            charBodySource[2] = new Rectangle(480, 0, 307, 243);
            charBodySource[3] = new Rectangle(98, 398, 198, 227);

            pronounBoxSource = new Rectangle[4];
            pronounBoxSource[0] = new Rectangle();
            pronounBoxSource[1] = new Rectangle();
            pronounBoxSource[2] = new Rectangle();
            pronounBoxSource[3] = new Rectangle();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add support for mouse states in conjunction with keyboard states
            var newState = Keyboard.GetState();

            // controls left arrow color change when keyboard is pressed down
            if (focusArea < 3 && newState.IsKeyDown(Keys.Left))
            {
                isLeftArrowDown= true;
            } else
            {
                isLeftArrowDown= false;
            }

            // controls right arrow color change when keyboard is pressed down
            if (focusArea < 3 && newState.IsKeyDown(Keys.Right))
            {
                isRightArrowDown = true;
            }
            else
            {
                isRightArrowDown= false;
            }

            // changes focus area to previous when up is pressed
            if (oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up))
            {
                if (focusArea > 0)
                {
                    focusArea--;
                }
            }

            // changes focus area to next when down is pressed
            if (oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down))
            {
                if (focusArea < 5)
                {
                    focusArea++;
                }
            }

            // controls left cycle for each body part
            if (oldState.IsKeyUp(Keys.Left) && newState.IsKeyDown(Keys.Left))
            {
                switch (focusArea)
                {
                    case 0:
                        headArea--;
                        if (headArea < 0)
                        {
                            headArea = 3;
                        }
                        break;
                    case 1:
                        faceArea--;
                        if (faceArea < 0)
                        {
                            faceArea = 3;
                        }
                        break;
                    case 2:
                        bodyArea--;
                        if (bodyArea < 0)
                        {
                            bodyArea = 3;
                        }
                        break;
                    case 3:
                        pronounsArea--;
                        if (pronounsArea < 0)
                        {
                            pronounsArea = 2;
                        }
                        break;
                    default:
                        break;
                }

            }

            // controls right cycle for each body part
            if (oldState.IsKeyUp(Keys.Right) && newState.IsKeyDown(Keys.Right))
            {
                switch(focusArea)
                {
                    case 0:
                        headArea++;
                        if (headArea > 3)
                        {
                            headArea = 0;
                        }
                        break;
                    case 1:
                        faceArea++;
                        if (faceArea > 3)
                        {
                            faceArea = 0;
                        }
                        break;
                    case 2:
                        bodyArea++;
                        if (bodyArea > 3)
                        {
                            bodyArea = 0;
                        }
                        break;
                    case 3:
                        pronounsArea++;
                        if (pronounsArea > 2)
                        {
                            pronounsArea = 0;
                        }
                        break;
                    default:
                        break;
                }
            }

            // move to next story screen; saves customization screen input then changes screen state
            if (focusArea > 3)
            {
                if (newState.IsKeyDown(Keys.Enter) || newState.IsKeyDown(Keys.Space))
                {
                    entryPronouns = "they";
                    _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
                }
            }

            // go back to the main menu screen
            if (newState.IsKeyDown(Keys.Back))
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            }

            // change oldState to newState to allow reaction only on first key press
            oldState = newState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.LightPink);

            _spriteBatch.Begin();
            _spriteBatch.Draw(charTitle, new Vector2(781, 58), Color.White);

            // draw pronoun selection box components

            // draw continue button
            if (focusArea == 4)
                _spriteBatch.Draw(buttonContinue, new Vector2(784, 547), Color.GreenYellow);
            else
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
                case 2:
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

            // draw character base
            _spriteBatch.Draw(charBase, new Vector2(0,0), Color.White);

            // draw head area sprites
            switch (headArea)
            {
                case 1:
                    _spriteBatch.Draw(charHead, new Vector2(263, 30), charHeadSource[1], Color.White);
                    break;
                case 2:
                    _spriteBatch.Draw(charHead, new Vector2(238, 125), charHeadSource[2], Color.White);
                    break;
                case 3:
                    _spriteBatch.Draw(charHead, new Vector2(278, 53), charHeadSource[3], Color.White);
                    break;
                default:
                    _spriteBatch.Draw(charHead, new Vector2(0, 0), charHeadSource[0], Color.White);
                    break;
            }

            // draw face area sprites
            _spriteBatch.Draw(charFace, new Vector2(228, 166), charFaceSource[faceArea], Color.White);

            // draw body area sprites
            switch (bodyArea)
            {
                case 1:
                    _spriteBatch.Draw(charBody, new Vector2(197, 364), charBodySource[1], Color.White);
                    break;
                case 2:
                    _spriteBatch.Draw(charBody, new Vector2(223, 364), charBodySource[2], Color.White);
                    break;
                case 3:
                    _spriteBatch.Draw(charBody, new Vector2(291, 364), charBodySource[3], Color.White);
                    break;
                default:
                    _spriteBatch.Draw(charBody, new Vector2(0, 0), charBodySource[0], Color.White);
                    break;
            }

            // draw all the base arrows
            _spriteBatch.Draw(buttonArrow1, new Vector2(63, 44), arrowLeftSource[0], Color.White);
            _spriteBatch.Draw(buttonArrow2, new Vector2(607, 44), arrowRightSource[0], Color.White);
            _spriteBatch.Draw(buttonArrow3, new Vector2(63, 205), arrowLeftSource[0], Color.White);
            _spriteBatch.Draw(buttonArrow4, new Vector2(607, 205), arrowRightSource[0], Color.White);
            _spriteBatch.Draw(buttonArrow5, new Vector2(63, 362), arrowLeftSource[0], Color.White);
            _spriteBatch.Draw(buttonArrow6, new Vector2(607, 362), arrowRightSource[0], Color.White);

            // change the arrow color based on focusArea and key press state
            if (isLeftArrowDown)
            {
                if (focusArea == 0)
                {
                    _spriteBatch.Draw(buttonArrow1, new Vector2(63, 44), arrowLeftSource[2], Color.White);
                }
                if (focusArea == 1)
                {
                    _spriteBatch.Draw(buttonArrow3, new Vector2(63, 205), arrowLeftSource[2], Color.White);
                }
                if (focusArea == 2)
                {
                    _spriteBatch.Draw(buttonArrow5, new Vector2(63, 362), arrowLeftSource[2], Color.White);
                }
            }

            if (isRightArrowDown)
            {
                if (focusArea == 0)
                {
                    _spriteBatch.Draw(buttonArrow2, new Vector2(607, 44), arrowRightSource[2], Color.White);
                }
                if (focusArea == 1)
                {
                    _spriteBatch.Draw(buttonArrow4, new Vector2(607, 205), arrowRightSource[2], Color.White);
                }
                if (focusArea == 2)
                {
                    _spriteBatch.Draw(buttonArrow6, new Vector2(607, 362), arrowRightSource[2], Color.White);
                }
            }

            _spriteBatch.End();
        }
    }
}

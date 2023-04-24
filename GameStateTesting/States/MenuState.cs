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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameStateTesting.States
{
    public class MenuState : State
    {
        //loading main variables and classes
        private Texture2D titleScreen;
        private Texture2D titleArrow;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private int optionFocused;
        private bool isOptionFocused;
        private KeyboardState oldKstate;

        private Rectangle arrowSource;

        //loading clickable areas
        private Rectangle charSceneClickable = new Rectangle(880, 236, 364, 67);
        private Rectangle storySceneClickable = new Rectangle(880, 311, 364, 67);
        private Rectangle battleSceneClickable = new Rectangle(880, 387, 364, 67);

        //audio vars
        private SoundEffectInstance titleMusicInstance;
        private Boolean MUTEAUDIO = false;
        private SoundEffect SE_1;
        private SoundEffect SE_2;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            optionFocused = 0; //set option to first option
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            titleScreen = _content.Load<Texture2D>("cough-story-title");  //load main screen
            titleArrow = _content.Load<Texture2D>("pronouns-sheet"); //load arrow sprite
            arrowSource = new Rectangle(464, 168, 36, 52);
            font = _content.Load<SpriteFont>("TestFont");

            //load audio
            SoundEffect titleMusic = _content.Load<SoundEffect>("cs_project_thing_4_looped");
            titleMusicInstance = titleMusic.CreateInstance();
            titleMusicInstance.IsLooped = true;
            if (!MUTEAUDIO) { titleMusicInstance.Play(); }

            //load sound effects
            SE_1 = _content.Load<SoundEffect>("SE-1");
            SE_2 = _content.Load<SoundEffect>("SE-2");

        }

        public override void Update(GameTime gameTime)
        {
            var newKstate = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            isOptionFocused = false;

            //swap between options
            if (newKstate.IsKeyDown(Keys.Down) && oldKstate.IsKeyUp(Keys.Down))
            {
                optionFocused += 1;
                SE_1.Play();
            }
            if (newKstate.IsKeyDown(Keys.Up) && oldKstate.IsKeyUp(Keys.Up))
            {
                optionFocused += -1;
                SE_1.Play();
            }

            //roll over
            if (optionFocused == 3) { optionFocused = 0; }
            if (optionFocused == -1) { optionFocused = 2; }

            //mouse-over option select
            if (charSceneClickable.Contains(mousePosition))
            {
                isOptionFocused = true;
                optionFocused = 0;
            }
            if (storySceneClickable.Contains(mousePosition))
            {
                isOptionFocused = true;
                optionFocused = 1;
            }
            if (battleSceneClickable.Contains(mousePosition))
            {
                isOptionFocused= true;
                optionFocused = 2;
            }

            //enter the selected state
            if (newKstate.IsKeyDown(Keys.Enter) || newKstate.IsKeyDown(Keys.Space) || (mouseState.LeftButton == ButtonState.Pressed && isOptionFocused == true)) 
            {
                //stop music
                titleMusicInstance.Stop();
                SE_2.Play();
                switch (optionFocused)
                {
                    case 0:
                        //go to character creation state
                        Story.CheckString.MakeOriginalString();   //reset story progress
                        _game.ChangeState(new CharacterCreationState(_game, _graphicsDevice, _content));
                        break;
                    case 1:
                        //go to story state
                        Story.CheckString.MakeOriginalString();   //reset story progress
                        _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
                        break;
                    case 2:
                        //go to battle state
                        BattleState nextState = new BattleState(_game, _graphicsDevice, _content);
                        nextState.createPlayer("Menu's KitKat", "The Menu's Default Hero", 30, 9, 5, 10); //set up custom name for player
                        nextState.fromMenu(true); //specify that we came from the menu
                        //then swap to the battle
                        _game.ChangeState(nextState);
                        break;
                    default:
                        //do nothing
                        break;
                }
            }

            oldKstate = newKstate;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //clear screen
            _graphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //draw background
            _spriteBatch.Draw(titleScreen, new Vector2(0, 0), Color.White);

            //find the selected option
            switch (optionFocused)
            {
                case 0:
                    _spriteBatch.Draw(titleArrow, new Vector2(902, 247), arrowSource, Color.White);
                    break;
                case 1:
                    _spriteBatch.Draw(titleArrow, new Vector2(902, 322), arrowSource, Color.White);
                    break;
                case 2:
                    _spriteBatch.Draw(titleArrow, new Vector2(902, 393), arrowSource, Color.White);
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

        }
    }
}

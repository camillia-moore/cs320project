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
        private Texture2D titleScreen;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private int optionFocused;
        private KeyboardState oldKstate;

        //audio
        private SoundEffectInstance titleMusicInstance;
        private Boolean MUTEAUDIO = false;
        private SoundEffect SE_1;
        private SoundEffect SE_2;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            optionFocused = 0;
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            titleScreen = _content.Load<Texture2D>("cough-story-title-draft");
            font = _content.Load<SpriteFont>("TestFont");

            //audio
            SoundEffect titleMusic = _content.Load<SoundEffect>("cs_project_thing_4_looped");
            titleMusicInstance = titleMusic.CreateInstance();
            titleMusicInstance.IsLooped = true;
            if (!MUTEAUDIO) { titleMusicInstance.Play(); }

            SE_1 = _content.Load<SoundEffect>("SE-1");
            SE_2 = _content.Load<SoundEffect>("SE-2");

        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            var newKstate = Keyboard.GetState();

            if(newKstate.IsKeyDown(Keys.Down) && oldKstate.IsKeyUp(Keys.Down))
            {
                optionFocused += 1;
                SE_1.Play();
            }
            if (newKstate.IsKeyDown(Keys.Up) && oldKstate.IsKeyUp(Keys.Up))
            {
                optionFocused += -1;
                SE_1.Play();
            }
            if (optionFocused == 3) { optionFocused = 0; }
            if (optionFocused == -1) { optionFocused = 2; }
            if (newKstate.IsKeyDown(Keys.Enter) || newKstate.IsKeyDown(Keys.Space)) 
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
                        nextState.createPlayer("Menu's KitKat", "The Default Hero", 30, 9, 5, 10);
                        //nextState.buffPlayer(+0, +1, +0, +0); //should buf attack by 1
                        nextState.fromMenu(true);//then swap to the battle
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
            //throw new NotImplementedException();
            _graphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(titleScreen, new Vector2(0, 0), Color.White);

            //draw the options
            Color CCS = Color.Black;
            Color SS = Color.Black;
            Color BS = Color.Black;
            switch (optionFocused)
            {
                case 0:
                    CCS = Color.Red;
                    break;
                case 1:
                    SS = Color.Red;
                    break;
                case 2:
                    BS = Color.Red;
                    break;
                default:
                    break;
            }
            _spriteBatch.DrawString(font, "Character Creation Scene", new Vector2(920, 250), CCS);
            _spriteBatch.DrawString(font, "Story Scene", new Vector2(920, 300), SS);
            _spriteBatch.DrawString(font, "Battle Scene", new Vector2(920, 350), BS);

            _spriteBatch.End();

        }
    }
}

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
    public class MenuState : State
    {
        private Texture2D titleScreen;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private int optionFocused;
        private KeyboardState oldKstate;
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            optionFocused = 0;
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            titleScreen = _content.Load<Texture2D>("cough-story-title-draft");
            font = _content.Load<SpriteFont>("TestFont");
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            var newKstate = Keyboard.GetState();

            if(newKstate.IsKeyDown(Keys.Down) && oldKstate.IsKeyUp(Keys.Down))
            {
                optionFocused += 1;
            }
            if (newKstate.IsKeyDown(Keys.Up) && oldKstate.IsKeyUp(Keys.Up))
            {
                optionFocused += -1;
            }
            if (optionFocused == 3) { optionFocused = 0; }
            if (optionFocused == -1) { optionFocused = 2; }
            if (newKstate.IsKeyDown(Keys.Enter) || newKstate.IsKeyDown(Keys.Space)) 
            {
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
                        nextState.addSpell("Menu's Fireball", "Deals damage to the opponent", -10, 0, 0, 1, 3);
                        nextState.addSpell("Ice Storm", "Uses Ice to Weaken the enemy", 0, -2, -2, 1, 4);
                        nextState.addSpell("Diacute", "Buffs the user's stats", 0, +2, +2, 0, 5);
                        nextState.addSpell("Menu's Healing", "Heals the user", +5, 0, 0, 0, 6);
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

using GameStateTesting.Story;
using GameStateTesting.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System.Collections.Generic;
using System.Linq;

namespace GameStateTesting.States
{


    public class StoryState : State
    {
        private Desktop _desktop;

        SpriteFont TestFont; //create sprite for font
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
                Text = "Click me!"
            };

            buttonGood.Click += (s, a) =>
            {
                //This no longer just sends to menue state. will send back to story or battle
                string isA = "A";
                string placeinstoryA = Story.CheckString.ChangeString(isA);
                int ourStringLenA = placeinstoryA.Length;
                if ((ourStringLenA == 3) || (ourStringLenA == 5) || (ourStringLenA == 7))
                {
                    //_game.ChangeState(new BattleState(_game, _graphicsDevice, _content));
                    BattleState nextState = new BattleState(_game, _graphicsDevice, _content);
                    if (ourStringLenA == 3)
                    {
                        nextState.setEnemy(1);
                        Story.CheckString.increaseMonsterCount(); //This increases every time we fight a new baddy. At three we fight the boss.
                    }
                    if (ourStringLenA == 5)
                    {
                        nextState.setEnemy(2);
                        Story.CheckString.increaseMonsterCount();
                    }
                    if (ourStringLenA == 7)
                    {
                        nextState.setEnemy(3);
                        Story.CheckString.increaseMonsterCount();
                    }
                    nextState.createPlayer("KitKat", "The Default Hero", 30, 9, 5, 10);
                    nextState.addSpell("Fireball", "Deals damage to the opponent", -10, 0, 0, 1, 3);
                    nextState.addSpell("Ice Storm", "Uses Ice to Weaken the enemy", 0, -2, -2, 1, 4);
                    nextState.addSpell("Diacute", "Buffs the user's stats", 0, +2, +2, 0, 5);
                    nextState.addSpell("Healing", "Heals the user", +5, 0, 0, 0, 6);
                    int Pbuff = Story.CheckString.returnbuffcountA(placeinstoryA);
                    int EBuff = Story.CheckString.returnbuffcountB(placeinstoryA);
                    nextState.buffPlayer(Pbuff, Pbuff, Pbuff, 0); ///dEPENDING ON WHATS PLACED IN HERE WILL BUFF THE 
                    nextState.buffEnemy(EBuff, EBuff, EBuff, 0); //ap//att//df//last one not used.
                    _game.ChangeState(nextState);
                }
                else
                {
                    _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
                }
            };

            grid.Widgets.Add(buttonGood);

            var buttonBad = new TextButton
            {
                GridColumn = 0,
                GridRow = 9,
                Text = "Click me!"
            };

            buttonBad.Click += (s, a) =>
            {
                //No longer goes to just the menu state
                string isB = "B";
                string placeinstoryB = Story.CheckString.ChangeString(isB);
                int ourStringLenB = placeinstoryB.Length;
                if ((ourStringLenB == 3) ||  (ourStringLenB == 5) || (ourStringLenB == 7))
                {
                    //_game.ChangeState(new BattleState(_game, _graphicsDevice, _content));
                    BattleState nextState = new BattleState(_game, _graphicsDevice, _content);
                    if(ourStringLenB == 3)
                    {
                        nextState.setEnemy(1);
                    }
                    if(ourStringLenB == 5)
                    {
                        nextState.setEnemy(2);
                    }
                    if(ourStringLenB == 7)
                    {
                        nextState.setEnemy(3);
                    }
                    nextState.createPlayer("KitKat", "The Default Hero", 30, 9, 5, 10);
                    nextState.addSpell("Fireball", "Deals damage to the opponent", -10, 0, 0, 1, 3);
                    nextState.addSpell("Ice Storm", "Uses Ice to Weaken the enemy", 0, -2, -2, 1, 4);
                    nextState.addSpell("Diacute", "Buffs the user's stats", 0, +2, +2, 0, 5);
                    nextState.addSpell("Healing", "Heals the user", +5, 0, 0, 0, 6);
                    int Pbuff = Story.CheckString.returnbuffcountA(placeinstoryB);
                    int EBuff = Story.CheckString.returnbuffcountB(placeinstoryB);
                    nextState.buffPlayer(Pbuff,Pbuff, Pbuff, 0); ///dEPENDING ON WHATS PLACED IN HERE WILL BUFF THE 
                    nextState.buffEnemy(EBuff,EBuff,EBuff, 0); //ap//att//df//last one not used.
                    //last number relevant to spells
                    _game.ChangeState(nextState);
                }
                else
                {
                    _game.ChangeState(new StoryState(_game, _graphicsDevice, _content));
                }


            };

            grid.Widgets.Add(buttonBad);

            _desktop = new Desktop();
            _desktop.Root = grid;

            TestFont = _content.Load<SpriteFont>("Fonts/TestFont"); //load the font into the current content manager
        }


        //overide from story to get to the other states while testing.
        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
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
            _graphicsDevice.Clear(Color.LightGreen);


            string placeinstory = Story.CheckString.StoryCheckString();
            int lengthOfPlace = placeinstory.Length;

            //string fileIncoming = "Part1.json"; //This doesn't work no matter what the Part1.json reads null

            if (lengthOfPlace < 3) //so up to two in length to access the part1.json
            {
                List<Message> message = JsonUtility.GetJsonStringMessageFromJSON("Story/Part1.json");
                //Draw test to the screen
                spriteBatch.Begin();
                //!!!!!!!!!!!!M.id == WILL EVENTUALLY BE THE CODE THAT COMES IN AFTER CHANGE OF id

                spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Story}", new Vector2(0, 0), Color.Black); //draw the font 
                //Draw good string
                spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Good}", new Vector2(100, 550), Color.Black);
                //Draw bad string
                spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Bad}", new Vector2(100, 640), Color.Black);
                spriteBatch.End();
            }
            //Should grab from only the part2.json
            if ((lengthOfPlace >= 3) && (lengthOfPlace < 5))
            {
                List<Message> message = JsonUtility.GetJsonStringMessageFromJSON("Story/Part2.json");
                //Draw test to the screen
                spriteBatch.Begin();
                //!!!!!!!!!!!!M.id == WILL EVENTUALLY BE THE CODE THAT COMES IN AFTER CHANGE OF id
                spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Story}", new Vector2(0, 0), Color.Black); //draw the font 
                //Draw good string
                spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Good}", new Vector2(100, 550), Color.Black);
                //Draw bad string
                spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Bad}", new Vector2(100, 640), Color.Black);
                spriteBatch.End();
            }
            if ((lengthOfPlace >= 5) && (lengthOfPlace < 7))
            {
                List<Message> message = JsonUtility.GetJsonStringMessageFromJSON("Story/Part3.json");
                //Draw test to the screen
                spriteBatch.Begin();
                //!!!!!!!!!!!!M.id == WILL EVENTUALLY BE THE CODE THAT COMES IN AFTER CHANGE OF id
                spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Story}", new Vector2(0, 0), Color.Black); //draw the font 
                //Draw good string
                spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Good}", new Vector2(100, 550), Color.Black);
                //Draw bad string
                spriteBatch.DrawString(TestFont, text: $"{message.First(m => m.Id == placeinstory).Bad}", new Vector2(100, 640), Color.Black);
                spriteBatch.End();
            }

            //spriteBatch.End();

            _desktop.Render();
        }
    }
}

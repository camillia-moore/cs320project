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


    public class StoryState : State
    {
        private Desktop _desktop;

        SpriteFont TestFont; //create sprite for font
        public StoryState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            //private string idStringToChange = "X"; 
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
                int ourStringLen = placeinstoryA.Length;
                if ((ourStringLen == 3) || (ourStringLen == 5) || (ourStringLen == 7))
                {
                    //_game.ChangeState(new BattleState(_game, _graphicsDevice, _content));
                    BattleState nextState = new BattleState(_game, _graphicsDevice, _content);
                    nextState.createPlayer("KitKat", "The Default Hero", 30, 9, 5, 10);
                    nextState.createEnemy("Spooky Monster", "Generic Enemy", 20, 8, 4);
                    nextState.addSpell("Fireball", "Deals damage to the opponent", -10, 0, 0, 1, 3);
                    nextState.addSpell("Ice Storm", "Uses Ice to Weaken the enemy", 0, -2, -2, 1, 4);
                    nextState.addSpell("Diacute", "Buffs the user's stats", 0, +2, +2, 0, 5);
                    nextState.addSpell("Healing", "Heals the user", +5, 0, 0, 0, 6);
                    nextState.buffPlayer(0, 0, 0, 0); ///dEPENDING ON WHATS PLACED IN HERE WILL BUFF THE 
                    nextState.buffEnemy(0, 0, 0, 0);
                    nextState.fromMenu(true);
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
                int ourStringLen = placeinstoryB.Length;
                if ((ourStringLen == 3) ||  (ourStringLen == 5) || (ourStringLen == 7))
                {
                    //_game.ChangeState(new BattleState(_game, _graphicsDevice, _content));
                    BattleState nextState = new BattleState(_game, _graphicsDevice, _content);
                    nextState.createPlayer("KitKat", "The Default Hero", 30, 9, 5, 10);
                    nextState.createEnemy("Spooky Monster", "Generic Enemy", 20, 8, 4);
                    nextState.addSpell("Fireball", "Deals damage to the opponent", -10, 0, 0, 1, 3);
                    nextState.addSpell("Ice Storm", "Uses Ice to Weaken the enemy", 0, -2, -2, 1, 4);
                    nextState.addSpell("Diacute", "Buffs the user's stats", 0, +2, +2, 0, 5);
                    nextState.addSpell("Healing", "Heals the user", +5, 0, 0, 0, 6);
                    nextState.buffPlayer(0, 0, 0, 0); ///dEPENDING ON WHATS PLACED IN HERE WILL BUFF THE 
                    nextState.buffEnemy(0,0,0, 0);
                    //last number relevant to spells
                    nextState.fromMenu(true);
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
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            }
            if (kstate.IsKeyDown(Keys.Left))
            {
                _game.ChangeState(new CharacterCreationState(_game, _graphicsDevice, _content));
            }
            if (kstate.IsKeyDown(Keys.Right))
            {
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

            



            ///Just shit code that I want to reuse.
            ///
                        //doesn't work
            //string fileIncoming = "Story/Part1.json";
            //string fileIncoming = "C:\Users/Lyndsey/Documents/GitHub/cs320project/GameStateTesting/Story/Part1.json";

            /*Doesn't work
             * string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\Story\Part1.json");
            string fileIncoming = Path.GetFullPath(sFile);*/

            //string fileIncoming = Path.GetFullPath("Part1.json"); //THis doesn't work. This makes it null

            //string jsonstring = File.ReadAllText(@"./Story/Part1.json"); //This doesn't work. IT still null
            //string jsonstring = File.ReadAllText(fileIncoming); //This doesn't work

            /*try   I need to read the whole file not line by line
            {
                var path = "Part1.txt";
                using (StreamReader aa = new StreamReader(path))
                {
                    string[] line;
                    while ((line = aa.ReadAllLines()) != null)
                    {
                        Console.
                    }
                }
            }catch(Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }*/



            ////
            //string jsonstring = string.Empty; //not initialized in memory
            //string jsonstring = "";// this one is.
            /*string jsonstring = @"{
                                   ""Id"" : 17,
                                     ""MessageDescription"" : ""Hello World!""
                                        }";*/
            //string fileIncoming = "Part1.json";
            // string jsonstring = File.ReadAllText(fileIncoming);
            // Message? message = JsonSerializer.Deserialize<Message>(jsonstring);
        }



    }
}

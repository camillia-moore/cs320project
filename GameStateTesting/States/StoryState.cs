﻿using GameStateTesting.Story;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.IO;
using System.Text.Json;

namespace GameStateTesting.States
{


    public class StoryState : State
    {
        private Desktop _desktop;

        //Trying to create sprite boxes for the story to be printed.
        //private Rectangle[] PrintStory;
        //private Rectangle[] StoryGood;
        //private Rectangle[] StoryBad;

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
                Text = "Good Button Test"
            };

            buttonGood.Click += (s, a) =>
            {
                //currently sends back to main menu but temperary
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            };

            grid.Widgets.Add(buttonGood);

 /*           var buttonMid = new TextButton
            {
                GridColumn = 0,
                GridRow = 9,
                Text = "Neutral Button Test"
            };

            buttonMid.Click += (s, a) =>
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            };

            grid.Widgets.Add(buttonMid);*/


            var buttonBad = new TextButton
            {
                GridColumn = 0,
                GridRow = 9,
                Text = "Click me I'm bad!"
            };

            buttonBad.Click += (s, a) =>
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
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

           
            //string fileIncoming = "Part1.json"; //This doesn't work no matter what the Part1.json reads null
            string jsonstring = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Story/Part1.json")); //This doesn't work*/

            /*string jsonstring = @"{
                       ""Id"" : 17,
                         ""Story"" : ""Hello World!""
                            }";*/

            Message? message = JsonSerializer.Deserialize<Message>(jsonstring); 

            //throw new NotImplementedException();
            _graphicsDevice.Clear(Color.LightGreen);

            //Draw test to the screen
            spriteBatch.Begin();
            spriteBatch.DrawString(TestFont, text: $"Our message:{message.Story}", new Vector2(0, 0), Color.Black); //draw the font 
            spriteBatch.End();

            _desktop.Render();


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

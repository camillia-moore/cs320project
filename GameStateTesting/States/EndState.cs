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
    public class EndState : State

    {
        private Desktop _desktop;

        SpriteFont TestFont; //create sprite for font

        public override void Update(GameTime gameTime)
        {
        }
        public EndState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            MyraEnvironment.Game = _game;

            bool creditorEnd = Story.CheckString.returnEnd();
            if (creditorEnd == false )
            {
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
                var goToCredits = new TextButton
                {
                    GridColumn = 0,
                    GridRow = 8,
                    Text = "Fin"
                };

                goToCredits.Click += (s, a) =>
                {
                    EndState nextstate = new EndState(_game, _graphicsDevice, _content);
                    Story.CheckString.changeEnd();//This will change the bool to let us know that next iteration we will be on credits.
                    _game.ChangeState(nextstate);
                };
                grid.Widgets.Add(goToCredits);
                _desktop = new Desktop();
                _desktop.Root = grid;
                TestFont = _content.Load<SpriteFont>("Fonts/FreakingTest"); //
            }
            if (creditorEnd == true )
            {
                var gridlost = new Grid
                {
                    RowSpacing = 8,
                    ColumnSpacing = 8
                };

                gridlost.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
                gridlost.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
                gridlost.RowsProportions.Add(new Proportion(ProportionType.Auto));
                gridlost.RowsProportions.Add(new Proportion(ProportionType.Auto));

                // Button to Character Creation
                var goToCreditslost = new TextButton
                {
                    GridColumn = 0,
                    GridRow = 8,
                    Text = "Exit"
                };

                goToCreditslost.Click += (s, a) =>
                {
                    MenuState nextstate = new MenuState(_game, _graphicsDevice, _content);
                    _game.ChangeState(nextstate);

                };
                gridlost.Widgets.Add(goToCreditslost);
                _desktop = new Desktop();
                _desktop.Root = gridlost;
                TestFont = _content.Load<SpriteFont>("Fonts/FreakingTest"); //
            }
        }



        public override void LoadContent()
        {
            throw new NotImplementedException();
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            string placeinstory = Story.CheckString.StoryCheckString();
            int countOfA = Story.CheckString.returnbuffcountA(placeinstory);
            int countOfB = Story.CheckString.returnbuffcountB(placeinstory);
            bool checkCreditorStory = Story.CheckString.returnEnd();

            if (checkCreditorStory == false)
            {

                if (Story.CheckString.returnLostBattle() == true) //if the battle is won
                {
                    _graphicsDevice.Clear(Color.Red);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(TestFont, text: $"{"Unfortunately, your body gave out during your life and death battle with illness."}", new Vector2(100, 150), Color.Black); //draw the font 
                    spriteBatch.DrawString(TestFont, text: $"{"If there is a next time, may fate fancy you in a bright light."}", new Vector2(110, 200), Color.Black);
                    spriteBatch.End();
                    _desktop.Render();
                }
                if (Story.CheckString.returnLostBattle() ==  false)//if the battle was lost
                {
                    if (countOfA == 0)//then this is the worst ending 
                    {
                        _graphicsDevice.Clear(Color.Red);
                        spriteBatch.Begin();
                        spriteBatch.DrawString(TestFont, text: $"{"Your accident killed two people and you survived the accident, however you were injured."}", new Vector2(100, 150), Color.Black); //draw the font 
                        spriteBatch.DrawString(TestFont, text: $"{"You were hosopitalized and treated for COVID with the use of a ventilator."}", new Vector2(110, 200), Color.Black);
                        spriteBatch.DrawString(TestFont, text: $"{"Most of your family caught it from you and so did you school mates."}", new Vector2(120, 250), Color.Black);
                        spriteBatch.DrawString(TestFont, text: $"{"Three weeks later you would go to Grandma nans funeral...."}", new Vector2(130, 250), Color.Black);
                        spriteBatch.End();
                        _desktop.Render();
                    }

                    if (countOfB == 0)
                    {
                        _graphicsDevice.Clear(Color.LightBlue);
                        spriteBatch.Begin();
                        spriteBatch.DrawString(TestFont, text: $"{"You followed all suggestions from medical professionals and took care of your health."}", new Vector2(100, 150), Color.Black); //draw the font 
                        spriteBatch.DrawString(TestFont, text: $"{"You also got no one sick meaning the illness stopped with you. GOOD JOB!!"}", new Vector2(100, 200), Color.Black);
                        spriteBatch.DrawString(TestFont, text: $"{"You deserve cake but be careful, the cake is a lie."}", new Vector2(100, 200), Color.Black);
                        spriteBatch.End();
                        _desktop.Render();
                    }

                    if ((countOfB != 0) && (countOfA != 0))
                    {
                        _graphicsDevice.Clear(Color.LightCoral);
                        spriteBatch.Begin();
                        spriteBatch.DrawString(TestFont, text: $"{"In your own way you tried and failed in many aspects."}", new Vector2(120, 150), Color.Black); //draw the font 
                        spriteBatch.DrawString(TestFont, text: $"{"However, you survived the big ick and that's good!"}", new Vector2(100, 200), Color.Black);
                        spriteBatch.DrawString(TestFont, text: $"{"Hppefully next time you will do everything in your power to make better decisions."}", new Vector2(100, 200), Color.Black);
                        spriteBatch.End();
                        _desktop.Render();
                    }
                }
            }
            if (checkCreditorStory == true)
            {

            _graphicsDevice.Clear(Color.Silver);
            //List<Message> message = JsonUtility.GetJsonStringMessageFromJSON("Story/Part3.json");
            //Draw test to the screen
            spriteBatch.Begin();
            spriteBatch.DrawString(TestFont, text: $"{"The creators of the school project called 'Coughing Story'"}", new Vector2(120, 150), Color.Black); //draw the font 
            spriteBatch.DrawString(TestFont, text: $"{"Camillia"}", new Vector2(100, 200), Color.Black);
            spriteBatch.DrawString(TestFont, text: $"{"June"}", new Vector2(100, 200), Color.Black);
            spriteBatch.DrawString(TestFont, text: $"{"Lyndsey"}", new Vector2(100, 200), Color.Black);
            spriteBatch.End();
            _desktop.Render();
            }

        }
    }
}

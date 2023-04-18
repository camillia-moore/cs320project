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
using Myra.Graphics2D;

namespace GameStateTesting.Customization
{
    public class CharacterCustom
    {
        private string charPronouns { get; set; }
        private int[] charCustomization { get; set; }
        private int charWeapon { get; set; }
        private int charEquipment { get; set; }

        private int xOffset;
        private int yOffset;

        /*
        private Texture2D charBase;
        private Texture2D charHead;
        private Texture2D charFace;
        private Texture2D charBody;

        private Rectangle[] charHeadSource;
        private Rectangle[] charFaceSource;
        private Rectangle[] charBodySource;

        private GraphicsDeviceManager _graphics;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch charSpriteBatch;

        */

        public CharacterCustom(int pronouns, int head, int face, int body, int bodyColor) 
        {
            int[] charCustomization = { head, face, body, bodyColor };

            switch(pronouns)
            {
                case 1:
                    charPronouns = "he";
                    break;
                case 2:
                    charPronouns = "she";
                    break;
                default:
                    charPronouns = "they";
                    break;
            }
        }
        // call this in the LoadContent() in a scene first before drawing
        public void DrawCharSpriteInitialize()
        {
            /*
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            charBase = _content.Load<Texture2D>("char-base");
            charHead = _content.Load<Texture2D>("char-head");
            charFace = _content.Load<Texture2D>("char-face");
            charBody = _content.Load<Texture2D>("char-body");

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
            */
        }

        // call this in the Draw() in a scene to load the sprites in
        // can specify a position to place the image by using x and y
        // if another spriteBatch is being used, make sure it End it first
        public void DrawCharSpriteDraw(int x, int y)
        {
            /*
            xOffset = x;
            yOffset = y;

            _spriteBatch.Begin();

            // draw character base
            charSpriteBatch.Draw(charBase, new Vector2(0 + xOffset, 0 + yOffset), Color.White);

            // draw head area sprites
            switch (charCustomization[0])
            {
                case 1:
                    _spriteBatch.Draw(charHead, new Vector2(263 + xOffset, 30 + yOffset), charHeadSource[1], Color.White);
                    break;
                case 2:
                    _spriteBatch.Draw(charHead, new Vector2(238 + xOffset, 125 + yOffset), charHeadSource[2], Color.White);
                    break;
                case 3:
                    _spriteBatch.Draw(charHead, new Vector2(278 + xOffset, 53 + yOffset), charHeadSource[3], Color.White);
                    break;
                default:
                    _spriteBatch.Draw(charHead, new Vector2(0 + xOffset, 0 + yOffset), charHeadSource[0], Color.White);
                    break;
            }

            // draw face area sprites
            _spriteBatch.Draw(charFace, new Vector2(228 + xOffset, 166 + yOffset), charFaceSource[charCustomization[1]], Color.White);

            // draw body area sprites
            switch (charCustomization[2])
            {
                case 1:
                    _spriteBatch.Draw(charBody, new Vector2(197 + xOffset, 364 + yOffset), charBodySource[1], Color.White);
                    break;
                case 2:
                    _spriteBatch.Draw(charBody, new Vector2(223 + xOffset, 364 + yOffset), charBodySource[2], Color.White);
                    break;
                case 3:
                    _spriteBatch.Draw(charBody, new Vector2(291 + xOffset, 364 + yOffset), charBodySource[3], Color.White);
                    break;
                default:
                    _spriteBatch.Draw(charBody, new Vector2(0 + xOffset, 0 + yOffset), charBodySource[0], Color.White);
                    break;
            }

            _spriteBatch.End();
            */

        }

    }
}

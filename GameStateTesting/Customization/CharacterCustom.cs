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
    // holds the character customization options
    static class CharacterCustom
    {
        private static string charPronouns = "they";
        private static int[] charCustomization = { 0, 0, 0, 0 };

        public static void setPronouns(int pronounNumber)
        {
            switch (pronounNumber)
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

        public static void setCustomization(int head, int face, int body, int baseColor) 
        {
            charCustomization = new int[] { head, face, body, baseColor };
        }

        public static string getPronouns()
        {
            return charPronouns;
        }

        public static int[] getCustomization() 
        {
            return charCustomization;
        }

    }
}

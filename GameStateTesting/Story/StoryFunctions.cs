using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameStateTesting.Story
{
    //This will hold the story increment and initialization
    //It will also hold our to jump to the end battle at the end
    static class CheckString 
    {
        private static string idPlacement = "X";
        private static int monsterCounter = 0;

        public static string StoryCheckString()
        { return idPlacement; }

        public static int monsterCountGet()
        { return monsterCounter; }

        public static string ChangeString(string newstring)
        {
            idPlacement = idPlacement + newstring;
            return idPlacement;
        }

        public static void increaseMonsterCount()
        { monsterCounter = monsterCounter + 1; }

        public static void MakeOriginalString()
        { idPlacement = "X"; }

        public static void MakeZeroMonCount()
        { monsterCounter = 0; }
    }
}

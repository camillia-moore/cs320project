using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
        private static bool isend = false;
        private static bool lostbattle = false;

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
        //Counts how many A's are in the story string 
        public static int returnbuffcountA(string letscountstringA)
        {
            char charA = 'A';
            int countA = 0;
            for (int n = 0; n < letscountstringA.Length; n++)
            {
                if (letscountstringA[n] == charA)
                    countA++;
            }
            return countA;
        }
        //counts how many B's are in the story string
        public static int returnbuffcountB(string letscountstringB)
        {
            char charB = 'B';
            int countB = 0;
            for (int n = 0; n < letscountstringB.Length; n++)
            {
                if (letscountstringB[n] == charB)
                    countB++;
            }
            return countB;
        }

        public static void changeEnd() //THis bool using to move between the end restponse and credits 
        { isend = true; }

        public static bool returnEnd() { return isend; }    

        public static void revertEnd()
        { isend = false; }

        public static void changeLostBoss() //if lose to boss battle change to true
        { lostbattle = true; }

        public static bool returnLostBattle() { return lostbattle; }

        public static void revertLost()
        { lostbattle = false; }

        public static void revertALL() //This will revert everything to a fresh start
        {
            revertLost();
            revertEnd();
            MakeZeroMonCount();
            MakeOriginalString();
        }


    }
}

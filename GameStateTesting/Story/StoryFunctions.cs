using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameStateTesting.Story
{
    static class CheckString //This will hold the story increment and initialization
    {
        private static string idPlacement = "X";

        public static string StoryCheckString()
        {
            return idPlacement;
        }

        public static string ChangeString( string newstring )
        {
            idPlacement = idPlacement + newstring;
            return idPlacement;
        }

        public static void MakeOriginalString()
        {
            idPlacement = "X";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateTesting.BattleClasses
{
    public class Effect
    {
        public int _HPMod;
        public int _AttackMod;
        public int _DefenseMod;
        public int _healOrDeal; //bool to track if the spell buffs the user or nerfs the oponent
                                       // 0 = buff user, 1 = nerf enemy
        
        public Effect(int HP, int atk, int def, int hd) {
            //constructor
            _HPMod = HP;
            _AttackMod = atk;
            _DefenseMod = def;
            _healOrDeal = hd;
        }

        public int[] getEffect()
        {
            //returns data
            int[] ret = {_HPMod, _AttackMod, _DefenseMod, _healOrDeal};
            return ret;
        }
    }
}

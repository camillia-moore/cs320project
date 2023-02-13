using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateTesting.BattleClasses
{
    internal class Effect
    {
        private int _HPMod;
        private int _AttackMod;
        private int _DefenseMod;
        private int _healOrDeal; //bool to track if the spell buffs the user or nerfs the oponent
                                       // 0 = buff user, 1 = nerf enemy
        
        public Effect(int HP, int atk, int def, int hd) {
            _HPMod = HP;
            _AttackMod = atk;
            _DefenseMod = def;
            _healOrDeal = hd;
        }

        public int[] getEffect()
        {
            int[] ret = {_HPMod, _AttackMod, _DefenseMod, _healOrDeal};
            return ret;
        }
    }
}

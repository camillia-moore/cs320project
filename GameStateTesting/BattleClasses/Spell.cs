using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateTesting.BattleClasses
{
    public class Spell
    {
        public string _name;
        public string _description;
        public Effect _effect;
        public Spell(string name, string description, Effect effect) {
            _name = name;
            _description = description;
            _effect = effect;
        }

        public int[] cast()
        {
            return _effect.getEffect();
        }


    }
}

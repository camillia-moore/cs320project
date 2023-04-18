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
<<<<<<< HEAD
        private Effect _effect;
        public int _manaCost;
        public Spell(string name, string description, Effect effect, int manaCost)
        {
=======
        public Effect _effect;
        public Spell(string name, string description, Effect effect) {
>>>>>>> main
            _name = name;
            _description = description;
            _effect = effect;
            _manaCost = manaCost;
        }

        public int[] cast()
        {
            return _effect.getEffect();
        }

    }
}

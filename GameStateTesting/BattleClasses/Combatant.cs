using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameStateTesting.BattleClasses
{
    public class Combatant
    {
        public string Name;
        public string Description;
        public int MaxHP;
        public int CurrentHP;
        public int Attack;
        public int AttackMod;
        public int Defense;
        public int DefenseMod;
        
        public Combatant(string name, string description, int hp, int atk, int def) {
            Name = name;
            Description= description;
            MaxHP = hp;
            CurrentHP = hp;
            Attack = atk;
            AttackMod = 0; 
            Defense = def;
            DefenseMod = 0;
        }

        public int DealDamage()
        {
            return Attack + AttackMod;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage - (Defense + DefenseMod);
        }

        public int[] getStats()
        {
            int[] stats = { Attack, AttackMod, Defense, DefenseMod };
            return stats;
        }

        public int[] getHP()
        {
            int[] hp = { CurrentHP, MaxHP };
            return hp;
        }

        public void ModifyStats(int HPModify, int AttackModify, int DefenseModify)
        {
            CurrentHP += HPModify;
            AttackMod += AttackModify;
            DefenseMod += DefenseModify;
        }

        public void ClearMods() 
        {
            AttackMod = 0;
            DefenseMod = 0;
        }
    }
}

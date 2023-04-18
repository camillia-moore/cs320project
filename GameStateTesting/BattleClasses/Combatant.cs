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
        private int MaxHP;
        private int CurrentHP;
        private int Attack;
        private int AttackMod;
        private int Defense;
        private int DefenseMod;
        private bool defeated;
        
        public Combatant(string name, string description, int hp, int atk, int def) {
            Name = name;
            Description= description;
            MaxHP = hp;
            CurrentHP = hp;
            Attack = atk;
            AttackMod = 0; 
            Defense = def;
            DefenseMod = 0;
            defeated = false;
        }

        public int DealDamage()
        {
            return Attack + AttackMod;
        }

        public int TakeDamage(int damage)
        {
            int damageTaken = damage - (Defense + DefenseMod);
            if ( damageTaken < 1 ) { damageTaken = 1; } //should not be healed by attacks, min damage is 1
            CurrentHP -= damageTaken;
            updateDefeated();
            return damageTaken;
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
            if (CurrentHP > MaxHP) { CurrentHP = MaxHP; }
            updateDefeated();
        }

        public void ClearMods() 
        {
            AttackMod = 0;
            DefenseMod = 0;
        }

        public bool isDefeated()
        {
            return defeated;
        }

        private void updateDefeated()
        {
            if (CurrentHP <= 0)
            {
                defeated = true;
                CurrentHP = 0;
            }
        }
    }
}

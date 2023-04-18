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
            //constructor
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
            //returns the amount of damage this combatant deals rn
            return Attack + AttackMod;
        }

        public int TakeDamage(int damage)
        {
            //has the combatant take the given damage
            int damageTaken = damage - (Defense + DefenseMod);
            if ( damageTaken < 1 ) { damageTaken = 1; } //should not be healed by attacks, min damage is 1
            CurrentHP -= damageTaken;
            updateDefeated();
            return damageTaken;  //returns damage taken so it can be displayed
        }

        public int[] getStats()
        {
            //returns an array of stats
            int[] stats = { Attack, AttackMod, Defense, DefenseMod };
            return stats;
        }

        public int[] getHP()
        {
            //returns the current hp stats
            int[] hp = { CurrentHP, MaxHP };
            return hp;
        }

        public void ModifyStats(int HPModify, int AttackModify, int DefenseModify)
        {
            //modifies the stat mods
            CurrentHP += HPModify;
            AttackMod += AttackModify;
            DefenseMod += DefenseModify;
            if (CurrentHP > MaxHP) { CurrentHP = MaxHP; }
            updateDefeated();
        }

        public void ClearMods() 
        {
            //clears any stat mods, unused
            AttackMod = 0;
            DefenseMod = 0;
        }

        public bool isDefeated()
        {
            //returns if the combatant is defeated
            return defeated;
        }

        private void updateDefeated()
        {
            //checks if the combatant should be dead
            if (CurrentHP <= 0)
            {
                defeated = true;
                CurrentHP = 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameStateTesting.States;
using GameStateTesting.BattleClasses;
using System.Numerics;
using Myra.Graphics2D.UI;

public class MockBattleScreen
{
    private int playerHP = 30;
    public Combatant player;
    public Combatant enemy;
    public Spell fireball;
    public Spell iceStorm;
    public Spell diacute;
    public Spell healing;
    public Random rand;

    public MockBattleScreen()
	{
        player = new Combatant("KitKat", "The Default Hero", 30, 9, 5);
        enemy = new Combatant("Monster", "Generic Enemy", 20, 8, 4);
        fireball = new Spell("Fireball", "Deals damage to the opponent", new GameStateTesting.BattleClasses.Effect(-10, 0, 0, 1), 2);
        iceStorm = new Spell("Ice Storm", "Uses Ice to Weaken the enemy", new GameStateTesting.BattleClasses.Effect(0, -2, -2, 1), 3);
        diacute = new Spell("Diacute", "Buffs the user's stats", new GameStateTesting.BattleClasses.Effect(0, +2, +2, 0), 4);
        healing = new Spell("Healing", "Heals the user", new GameStateTesting.BattleClasses.Effect(+5, 0, 0, 0), 5);

        rand = new Random();
    }
    
	public bool fleeCommand()
	{
        int[] hp = player.getHP();
        double fleeChance = hp[0] / hp[1];
        double fleeSuccess = rand.Next(0, hp[1]) / hp[1];

        if (fleeChance > fleeSuccess) // flee was successful
        {
            return true;
        }
        else  // couldn't flee
        {
            return false;
        }
    }

    public string getStatsString()
    {
        int[] hp = player.getHP();
        int[] stats = player.getStats();
        int[] enemyHP = enemy.getHP();
        string toDisplay = enemy.Name + " HP: " + enemyHP[0] + "/" + enemyHP[1] + "\n" +
                            player.Name + " HP: " + hp[0] + "/" + hp[1] + "\n" +
                            "Atk: " + stats[0] + " + " + stats[1] + "\n" +
                            "Def: " + stats[2] + " + " + stats[3];
        return toDisplay;
    }

    public void spellEffect(Spell spellToCast)
    {
        int[] spellEffect = spellToCast.cast();
        if (spellEffect[3] == 0)
        {
            //buff player
            player.ModifyStats(spellEffect[0], spellEffect[1], spellEffect[2]);
        }
        else //spellEffect[3] == 1
        {
            //nerf enemy
            enemy.ModifyStats(spellEffect[0], spellEffect[1], spellEffect[2]);
        }
    }

}

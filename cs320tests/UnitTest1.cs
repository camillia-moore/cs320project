using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameStateTesting;
using GameStateTesting.BattleClasses;
using GameStateTesting.States;
using Microsoft.Xna.Framework.Content;
using System.Numerics;

namespace cs320tests
{
    [TestClass]
    public class CoughingStoryTests
    {
        [TestMethod]
        public void StatModTest()
        {
            // Acceptance test
            // Tests whether the modifiers for the player stats are calculated correctly when applied

            int playerHP = 30;
            int playerAtk = 6;
            int playerDef = 10;

            int modHP = 5;
            int modAtk = 4;
            int modDef = -12;

            int expectedHP = 35;
            int expectedAtk = 10;
            int expectedDef = -2;

            Combatant player = new Combatant("TestPlayer", "The Unit Test Pawn", playerHP, playerAtk, playerDef);

            player.ModifyStats(modHP, modAtk, modDef);

            int actualHP = player.CurrentHP;
            int actualAtk = player.Attack;
            int actualDef = player.Defense;

            Assert.AreEqual(expectedHP, actualHP);
            Assert.AreEqual(expectedAtk, actualAtk + modAtk);
            Assert.AreEqual(expectedDef, actualDef + modDef);
        }

        [TestMethod]
        public void battlePlayerEnemyInteraction()
        {
            // Acceptance test
            // for player damage calculations between two combatants
            // to see if Attack and Defense modifications work correctly on HP values

            int playerHP = 20;
            int playerAtk = 10;
            int playerDef = 7;

            int enemyHP = 15;
            int enemyAtk = 12;
            int enemyDef = 9;

            Combatant player = new Combatant("TestPlayer", "The Unit Test Pawn", playerHP, playerAtk, playerDef);
            Combatant enemy = new Combatant("TestEnemy", "The Unit Test Monster", enemyHP, enemyAtk, enemyDef);

            int expectedPlayerHP = 15; // 20 - (12 - 7)
            int expectedEnemyHP = 14; // 15 - (10 - 9)

            int damageFromPlayer = player.DealDamage();
            int damageFromEnemy = enemy.DealDamage();

            enemy.TakeDamage(damageFromPlayer);
            player.TakeDamage(damageFromEnemy);

            Assert.AreEqual(expectedPlayerHP, player.CurrentHP);
            Assert.AreEqual(expectedEnemyHP, enemy.CurrentHP);
        }

        [TestMethod]
        public void spellEffectInteraction()
        {
            // Integration test: bottom-up approach
            // for interactions between Spell and Effect classes
            // test by creating an Effect, using it in Spell, then seeing if casting the Spell returns the same Effect

            GameStateTesting.BattleClasses.Effect unitEffect= new GameStateTesting.BattleClasses.Effect(5, 2, 3, 1);
            Spell unitSpell = new Spell("Spell Test", "This is a magical unit testing spell", unitEffect);

            int[] expectedEffect = { 5, 2, 3, 1 };

            // cast returns an int[], compare elements inside both arrays
            CollectionAssert.AreEqual(expectedEffect, unitSpell.cast());
        }

        [TestMethod]
        public void UItoEndTest()
        {
            // White-box coverage test
            // for UI navigation to satisfy conditions to go to next screen.
            // Traces a series of inputs and sees if it results in the correct target location (1 up, 2 down, 3 left, 4 right)
            // Inputs can be infinite, so test possible looped inputs to be thorough. (accounts for repeat inputs and loops)
            // Note: Mock classes were created based on the logic tree of the original file;
            // the original framework utilizes classes which cause dependency issues in unit tests (google verified)
            
            int[] myInputs1 = { 2, 2, 2 };
            int[] myInputs2 = { 2, 2, 2, 2 };
            int[] myInputs3 = { 1, 2, 2, 2 };
            int[] myInputs4 = { 1, 1, 2, 2, 1, 1, 1, 2, 2, 1, 2, 2 };
            int[] myInputs5 = { 1, 1, 2, 3, 3, 2, 4, 4, 1, 1, 1, 2, 3, 4, 2, 4, 3, 1, 3, 4, 2, 3, 2 };

            MockCharCreationUI testInputs1 = new MockCharCreationUI(myInputs1);
            MockCharCreationUI testInputs2 = new MockCharCreationUI(myInputs2);
            MockCharCreationUI testInputs3 = new MockCharCreationUI(myInputs3);
            MockCharCreationUI testInputs4 = new MockCharCreationUI(myInputs4);
            MockCharCreationUI testInputs5 = new MockCharCreationUI(myInputs5);

            // test that the continue flag is set to positive at the end of each series of inputs
            Assert.IsTrue(testInputs1.canContinue());
            Assert.IsTrue(testInputs2.canContinue());
            Assert.IsTrue(testInputs3.canContinue());
            Assert.IsTrue(testInputs4.canContinue());
            Assert.IsTrue(testInputs5.canContinue());
        }

        [TestMethod]
        public void UIbodyAreaTest()
        {
            // White-box coverage test
            // for UI navigation to see if a series of inputs results in the correct target chosen for head area
            // Inputs can be infinite, so test possible looped inputs to be thorough.

            int[] myInputs6 = { 3, 1, 2, 2, 3 };
            int[] myInputs7 = { 2, 2, 3, 3 };
            int[] myInputs8 = { 3, 1, 2, 2, 3, 1, 2, 3, 3 };
            int[] myInputs9 = { 2, 2, 4 };
            int[] myInputs10 = { 2, 2, 4, 1, 2, 4 };
            int[] myInputs11 = { 1, 2, 2, 3, 4 };

            MockCharCreationUI testInputs6 = new MockCharCreationUI(myInputs6);
            MockCharCreationUI testInputs7 = new MockCharCreationUI(myInputs7);
            MockCharCreationUI testInputs8 = new MockCharCreationUI(myInputs8);
            MockCharCreationUI testInputs9 = new MockCharCreationUI(myInputs9);
            MockCharCreationUI testInputs10 = new MockCharCreationUI(myInputs10);
            MockCharCreationUI testInputs11 = new MockCharCreationUI(myInputs11);

            // test whether the input string results in the expected number for the variable

            Assert.AreEqual(testInputs6.getBody(), 1);
            Assert.AreEqual(testInputs7.getBody(), 2);
            Assert.AreEqual(testInputs8.getBody(), 3);
            Assert.AreEqual(testInputs9.getBody(), 3);
            Assert.AreEqual(testInputs10.getBody(), 2);
            Assert.AreEqual(testInputs11.getBody(), 0);
        }

        [TestMethod]
        public void customizationLoopTest()
        {
            // Acceptance test
            // for testing if cycling through customization options will loop properly
            int[] myInputs = { 3, 3, 3, 3, 3 };
            MockCharCreationUI testInputs = new MockCharCreationUI(myInputs);

            Assert.AreEqual(testInputs.headArea, 0);

            Assert.AreEqual(testInputs.getHead(), 0);
        }

        [TestMethod]
        public void defenseModDamageTest()
        {
            // Acceptance test
            // for testing if the defense mod is accounted for properly when taking damage

            int playerHP = 50;
            int playerAtk = 0;
            int playerDef = 6;

            int defenseModify = 5;

            int expectedPlayerHP = 46;

            Combatant player = new Combatant("TestPlayer", "The Unit Test Pawn", playerHP, playerAtk, playerDef);
            
            player.ModifyStats(0, 0, defenseModify);
            player.TakeDamage(15);

            Assert.AreEqual(player.CurrentHP, expectedPlayerHP);
        }

        [TestMethod]
        public void clearModsTest()
        {
            // Acceptance test
            // for testing whether clearing mods properly returns stats to normal

            int playerHP = 50;
            int playerAtk = 0;
            int playerDef = 6;

            int attackModify = 10;
            int defenseModify = 5;

            int expectedPlayerDamageBefore = 10;
            int expectedPlayerDamageAfter = 0;

            Combatant player = new Combatant("TestPlayer", "The Unit Test Pawn", playerHP, playerAtk, playerDef);

            player.ModifyStats(0, attackModify, defenseModify);

            Assert.AreEqual(player.DealDamage(), expectedPlayerDamageBefore);

            player.ClearMods();

            Assert.AreEqual(player.DealDamage(), expectedPlayerDamageAfter);
        }

        [TestMethod]
        public void fleeChanceTest()
        {
            // Acceptance test
            // for comparing values for when the player is able to flee

            MockBattleScreen testBattle = new MockBattleScreen();

            int damageAmount = 25;

            testBattle.player.TakeDamage(damageAmount);

            // should flee successfully if enough damage was taken
            Assert.IsTrue(testBattle.fleeCommand());
        }

        [TestMethod]
        public void HPcanGoBelowZeroTest()
        {
            // Acceptance test
            // Tests whether a combatant's HP is allowed to go below zero
            // Should not display negative values if HP is below 0

            int playerHP = 30;
            int playerAtk = 10;
            int playerDef = 10;

            int bigDamage = 50;
            int expectedHP = 0;

            Combatant player = new Combatant("TestPlayer", "The Unit Test Pawn", playerHP, playerAtk, playerDef);

            player.TakeDamage(bigDamage);

            Assert.AreEqual(player.CurrentHP, expectedHP);
        }

        [TestMethod]
        public void spellHealUserTest()
        {
            // Acceptance test
            // for testing if a spell correctly heals the user

            MockBattleScreen testBattle = new MockBattleScreen();

            testBattle.spellEffect(testBattle.healing);

            int expectedHP = 35; // initial player HP is 30, healing spell heals 5

            Assert.AreEqual(expectedHP, testBattle.player.CurrentHP);
        }

        [TestMethod]
        public void spellUserOrEnemyTest()
        {
            // Acceptance test
            // for testing if a spell properly affects the player's or enemy's stats

            MockBattleScreen testBattle = new MockBattleScreen();

            testBattle.spellEffect(testBattle.diacute); // expected to buff user's stats
            int expectedPlayerAtk = 11;

            Assert.AreEqual(expectedPlayerAtk, (testBattle.player.Attack + testBattle.player.AttackMod));


            testBattle.spellEffect(testBattle.iceStorm); // expected to weaken the enemy's stats
            int expectedEnemyAtk = 6;

            Assert.AreEqual(expectedEnemyAtk, (testBattle.enemy.Attack + testBattle.enemy.AttackMod));
        }
    }
}
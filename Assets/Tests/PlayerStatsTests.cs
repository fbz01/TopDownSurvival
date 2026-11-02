using NUnit.Framework;
using UnityEngine;

public class PlayerStatsTests
{
    private PlayerStats stats;

    [SetUp]
    public void SetUp()
    {
        stats = new GameObject("TestPlayer").AddComponent<PlayerStats>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(stats.gameObject);
    }

    [Test]
    public void IncreaseStrength_AddsAmount()
    {
        stats.IncreaseStrength(3);
        Assert.AreEqual(8, stats.Strength);
    }

    [TestCase(0)]
    [TestCase(-5)]
    public void IncreaseStrength_IgnoresZeroAndNegative(int amount)
    {
        stats.IncreaseStrength(amount);
        Assert.AreEqual(5, stats.Strength);
    }

    [Test]
    public void IncreaseCritChance_NeverGoesAbove100()
    {
        stats.IncreaseCritChance(500);
        Assert.AreEqual(100, stats.CritChance);
    }

    [Test]
    public void IncreaseAttackSpeed_NeverGoesAboveMax()
    {
        stats.IncreaseAttackSpeed(10f);
        Assert.AreEqual(3f, stats.AttackSpeedMultiplier, 0.0001f);
    }

    [Test]
    public void OnStatsChanged_IsInvoked_WhenValueChanges()
    {
        bool changed = false;
        stats.OnStatsChanged += () => changed = true;

        stats.IncreaseDamage(2);

        Assert.IsTrue(changed);
    }

    [Test]
    public void OnStatsChanged_IsNotInvoked_WhenAmountIsIgnored()
    {
        bool changed = false;
        stats.OnStatsChanged += () => changed = true;

        stats.IncreaseDamage(0);

        Assert.IsFalse(changed);
    }

    [Test]
    public void RollCrit_AlwaysCrits_At100Percent()
    {
        stats.IncreaseCritChance(100);

        for (int i = 0; i < 1000; i++)
            Assert.IsTrue(stats.RollCrit());
    }

    [Test]
    public void RollCrit_NeverCrits_AtZeroPercent()
    {
        for (int i = 0; i < 1000; i++)
            Assert.IsFalse(stats.RollCrit(-stats.CritChance));
    }
}
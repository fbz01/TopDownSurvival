using NUnit.Framework;
using UnityEngine;

public class HealthTests
{
    private Health health;
    private PlayerStats attackerStats;

    // SetUp runs before each test
    [SetUp]
    public void SetUp()
    {
        health = new GameObject("TestObject").AddComponent<Health>();
        health.ResetHealth(); // Awake doesn't run in Edit Mode, so health is set manually

        attackerStats = new GameObject("TestPlayer").AddComponent<PlayerStats>();
    }

    // TearDown cleans up after each test
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(health.gameObject);
        Object.DestroyImmediate(attackerStats.gameObject);
    }

    [Test]
    public void TakeDamage_ReducesCurrentHealth()
    {
        health.TakeDamage(30);
        Assert.AreEqual(70, health.Current);
    }

    [Test]
    public void TakeDamage_NeverGoesBelowZero()
    {
        health.TakeDamage(500);
        Assert.AreEqual(0, health.Current);
    }

    [Test]
    public void Heal_RaisesCurrentHealth()
    {
        health.TakeDamage(50);
        health.Heal(20);
        Assert.AreEqual(70, health.Current);
    }

    [Test]
    public void Heal_NeverGoesAboveMax()
    {
        health.TakeDamage(20);
        health.Heal(50);
        Assert.AreEqual(100, health.Current);
    }

    [Test]
    public void TakeDamage_WithNegativeAmount_DoesNothing()
    {
        health.TakeDamage(-10);
        Assert.AreEqual(100, health.Current);
    }

    [Test]
    public void Heal_WhenDead_DoesNotRevive()
    {
        health.TakeDamage(100);
        health.Heal(20);
        Assert.AreEqual(0, health.Current);
    }

    [Test]
    public void OnDied_IsInvokedExactlyOnce()
    {
        int diedCount = 0;
        health.OnDied += () => diedCount++;

        health.TakeDamage(100);
        health.TakeDamage(10);   // hitting an already dead object

        Assert.AreEqual(1, diedCount);
    }

    [Test]
    public void OnDied_IsNotInvoked_WhenStillAlive()
    {
        bool died = false;
        health.OnDied += () => died = true;

        health.TakeDamage(99);

        Assert.IsFalse(died);
    }

    [Test]
    public void OnDamaged_SendsAmountAndCritFlag()
    {
        int receivedAmount = 0;
        bool receivedCrit = false;
        health.OnDamaged += (amount, isCrit) =>
        {
            receivedAmount = amount;
            receivedCrit = isCrit;
        };

        health.TakeDamage(25, true);

        Assert.AreEqual(25, receivedAmount);
        Assert.IsTrue(receivedCrit);
    }

}

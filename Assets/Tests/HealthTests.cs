using NUnit.Framework;
using UnityEngine;

public class HealthTests
{
    private Health health;

    // setup körs före test
    [SetUp]
    public void SetUp()
    {
        health = new GameObject("TestObject").AddComponent<Health>();
        health.ResetHealth();
    }

    // teardown städar upp och körs efter testet
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(health.gameObject);
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

}

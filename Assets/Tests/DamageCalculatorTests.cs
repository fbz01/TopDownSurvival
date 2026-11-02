using NUnit.Framework;

public class DamageCalculatorTests
{
    [Test]
    public void Calculate_AddsWeaponBonusAndStrength()
    {
        int damage = DamageCalculator.Calculate(10, 2, 5);
        Assert.AreEqual(17, damage);
    }

    [Test]
    public void Calculate_WithoutCrit_IgnoresMultiplier()
    {
        int damage = DamageCalculator.Calculate(10, 0, 5, false, 2f);
        Assert.AreEqual(15, damage);
    }

    [Test]
    public void Calculate_WithCrit_MultipliesDamage()
    {
        int damage = DamageCalculator.Calculate(10, 0, 5, true, 2f);
        Assert.AreEqual(30, damage);
    }

    [TestCase(15, 1.5f, 22)]
    [TestCase(17, 1.5f, 26)]
    [TestCase(11, 1.5f, 16)]
    public void Calculate_WithCrit_RoundsHalfToEven(int baseDamage, float multiplier, int expected)
    {
        int damage = DamageCalculator.Calculate(baseDamage, 0, 0, true, multiplier);
        Assert.AreEqual(expected, damage);
    }

    [Test]
    public void Cooldown_IsInverseOfAttacksPerSecond()
    {
        Assert.AreEqual(0.5f, DamageCalculator.Cooldown(2f, 1f), 0.0001f);
    }

    [Test]
    public void Cooldown_GetsShorter_WithHigherSpeedMultiplier()
    {
        Assert.AreEqual(0.25f, DamageCalculator.Cooldown(2f, 2f), 0.0001f);
    }

    // Visar nuvarande beteende: speed 0 = spelaren kan aldrig attackera
    [Test]
    public void Cooldown_WithZeroSpeed_IsInfinite()
    {
        Assert.IsTrue(float.IsPositiveInfinity(DamageCalculator.Cooldown(2f, 0f)));
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Upgrade Amounts")]
    [SerializeField] private int strengthUpgrade = 5;
    [SerializeField] private int critChanceUpgrade = 10;
    [SerializeField] private float attackSpeedUpgrade = 0.5f;

    private int newStrength;
    private int newCritChance;
    private float newAttackSpeed;

    private bool strengthSelected;
    private bool critChanceSelected;
    private bool attackSpeedSelected;

    private void OnEnable()
    {
        var stats = PlayerStatsManager.Instance;
        if (stats == null) return;

        newStrength = stats.Strength;
        newCritChance = stats.CritChance;
        newAttackSpeed = stats.AttackSpeedMultiplier;

        strengthSelected = false;
        critChanceSelected = false;
        attackSpeedSelected = false;

        if (resultText != null) resultText.text = "";
    }

    public void UpgradeStrength()
    {
        var stats = PlayerStatsManager.Instance;
        if (stats == null) return;

        newStrength = (strengthSelected
            ? newStrength
            : stats.Strength) + strengthUpgrade;

        strengthSelected = true;
        resultText.text = 
            $"Strength: {stats.Strength} > {newStrength}";
    }
    
    public void UpgradeCritChance()
    {
        var stats = PlayerStatsManager.Instance;
        if (stats == null) return;
        
        newCritChance = Mathf.Min((critChanceSelected
            ? newCritChance
            : stats.CritChance) + critChanceUpgrade, 100);

        critChanceSelected = true;

        resultText.text = 
            $"Critical chance: {stats.CritChance}% > {newCritChance}%";
    }
    
    public void UpgradeSpeed()
    {
        var stats = PlayerStatsManager.Instance;
        if (stats == null) return;

        newAttackSpeed = Mathf.Min((attackSpeedSelected
            ? newAttackSpeed
            : stats.AttackSpeedMultiplier) + attackSpeedUpgrade, 3f);

        attackSpeedSelected = true;

        resultText.text = 
            $"Attack Speed: {stats.AttackSpeedMultiplier} > {newAttackSpeed}";
    }

    public void ConfirmUpgrades()
    {
        var stats = PlayerStatsManager.Instance;
        if (stats == null) return;

        stats.SetStats(newStrength, newCritChance, newAttackSpeed);

        GameEvents.GameStateChanged(GameState.Playing);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private string gameSceneName = "Test_A";

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

    private void Start()
    {
        var stats = PlayerStatsManager.Instance;
        
        if (stats == null)
        {
            Debug.LogError("PlayerStatsManager missing!");
            return;
        }

        // Start with players current values
        newStrength = stats.Strength;
        newCritChance = stats.CritChance;
        newAttackSpeed = stats.AttackSpeedMultiplier;
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

        SceneManager.LoadScene(gameSceneName);
    }
}

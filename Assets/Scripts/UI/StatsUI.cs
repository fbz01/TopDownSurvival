using System.Globalization;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;

    private PlayerStats stats;
    private Weapon weapon;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            statsText.text = "";
            return;
        }

        stats = player.GetComponent<PlayerStats>();
        weapon = player.GetComponent<Weapon>();

        stats.OnStatsChanged += UpdateText;
        if (weapon != null) weapon.OnWeaponChanged += HandleWeaponChanged;

        UpdateText();
    }

    private void OnDestroy()
    {
        if (stats != null) stats.OnStatsChanged -= UpdateText;
        if (weapon != null) weapon.OnWeaponChanged -= HandleWeaponChanged;
    }

    private void HandleWeaponChanged(WeaponData newWeapon) => UpdateText();

    private void UpdateText()
    {
        CultureInfo dot = CultureInfo.InvariantCulture; // always "1.5".

        string weaponLine = weapon != null && weapon.CurrentWeapon != null
            ? $"<b>{weapon.CurrentWeapon.DisplayName}</b>  {weapon.Damage} dmg\n"
            : "";

        statsText.text =
            weaponLine +
            $"Strength  {stats.Strength}\n" +
            $"Crit  {stats.CritChance}% (x{stats.CritMultiplier.ToString("0.0", dot)})\n" +
            $"Attack Speed  {stats.AttackSpeedMultiplier.ToString("0.0", dot)}x";
    }
}
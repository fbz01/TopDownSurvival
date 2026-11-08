using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Weapon))]
public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private WeaponData[] weapons; 

    private Weapon weapon;
    private int currentIndex;

    private void Awake() => weapon = GetComponent<Weapon>();

    private void Start()
    {
        int index = System.Array.IndexOf(weapons, weapon.CurrentWeapon);
        currentIndex = index >= 0 ? index : 0;
    }

    private void Update()
    {
        // Weapon is disabled when the player dies
        if (!weapon.enabled) return;
        if (weapons.Length < 2) return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.eKey.wasPressedThisFrame)
            NextWeapon();
    }

    private void NextWeapon()
    {
        currentIndex = (currentIndex + 1) % weapons.Length;
        weapon.Equip(weapons[currentIndex]);
    }
}
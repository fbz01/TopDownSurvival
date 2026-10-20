using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    [SerializeField] private DamagePopup popupPrefab;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.8f, 0f);


    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDamaged += Spawn;
    }

    private void OnDisable()
    {
        health.OnDamaged -= Spawn;
    }

    private void Spawn(int amount, bool isCrit)
    {
        Vector3 jitter = new Vector3(Random.Range(-0.2f, 0.2f), 0f, 0f);

        DamagePopup popup = Instantiate(popupPrefab, transform.position + offset + jitter, Quaternion.identity);
        popup.Setup(amount, isCrit);
    }
}
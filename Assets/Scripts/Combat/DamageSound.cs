using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField, Range(0f, 1f)] private float volume = 0.8f;
    [SerializeField] private float pitchVariation = 0.1f;

    private Health health;

    private void Awake()     => health = GetComponent<Health>();
    private void OnEnable()  => health.OnDamaged += HandleDamaged;
    private void OnDisable() => health.OnDamaged -= HandleDamaged;

    private void HandleDamaged(int amount, bool isCrit)
    {
        if (health.Current <= 0) return;
        if (audioSource == null || hurtSound == null) return;

        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(hurtSound, volume);
    }
}
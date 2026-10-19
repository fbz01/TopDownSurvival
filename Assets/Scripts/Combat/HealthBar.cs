using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Transform fillPivot;
    [SerializeField] private bool hideWhenFull = false;

    private void Awake()
    {
        if (health == null)
            health = GetComponentInParent<Health>();
    }
    private void OnEnable()
    {
        health.OnHealthChanged += UpdateBar;
        health.OnDied += Hide;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateBar;
        health.OnDied -= Hide;
    }

    private void Start()
    {
        UpdateBar(health.Current, health.Max);
    }

    private void UpdateBar(int current, int max)
    {
        float ratio = (float)current / max;
        fillPivot.localScale = new Vector3(ratio, 1f, 1f);
        fillPivot.parent.gameObject.SetActive(!(hideWhenFull && current == max));
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

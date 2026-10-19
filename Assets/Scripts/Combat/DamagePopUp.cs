using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 1.5f;
    [SerializeField] private float lifetime = 0.6f;

    private TextMeshPro text;
    private float timer;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Setup(int amount)
    {
        text.text = amount.ToString();
    }

    private void Update()
    {
        transform.position += Vector3.up * riseSpeed * Time.deltaTime; 

        timer += Time.deltaTime;
        text.alpha = 1f - timer / lifetime;

        if (timer >= lifetime)
            Destroy(gameObject);
    }
}
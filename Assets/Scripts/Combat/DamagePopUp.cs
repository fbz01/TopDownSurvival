using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 1.5f;
    [SerializeField] private float lifetime = 0.6f;
    [SerializeField] private Color critColor = new Color(1f, 0.85f, 0.1f);   // gul
    [SerializeField] private float critScale = 1.5f;

    private TextMeshPro text;
    private float timer;

    public void Setup(int amount, bool isCrit)
    {
        text.text = amount.ToString();

        if (isCrit)
        {
            text.text += "!";
            text.color = critColor;
            transform.localScale *= critScale;
        }
    }
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
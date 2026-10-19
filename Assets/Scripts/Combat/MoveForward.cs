using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float speed = 4f;

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
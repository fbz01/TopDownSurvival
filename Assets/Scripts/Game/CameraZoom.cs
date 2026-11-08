using UnityEngine;

[RequireComponent(typeof(Camera))]

// Scales the camera with the window height so the arena looks the same at any size.
public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float referenceHeight = 720f;
    [SerializeField] private float referenceSize = 4.2f;
    [SerializeField] private float maxSize = 7f;

    private Camera cam;
    private int lastHeight;

    private void Awake() => cam = GetComponent<Camera>();

    private void Update()
    {
        if (Screen.height == lastHeight) return;  // only recalculate when the window size changes
        lastHeight = Screen.height;

        float size = referenceSize * Screen.height / referenceHeight;
        cam.orthographicSize = Mathf.Min(size, maxSize);
    }
}
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    Transform playerTransform;
    private float horizontalOffset;
    private float verticalOffset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        horizontalOffset = transform.position.x - playerTransform.position.x;
        verticalOffset = transform.position.y - playerTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 updatedCameraPosition = transform.position;
        updatedCameraPosition.x = playerTransform.position.x + horizontalOffset;
        updatedCameraPosition.y = playerTransform.position.y + verticalOffset;
        transform.position = updatedCameraPosition;
    }
}

using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Declare player stats
    public int strength {get; private set;}
    public int luck {get; private set;}
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize player stats
        strength = 5;
        luck = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ImproveStrength()
    {
        strength += 5;
    }

    void ImproveLuck()
    {
        luck += 1;
    }
}

using UnityEngine;

// A simple script to bring enemies to life with a bobbing motion.
// "Vibe-coding" style: functional, simple math, easy to read.
public class SimpleEnemy : MonoBehaviour
{
    [Header("Bobbing Settings")]
    // How fast the enemy bobs up and down.
    public float bobSpeed = 2f;

    // How high/low the enemy bobs from its starting position.
    public float bobHeight = 0.5f;

    // Store the original Y position so we can bob around it.
    private float startY;

    // Start is called before the first frame update.
    void Start()
    {
        // Capture the initial vertical position of the enemy.
        // We use this as the center point for the sine wave movement.
        startY = transform.position.y;
    }

    // Update is called once per frame.
    // Good for visual movements that don't rely on strict physics collisions.
    void Update()
    {
        // Calculate the new Y position using a Sine wave.
        // Time.time keeps increasing, creating a continuous wave.
        // We multiply by bobSpeed to control the frequency.
        // We multiply the result (-1 to 1) by bobHeight to control amplitude.
        float newY = startY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        // Apply the new position.
        // We keep X and Z the same, only modifying Y.
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

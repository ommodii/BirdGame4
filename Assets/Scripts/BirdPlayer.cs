using UnityEngine;

// This script controls the bird using physics-based forces.
// It's designed to be "vibecoded": easy to read and tweak in the Inspector.
public class BirdPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    // The constant forward speed of the bird.
    // Public so you can tweak it in the Inspector to find the right pace.
    public float forwardSpeed = 10f;

    // How much upward force is applied when holding Space.
    // Making this public allows for "vibe-checking" the lift feel.
    public float liftForce = 20f;

    // How fast the bird turns left and right.
    // Public to easily adjust responsiveness.
    public float turnSpeed = 100f;

    [Header("Components")]
    // Reference to the Rigidbody component for physics interactions.
    public Rigidbody rb;

    // Start is called before the first frame update.
    void Start()
    {
        // Get the Rigidbody component attached to this GameObject.
        // We need this to apply physics forces like gravity and lift.
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame.
    // We use Update to check for input because it's more responsive than FixedUpdate.
    void Update()
    {
        // We handle input here, but apply physics in FixedUpdate for stability.
        // This keeps the controls feeling snappy.
    }

    // FixedUpdate is called at fixed time intervals.
    // This is where ALL physics calculations (AddForce, Velocity) should happen.
    void FixedUpdate()
    {
        // --- 1. Forward Motion ---
        // We want the bird to always move forward relative to where it's facing.
        // transform.forward gives us the direction the bird is looking.
        // We multiply by forwardSpeed to set the magnitude.
        // We preserve the current Y velocity (rb.velocity.y) so gravity and lift still work naturally.
        Vector3 forwardMove = transform.forward * forwardSpeed;
        rb.linearVelocity = new Vector3(forwardMove.x, rb.linearVelocity.y, forwardMove.z);

        // --- 2. Ascension (The 'Antigravity' feel) ---
        // Check if the Space bar is currently being held down.
        if (Input.GetKey(KeyCode.Space))
        {
            // Apply a localized upward force.
            // Vector3.up is shorthand for (0, 1, 0).
            // ForceMode.Acceleration ignores mass, giving a consistent "lift" feel regardless of bird size.
            rb.AddForce(Vector3.up * liftForce, ForceMode.Acceleration);
        }
        // If Space is NOT held, we do nothing special.
        // The Rigidbody's built-in "Use Gravity" setting will naturally pull the bird down.

        // --- 3. Steering ---
        // Get input from A/D keys or Left/Right arrow keys.
        // Input.GetAxis("Horizontal") returns:
        // -1.0 for Left/A
        // +1.0 for Right/D
        //  0.0 for no input
        float turnInput = Input.GetAxis("Horizontal");

        // Calculate the rotation amount based on input, speed, and fixed delta time.
        // Vector3.up refers to the vertical Y-axis, which we rotate around to turn left/right.
        Vector3 rotation = Vector3.up * turnInput * turnSpeed * Time.fixedDeltaTime;

        // Apply the rotation to the Rigidbody.
        // MoveRotation ensures physics don't break when rotating.
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }

    // This method is automatically called when the Rigidbody hits another Collider.
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we hit has the tag "Enemy".
        // Tags are efficient ways to group objects in Unity.
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // If it is an enemy, destroy that game object immediately.
            // This removes it from the scene effectively "defeating" it.
            Destroy(collision.gameObject);
        }
    }
}

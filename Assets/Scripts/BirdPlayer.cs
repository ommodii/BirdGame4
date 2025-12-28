using UnityEngine;

public class BirdPlayer : MonoBehaviour
{
    [Header("Forward Drive")]
    public float forwardSpeed = 20f;
    public float turnSpeed = 150f;

    [Header("Exponential Lift")]
    public float initialLift = 15f;    // The 'kick' when you first hit space
    public float liftIncrease = 1.1f;  // How fast the lift grows (exponential)
    public float maxUpwardSpeed = 25f;

    [Header("The Heavy Fall")]
    public float gravityMultiplier = 4f; // Makes the "Release" feel snappy

    private Rigidbody rb;
    private float currentLiftTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Setup Rigidbody for "Sharp" feel
        rb.useGravity = true; 
        rb.linearDamping = 0.5f;   // Slight air resistance
        rb.angularDamping = 10f; // Stops spinning the moment you stop turning
    }

    void FixedUpdate()
    {
        // 1. ALWAYS MOVE FORWARD
        // We use MovePosition for the forward drive so it's unstoppable
        Vector3 forwardStep = transform.forward * forwardSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardStep);

        // 2. SNAPPY TURNING
        float turnInput = Input.GetAxis("Horizontal");
        if (turnInput != 0)
        {
            Quaternion turnRotation = Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        // 3. EXPONENTIAL LIFT (Spacebar)
        if (Input.GetKey(KeyCode.Space))
        {
            currentLiftTimer += Time.fixedDeltaTime;
            
            // Calculate exponential lift: initial + (time squared * growth)
            float liftEffort = initialLift + (currentLiftTimer * currentLiftTimer * liftIncrease * 50f);
            liftEffort = Mathf.Min(liftEffort, maxUpwardSpeed);

            // Apply the lift directly to velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, liftEffort, rb.linearVelocity.z);
        }
        else
        {
            currentLiftTimer = 0f; // Reset the "engine"
            
            // 4. THE HEAVY FALL (Gravity Override)
            // If we are falling (or just let go), pull down extra hard
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Debug.Log("🎯 Target Eliminated!");
        }
    }
}
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float bobSpeed = 2f;
    public float bobHeight = 0.5f;
    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public Collider2D personCollider;
    public Transform personTransform;
    public int dirt;
    public int maxDirt;
    public bool isGrounded;


    // Dummy variables for testing
    // public Vector2 targetPosition = new(5, 3);  // Target position (you can change this)
    public float jumpHeight = 3f;     // Jump height (how high the enemy will jump)
    private Rigidbody2D rb;
    public float jumpVelocity = 10f;
    public int targetDist = 2;

    private bool isRight = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxDirt = 1000;
        dirt = maxDirt;

        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        targetDist = 2;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), personCollider);
    }

    // Update is called once per frame
    void Update()
    {
        dirt -= (int)Time.deltaTime;

        if (dirt > 0)
        {
            if (isGrounded)
            {
                Debug.Log("why");
                JumpToPosition(personTransform.position);
            }
        }
    }


    public void JumpToPosition2()
    {
        rb.linearVelocity = new Vector2(2, 3);

        // Save the initial position
        Vector2 initialPosition = transform.position;
        Vector2 targetPosition = new(initialPosition.x - targetDist, initialPosition.y);

        // Calculate horizontal distance to the target position
        float horizontalDistance = Vector2.Distance(new Vector2(initialPosition.x, 0), new Vector2(targetPosition.x, 0));
        float jumpDuration = horizontalDistance / jumpVelocity;  // Time = distance / velocity

        // Calculate the vertical velocity to reach the jump height
        float gravity = Mathf.Abs(Physics2D.gravity.y);  // Get the gravity in 2D
        float verticalSpeed = Mathf.Sqrt(2 * gravity * jumpHeight);  // Calculate vertical speed needed to reach the target height

        // Set the initial velocity for the jump
        rb.linearVelocity = new Vector2(jumpVelocity * Mathf.Sign(targetPosition.x - transform.position.x), verticalSpeed);
    }

    public void JumpToPosition(Vector2 targetPosition)
    {
        Vector2 initialPosition = transform.position;
        // Vector2 targetPosition = new(isRight ? initialPosition.x + targetDist : initialPosition.x - targetDist, initialPosition.y);

        Debug.Log("target" + targetPosition + initialPosition + isRight + targetDist);

        isRight = !isRight;

        // Calculate horizontal distance to the target position
        float d = Vector2.Distance(new Vector2(initialPosition.x, 0), new Vector2(targetPosition.x, 0));

        float g = Mathf.Abs(Physics2D.gravity.y);

        // Step 1: Calculate the vertical component of the initial velocity (v0y)
        float v0y = Mathf.Sqrt(2 * g * jumpHeight);

        // Step 2: Calculate the time to reach max height (t_max)
        float tMax = v0y / g;

        // Step 3: Calculate the total flight time (T)
        float totalTime = 2 * tMax;  // Time to go up and come back down

        // Step 4: Calculate the horizontal component of the initial velocity (v0x)
        float v0x = d / totalTime;

        // Return the initial velocity vector as a Vector2
        Debug.Log("new" + new Vector2(v0x * Mathf.Sign(targetPosition.x - transform.position.x), v0y));
        rb.linearVelocity = new Vector2(v0x * Mathf.Sign(targetPosition.x - transform.position.x), v0y);
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

}

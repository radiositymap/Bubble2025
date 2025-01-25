using System;
using Unity.VisualScripting;
using UnityEngine;

public class Cat : MonoBehaviour
{
    // References
    public Collider2D personCollider;
    public Transform personTransform;

    // Dirt state change
    private SpriteRenderer spriteRenderer;
    public int maxHealth = 9;
    public int health;
    public Sprite[] healthStates;

    // Movement
    private Rigidbody2D rb;
    private bool isGrounded;
    public float jumpHeight = 3f;
    public float jumpVelocity = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // maxHealth = hitsPerState * (healthStates.Length - 1);
        health = maxHealth;

        // Movement
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), personCollider);


    }

    // Update is called once per frame
    void Update()
    {
        // health -= (int)Time.deltaTime;

        if (health > 0)
        {
            if (isGrounded)
            {
                JumpToPosition(personTransform.position);
            }

            int hitsTaken = maxHealth - health;
            spriteRenderer.sprite = healthStates[hitsTaken / maxHealth * (healthStates.Length - 1)];
        }
    }

    public void JumpToPosition(Vector2 targetPosition)
    {
        Vector2 initialPosition = transform.position;

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
        rb.linearVelocity = new Vector2(v0x * Mathf.Sign(targetPosition.x - transform.position.x), v0y);
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Grounded");
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

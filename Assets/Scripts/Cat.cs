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
    private CapsuleCollider2D catCollider;
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
        // Health management
        // maxHealth = hitsPerState * (healthStates.Length - 1);
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        catCollider = GetComponent<CapsuleCollider2D>();

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

            // state change
            float hitPercent = (float)(maxHealth - health) / (float)maxHealth;
            int stateIdx = (int)(hitPercent * (healthStates.Length - 1));
            spriteRenderer.sprite = healthStates[stateIdx];

            // collision adjustment
            // float stateScale = stateIdx switch
            // {
            //     0 => 1f,
            //     1 => 5f / 6f,
            //     2 => 1f / 2f,
            //     _ => 1f
            // };
            // Debug.Log(stateScale * catCollider.size.x);
            // catCollider.size = new Vector2((float)(catCollider.size.x * stateScale), catCollider.size.y);
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
            isGrounded = true;
        }
        if (collider.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("ow!");
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

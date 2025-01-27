using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cat : MonoBehaviour
{
    // References
    public Collider2D personCollider;
    public Transform personTransform;

    // Dirt state change
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D catCollider;
    private BoxCollider2D catTrigger;
    public int maxHealth = 9;
    public int health;
    public Sprite[] healthStates;

    // Movement
    private Rigidbody2D rb;
    private bool isGrounded;
    public float jumpHeight = 5f;
    public float jumpVelocity = 10f;

    Vector2 colliderSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Health management
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        catCollider = transform.Find("Collider").GetComponent<CapsuleCollider2D>();
        catTrigger = transform.Find("Trigger").GetComponent<BoxCollider2D>();
        colliderSize = catCollider.size;

        // Movement
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        Physics2D.IgnoreCollision(catCollider, personCollider);
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

    private void handleHit()
    {
        // state change
        float hitPercent = (float)(maxHealth - health) / (float)maxHealth;
        int stateIdx = (int)(hitPercent * (healthStates.Length - 1));
        spriteRenderer.sprite = healthStates[stateIdx];

        // collision size adjustment
        Vector2 stateScale = stateIdx switch
        {
            0 => Vector2.one,
            1 => new Vector2(1f, 0.6f),
            2 => new Vector2(0.5f, 0.4f),
            _ => new Vector2(0.2f, 0.15f)
        };
        catCollider.size = colliderSize * stateScale;
        catTrigger.size = colliderSize * stateScale;
        if (stateIdx >= 3) {
            catTrigger.enabled = false;
            StartCoroutine(LoadLevel(1));
        }
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (health < 0)
            return;
        if (collider.gameObject.CompareTag("Bullet"))
        {
            health -= 1;
            collider.gameObject.SetActive(false);
            handleHit();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    IEnumerator LoadLevel(int levelId) {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(levelId);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public float bulletTimeout;
    public Gun gun;
    public DirtyScreen dirtyScreen;

    Rigidbody2D rbd;
    Transform weapon;
    float timeout;
    Animator animator;
    BoxCollider2D collider;
    Vector2 colliderSize;

    public int maxHealth = 16;
    public int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        weapon = transform.GetChild(0);
        timeout = bulletTimeout;
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        colliderSize = collider.size;

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float xMotion = Input.GetAxis("Horizontal");
        rbd.AddForce(new Vector2(xMotion * speed, 0));
        if (xMotion < 0 && transform.localEulerAngles.y < 90f)
            transform.localEulerAngles = new Vector3(0, 180f, 0);
        if (xMotion > 0 && transform.localEulerAngles.y > 90f)
            transform.localEulerAngles = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.S)) {
            animator.Play("crouch");
            collider.size = new Vector2(colliderSize.x, colliderSize.y*0.75f);
            collider.offset = new Vector2(0, -0.3f);
        }
        if (Input.GetKeyUp(KeyCode.S)) {
            animator.Play("uncrouch");
            collider.size = colliderSize;
            collider.offset = Vector2.zero;
        }

        float yMotion = Input.GetAxis("Vertical");
        weapon.RotateAround(weapon.position,
            new Vector3(0, 0, -1), yMotion * rotateSpeed);
        weapon.localEulerAngles = new Vector3(0, 0,
            ClampAngle(weapon.localEulerAngles.z, -25.0f, 25.0f));

        if (Input.GetKey(KeyCode.Space)) {
            if (timeout > 0)
                timeout -= Time.deltaTime;
            else {
                if (gun != null)
                    gun.Shoot();
                timeout = bulletTimeout;
            }
        }
    }

    float ClampAngle(float angle, float min, float max) {
        if (angle > 180)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (health < 0)
            return;
        if (collider.gameObject.CompareTag("Cat"))
        {
            health -= 1;
            handleHit();
        }
    }

    private void handleHit()
    {
        // state change
        float hitPercent = (float)(maxHealth - health) / (float)maxHealth;
        int stateIdx = (int)(hitPercent * (dirtyScreen.screenNum - 1));
        dirtyScreen.SetScreen(stateIdx);
  }
}

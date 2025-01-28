using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public float bulletTimeout;
    public Gun gun;
    public Wand wand;
    public DirtyScreen dirtyScreen;
    public DialogueController myDialogue;

    Game game;
    Rigidbody2D rbd;
    Transform weapon;
    float timeout;
    Animator animator;
    BoxCollider2D playerCollider;
    Vector2 colliderSize;
    GameState prevState;

    public int maxHealth = 16;
    public int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = Game.game;
        rbd = GetComponent<Rigidbody2D>();
        weapon = transform.GetChild(0);
        timeout = bulletTimeout;
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        colliderSize = playerCollider.size;

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (game.currentState == GameState.RUNNING) {
            if (prevState == GameState.INTRO)
                myDialogue.PlayDialogue(0);

            float xMotion = Input.GetAxis("Horizontal");
            rbd.AddForce(new Vector2(xMotion * speed, 0));
            if (xMotion < 0 && transform.localEulerAngles.y < 90f)
                transform.localEulerAngles = new Vector3(0, 180f, 0);
            if (xMotion > 0 && transform.localEulerAngles.y > 90f)
                transform.localEulerAngles = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.S)) {
                animator.Play("crouch");
                playerCollider.size = new Vector2(colliderSize.x, colliderSize.y*0.75f);
                playerCollider.offset = new Vector2(0, -0.3f);
            }
            if (Input.GetKeyUp(KeyCode.S)) {
                animator.Play("uncrouch");
                playerCollider.size = colliderSize;
                playerCollider.offset = Vector2.zero;
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

                if (health <= 0)
                    SceneManager.LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (wand != null)
                    wand.MakeBubble();
            }
            if (Input.GetKeyUp(KeyCode.Space)) {
                if (wand != null)
                    wand.StopBubble();
            }
        }
        if (prevState == GameState.RUNNING && game.currentState == GameState.ENDED)
            myDialogue.PlayDialogue(2);

        prevState = game.currentState;
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
        if (collider.gameObject.CompareTag("Finish")) {
            myDialogue.PlayDialogue(1);
            game.currentState = GameState.ENDED;
        }
    }

    private void handleHit()
    {
        // state change
        float hitPercent = (float)(maxHealth - health) / (float)maxHealth;
        int stateIdx = (int)(hitPercent * (dirtyScreen.screenNum - 1));
        dirtyScreen.SetScreen(stateIdx);
        dirtyScreen.DrawRandomSplotch();
        myDialogue.PlayDialogue(1);
  }
}

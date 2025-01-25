using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public float bulletTimeout;
    public Gun gun;

    Rigidbody2D rbd;
    Transform weapon;
    float timeout;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbd = transform.GetComponent<Rigidbody2D>();
        weapon = transform.GetChild(0);
        timeout = bulletTimeout;
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
            // TODO swap sprite
            // reduce collider
            if (transform.localScale.y > 0.6f)
                transform.localScale = new Vector3(1f, 0.5f, 1f);
        }
        if (Input.GetKeyUp(KeyCode.S)) {
            if (transform.localScale.y < 0.6f)
                transform.localScale = Vector3.one;
        }

        float yMotion = Input.GetAxis("Vertical");
        weapon.RotateAround(transform.position,
            new Vector3(0, 0, -1), yMotion * rotateSpeed);

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
}

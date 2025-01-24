using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public Gun gun;
    Rigidbody2D rbd;

    Transform weapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbd = transform.GetComponent<Rigidbody2D>();
        weapon = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        float xMotion = Input.GetAxis("Horizontal");
        float yMotion = Input.GetAxis("Vertical");
        rbd.AddForce(new Vector2(xMotion * speed, 0));
        weapon.RotateAround(weapon.position,
            new Vector3(0, 0, 1), yMotion * rotateSpeed);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (gun != null) {
                gun.Shoot();
            }
        }
    }
}

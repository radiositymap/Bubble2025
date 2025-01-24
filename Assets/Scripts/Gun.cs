using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;

    public void Shoot() {
        GameObject newBullet = Instantiate(bullet,
            transform.position, Quaternion.identity);
        Vector3 bulletForce = transform.right * bulletSpeed;
        newBullet.GetComponent<Rigidbody2D>().AddForce(bulletForce);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;

    public List<GameObject> bullets = new List<GameObject>();

    public void Shoot() {
        GameObject newBullet = GetBullet(transform.position);
        if (newBullet == null)
            return;
        Vector3 bulletForce = transform.right * bulletSpeed;
        newBullet.GetComponent<Rigidbody2D>().AddForce(bulletForce);
    }

    public GameObject GetBullet(Vector3 pos){
        GameObject newBullet = null;
        foreach (GameObject bullet in bullets) {
            if (!bullet.activeInHierarchy) {
                bullet.SetActive(true);
                bullet.transform.SetPositionAndRotation(pos, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
                newBullet = bullet;
                break;
            }
        }
        if (newBullet == null) {
            newBullet = Instantiate(bullet, pos, Quaternion.identity);
            newBullet.SetActive(true);
            bullets.Add(newBullet);
        }
        return newBullet;
    }

    public void Update(){
        foreach (GameObject bullet in bullets) {
            if (Math.Abs(bullet.transform.position.x)  > 20 ||
                Math.Abs(bullet.transform.position.y)  > 20) {
                    bullet.SetActive(false);
            }
        }
   }

}

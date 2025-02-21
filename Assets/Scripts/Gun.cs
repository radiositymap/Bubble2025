using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour, IPointerClickHandler
{
    public GameObject bullet;
    public Transform shootingPoint;
    public float bulletSpeed;
    public Action OnClick;

    List<GameObject> bullets = new List<GameObject>();

    public void Shoot()
    {
        GameObject newBullet = GetBullet(shootingPoint.position);
        if (newBullet == null)
            return;
        Vector3 bulletForce = transform.right * bulletSpeed;
        newBullet.GetComponent<Rigidbody2D>().AddForce(bulletForce);
    }

    public GameObject GetBullet(Vector3 pos) {
        GameObject newBullet = null;
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                bullet.transform.SetPositionAndRotation(pos, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
                newBullet = bullet;
                break;
            }
        }
        if (newBullet == null)
        {
            newBullet = Instantiate(bullet, pos, Quaternion.identity);
            newBullet.SetActive(true);
            bullets.Add(newBullet);

            bullet.AddComponent<CircleCollider2D>();
            bullet.tag = "Bullet";
        }
        return newBullet;
    }

    public void Update()
    {
        foreach (GameObject bullet in bullets)
        {
            if (Math.Abs(bullet.transform.position.x) > 20 ||
                Math.Abs(bullet.transform.position.y) > 20)
            {
                bullet.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i=0; i<10; i++)
            OnClick();
    }
}

using System.Runtime;
using UnityEngine;

public class BigBubble : MonoBehaviour
{
    public float obstacleThreshold = 1.5f;
    public float myThreshold = 2.3f;
    GameObject bubbled;

    void OnTriggerEnter2D(Collider2D collider) {
        if (bubbled == null && collider.CompareTag("Obstacle")) {
            if (transform.localScale.x < obstacleThreshold)
                return;
            bubbled= collider.gameObject;
            collider.enabled = false;
            collider.GetComponent<Rigidbody2D>().bodyType =
                RigidbodyType2D.Kinematic;
            bubbled.transform.SetParent(transform, true);
            bubbled.transform.localPosition = Vector3.zero;
            bubbled.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
        }

        if (bubbled == null && collider.CompareTag("Player")) {
            if (transform.localScale.x < myThreshold)
                return;
            bubbled = collider.gameObject;
            collider.enabled = false;
            collider.GetComponent<Rigidbody2D>().bodyType =
                RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            bubbled.transform.SetParent(transform, true);
            bubbled.transform.localPosition = Vector3.zero;
            bubbled.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
        }
    }

    void Update() {
        if (transform.position.y > 20f)
            Destroy(gameObject);
        if (bubbled != null && bubbled.CompareTag("Player")) {
            Camera.main.transform.position = new Vector3(
                bubbled.transform.position.x,
                bubbled.transform.position.y,
                -10f);
        }
    }
}

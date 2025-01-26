using UnityEngine;

public class SmallBubble : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Bullet") &&
            !collision.gameObject.CompareTag("Cat")) {
                gameObject.SetActive(false);
        }
    }
}

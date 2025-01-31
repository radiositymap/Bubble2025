using Unity.Mathematics;
using UnityEngine;

public class Wand : MonoBehaviour
{
    public GameObject bubble;
    public Transform shootingPoint;
    public float growthRate;
    public float shootForce;
    GameObject newBubble;
    Rigidbody2D bubbleRbd;

    public void MakeBubble() {
        newBubble = Instantiate(bubble, shootingPoint.position, Quaternion.identity);
        bubbleRbd = newBubble.GetComponent<Rigidbody2D>();
        bubbleRbd.bodyType = RigidbodyType2D.Kinematic;
        newBubble.SetActive(true);
    }

    public void StopBubble() {
        if (newBubble == null)
            return;
        newBubble.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        bubbleRbd.AddForce(transform.right * shootForce);
        newBubble = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (newBubble != null) {
            newBubble.transform.localScale =
                new Vector3(newBubble.transform.localScale.x + growthRate,
                newBubble.transform.localScale.y + growthRate, 1f);
        }
    }
}

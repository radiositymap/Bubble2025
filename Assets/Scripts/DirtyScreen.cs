using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SubsystemsImplementation.Extensions;

public class DirtyScreen : MonoBehaviour
{
    public int screenNum = 4;
    public List<Sprite> splotches;
    public GameObject splotchObject;
    List<GameObject> dirtyScreens = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in transform)
            dirtyScreens.Add(child.gameObject);
    }

    public void SetScreen(int screenId) {
        foreach (GameObject screen in dirtyScreens)
            screen.SetActive(false);
        dirtyScreens[screenId].SetActive(true);
    }

    public void DrawRandomSplotch() {
        int splotchId = Random.Range(0, splotches.Count);
        Vector3 offset = new Vector3(
            Random.Range(-5, 5), Random.Range(-5, 5), 0);
        StartCoroutine(DrawSplotch(splotchId, offset));
    }

    IEnumerator DrawSplotch(int splotchId, Vector3 offset) {
        GameObject newSplotch =
            Instantiate(splotchObject, offset, Quaternion.identity, transform);
        Image img = newSplotch.GetComponent<Image>();
        img.sprite = splotches[splotchId];
        newSplotch.SetActive(true);

        yield return new WaitForSeconds(1f);

        Color c = img.color;
        while (img.color.a > 0) {
            img.color = new Color(c.r, c.g,  c.b, img.color.a-0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(newSplotch);
    }
}

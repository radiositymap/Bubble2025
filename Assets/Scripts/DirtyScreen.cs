using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation.Extensions;

public class DirtyScreen : MonoBehaviour
{
    public float screenNum = 4;
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
}

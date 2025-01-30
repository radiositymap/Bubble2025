using UnityEngine;
using UnityEngine.UIElements;

public class PauseController : MonoBehaviour
{
    public GameObject controlsPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (controlsPanel.activeSelf)
            {
                Debug.Log("Resume");
                controlsPanel.SetActive(false);
            }
            else
            {
                Debug.Log("Pause");
                controlsPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}

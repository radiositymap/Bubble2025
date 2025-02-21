using UnityEngine;
using UnityEngine.UIElements;

public class PauseController : MonoBehaviour
{
    public GameObject controlsPanel;
    GameState prevState = GameState.RUNNING;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (controlsPanel.activeSelf)
            {
                Debug.Log("Resume");
                Resume();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (controlsPanel.activeSelf)
            {
                Debug.Log("Resume");
                Resume();
            }
            else
            {
                Debug.Log("Pause");
                Pause();
            }
        }
    }

    public void Pause() {
        controlsPanel.SetActive(true);
        Time.timeScale = 0;
        prevState = Game.game.currentState;
        Game.game.currentState = GameState.PAUSED;
    }

    public void Resume() {
        controlsPanel.SetActive(false);
        Time.timeScale = 1f;
        Game.game.currentState = prevState;
    }
}

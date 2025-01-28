using UnityEngine;

public class Game : MonoBehaviour
{
    public bool skipIntro;
    public GameState currentState = GameState.INTRO;

    public static Game game {get; private set;}

    void Awake() {
        if (game != null && game != this)
            Destroy(this);
        else
            game = this;
        currentState = skipIntro ? GameState.RUNNING : GameState.INTRO;
    }
}

public enum GameState {
    INTRO,
    RUNNING,
    PAUSED,
    ENDED
}

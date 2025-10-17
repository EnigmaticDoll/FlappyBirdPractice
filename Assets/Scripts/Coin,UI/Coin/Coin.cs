using UnityEngine;

public class Coin : MonoBehaviour
{
    GameManager.GameState gameStateCache;

    private ScoreUIManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreUIManager>();
    }

    void OnEnable()
    {
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreUIManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SendMessageUpwards("OnPlayerCollectCoin", this.gameObject);
        }
    }

    private void OnGameStateChange(GameManager.GameState gameState)
    {
        gameStateCache = gameState;
        // TODO: this function is called when GameManager changes its game state.
        //       so cache this value and use that value in the FixedUpdate and Update function.
        // TODO: because the cached value do not cover other non-script components such as animators
        //       and rigidbodies, we need to disable and enable those components in this function.
        //       enable such components when the game is resummed and disable them when the game is paused.
        switch (gameState)
        {
            case GameManager.GameState.GameReady:
                break;
            case GameManager.GameState.Ongoing:
                break;
            case GameManager.GameState.GameOver:
                break;
            case GameManager.GameState.Paused:
                break;
        }
    }
}
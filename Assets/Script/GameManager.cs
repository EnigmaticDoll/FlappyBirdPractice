using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        GameReady,
        Ongoing,
        Paused,
        GameOver
    }

    public static GameManager instance { get; private set; } = null;

    // private UIManager uiManager;
    // private MapManager mapManager;
    private Player player;

    private ObjectPool coinPool;
    private ObjectPool pipePool;

    public float gameTime { get; private set; } = 0;
    public uint gameScore { get; private set; } = 0;
    public GameState gameState { get; private set; }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        // TODO: instantiate player and manager objects
    }

    private void FixedUpdate()
    {
        switch(gameState)
        {
            case GameState.GameReady:
                if (Input.GetMouseButtonDown(0))
                {
                    gameState = GameState.Ongoing;
                    player.gameObject.SetActive(false);
                    player.gameObject.SetActive(true);
                }
                break;
            case GameState.Ongoing:
                if (player.isDead)
                {
                    gameState = GameState.GameOver;
                }
                else
                {
                    gameTime += Time.fixedDeltaTime;
                    gameScore = (uint)gameTime;
                }
                // do we need to implement path for entering pause state?
                break;
            case GameState.GameOver:
                // need to implement path for entering GameReady state
                break;
            case GameState.Paused:
                // not implemented
                break;
        }
    }
}

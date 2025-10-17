using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

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

    [Header("Components")]
    [SerializeField] private ScoreUIManager scoreUIManager;
    [SerializeField] private MapController mapController;
    [SerializeField] private Player player;

    public uint globalGameScore { get; private set; } = 0;
    public GameState globalGameState { get; private set; }

    private void Awake()
    {
        if (null == instance) DontDestroyOnLoad((instance = this).gameObject);
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        globalGameState = GameState.GameReady;
        BroadcastMessage("OnGameStateChange", globalGameState);
    }

    private void LateUpdate()
    {
        switch (globalGameState)
        {
            case GameState.GameReady:
                if (Input.GetMouseButtonDown(0))
                {
                    globalGameState = GameState.Ongoing;
                    BroadcastMessage("OnGameStateChange", globalGameState);
                }
                break;
            case GameState.Ongoing:
                if (null != player && player.isDead)
                {
                    globalGameState = GameState.GameOver;
                    BroadcastMessage("OnGameStateChange", globalGameState);
                }
                // do we need to implement path for entering pause state?
                // if so, implement buttons with SendMessageUpwards.
                break;
            case GameState.GameOver:
                // press reset button to restart game, see OnPressRestartButton
                break;
            case GameState.Paused:
                // not implemented yet.
                break;
        }
    }

    public void OnPlayerCollectCoin(GameObject coin)
    {
        scoreUIManager.OnCoinCollected();
        
    }

    private void OnPressRestartButton()
    {
        globalGameState = GameState.GameReady;
        BroadcastMessage("OnGameStateChange", globalGameState);
    }
}

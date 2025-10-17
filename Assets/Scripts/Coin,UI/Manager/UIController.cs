using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject gameStartUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("GameOver Images")]
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject scoreResultImage;
    [SerializeField] private GameObject okButton;
    [SerializeField] private GameOverResultUI gameOverResultUI;
    [SerializeField] private float gameOverDelay = 0.5f;

    GameManager.GameState gameStateCache;

    void Awake()
    {
        Instance = this;
    }

    ////===========================System Test Key========================
    //void Update()
    //{
    //    if (gameStartUI != null && gameStartUI.activeSelf)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            HideGameStartUI();
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        ScoreUIManager scoreManager = FindObjectOfType<ScoreUIManager>();
    //        if (scoreManager != null)
    //        {
    //            scoreManager.AddScore(1);
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        ShowGameOverUI();
    //    }
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        RestartGame();
    //    }
    //}

    public void ShowGameStartUI()
    {
        if (gameStartUI != null) gameStartUI.SetActive(true);
    }

    public void HideGameStartUI()
    {
        if (gameStartUI != null) gameStartUI.SetActive(false);
    }

    //public void RestartGame()
    //{
    //    if (gameOverUI != null)
    //        gameOverUI.SetActive(false);
    //    if (gameOverImage != null)
    //        gameOverImage.SetActive(false);
    //    if (scoreResultImage != null)
    //        scoreResultImage.SetActive(false);
    //    if (okButton != null) 
    //        okButton.SetActive(false);

    //    ScoreUIManager scoreManager = FindObjectOfType<ScoreUIManager>();
    //    if (scoreManager != null)
    //    {
    //        scoreManager.ResetScore();
    //    }
    //    mapController.InitializeMap();
    //    if (player != null)
    //        player.ResetPlayer();
    //    ShowGameStartUI();
    //}


    public void ShowGameOverUI()
    {
        if (gameOverUI != null) gameOverUI.SetActive(true);
        StartCoroutine(ShowGameOverImagesWithDelay());
    }
    public void HideGameOverUI()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameOverImage != null) gameOverImage.SetActive(false);
        if (scoreResultImage != null) scoreResultImage.SetActive(false);
        if (okButton != null) okButton.SetActive(false);
    }

    private IEnumerator ShowGameOverImagesWithDelay()
    {
        yield return new WaitForSeconds(gameOverDelay);
        if (gameOverImage != null)
            gameOverImage.SetActive(true);
        if (scoreResultImage != null)
            scoreResultImage.SetActive(true);
        if (okButton != null)
            okButton.SetActive(true);

        ScoreUIManager scoreManager = FindObjectOfType<ScoreUIManager>();
        if (scoreManager != null && gameOverResultUI != null)
        {
            gameOverResultUI.ShowResult(scoreManager.GetCurrentScore());
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
        switch(gameState)
        {
            case GameManager.GameState.GameReady:
                ShowGameStartUI();
                HideGameOverUI();
                break;
            case GameManager.GameState.Ongoing:
                if (gameStartUI != null) gameStartUI.SetActive(false);
                break;
            case GameManager.GameState.GameOver:
                ShowGameOverUI();
                break;
            case GameManager.GameState.Paused:
                break;
        }
    }
}
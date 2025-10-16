using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameStartUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("GameOver Images")]
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject scoreResultImage;
    [SerializeField] private GameOverResultUI gameOverResultUI;
    [SerializeField] private float gameOverDelay = 0.5f;

    void Start()
    {
        ShowGameStartUI();

        if (gameOverImage != null)
            gameOverImage.SetActive(false);
        if (scoreResultImage != null)
            scoreResultImage.SetActive(false);
    }

    //===========================System Test Key========================
    void Update()
    {
        if (gameStartUI != null && gameStartUI.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HideGameStartUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ScoreUIManager scoreManager = FindObjectOfType<ScoreUIManager>();
            if (scoreManager != null)
            {
                scoreManager.AddScore(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ShowGameOverUI();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void HideGameStartUI()
    {
        if (gameStartUI != null)
            gameStartUI.SetActive(false);
    }

    public void RestartGame()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
        if (gameOverImage != null)
            gameOverImage.SetActive(false);
        if (scoreResultImage != null)
            scoreResultImage.SetActive(false);

        ScoreUIManager scoreManager = FindObjectOfType<ScoreUIManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }

        ShowGameStartUI();
    }

    public void ShowGameStartUI()
    {
        if (gameStartUI != null)
            gameStartUI.SetActive(true);
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public void ShowGameOverUI()
    {
        if (gameStartUI != null)
            gameStartUI.SetActive(false);
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        StartCoroutine(ShowGameOverImagesWithDelay());
    }

    private IEnumerator ShowGameOverImagesWithDelay()
    {
        yield return new WaitForSeconds(gameOverDelay);

        if (gameOverImage != null)
            gameOverImage.SetActive(true);
        if (scoreResultImage != null)
            scoreResultImage.SetActive(true);

        ScoreUIManager scoreManager = FindObjectOfType<ScoreUIManager>();
        if (scoreManager != null && gameOverResultUI != null)
        {
            gameOverResultUI.ShowResult(scoreManager.GetCurrentScore());
        }
    }
}
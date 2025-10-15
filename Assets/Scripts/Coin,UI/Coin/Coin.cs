using UnityEngine;

// ==================== 코인 스크립트 ====================
public class Coin : MonoBehaviour
{
    private ScoreUIManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreUIManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 점수 추가
            if (scoreManager != null)
            {
                scoreManager.OnCoinCollected();
            }

            // 코인 제거
            Destroy(gameObject);
        }
    }
}
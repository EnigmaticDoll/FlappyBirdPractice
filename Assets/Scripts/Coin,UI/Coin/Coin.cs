using UnityEngine;

// ==================== ���� ��ũ��Ʈ ====================
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
            // ���� �߰�
            if (scoreManager != null)
            {
                scoreManager.OnCoinCollected();
            }

            // ���� ����
            Destroy(gameObject);
        }
    }
}
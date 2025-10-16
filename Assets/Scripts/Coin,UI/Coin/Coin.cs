using UnityEngine;

public class Coin : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 2f;
    //[SerializeField] private float destroyX = -10f;

    private ScoreUIManager scoreManager;
    //private ObjectPool parentPool;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreUIManager>();
    }

    void OnEnable()
    {
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreUIManager>();
    }

    //void Update()
    //{
    //    transform.position += Vector3.left * moveSpeed * Time.deltaTime;

    //    if (transform.position.x < destroyX)
    //    {
    //        ReturnToPool();
    //    }
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (scoreManager != null)
            {
                scoreManager.OnCoinCollected();
            }
            SendMessageUpwards("OnPlayerCollectCoin", this.gameObject);
        }
    }

    //public void SetPool(ObjectPool pool)
    //{
    //    parentPool = pool;
    //}

    //private void ReturnToPool()
    //{
    //    if (parentPool != null)
    //    {
    //        parentPool.ReturnObject(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
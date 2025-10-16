using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float destroyX = -10f;

    private ObjectPool parentPool;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < destroyX)
        {
            ReturnToPool();
        }
    }

    public void SetPool(ObjectPool pool)
    {
        parentPool = pool;
    }

    private void ReturnToPool()
    {
        if (parentPool != null)
        {
            parentPool.ReturnObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
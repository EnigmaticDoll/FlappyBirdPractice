using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;

public class MapController : MonoBehaviour
{
    [Header("Object Pools")]
    [SerializeField] private ObjectPool pipePool;
    [SerializeField] private ObjectPool coinPool;

    [Header("Base Prefab")]
    [SerializeField] private GameObject basePrefab;

    [Header("Base Settings")]
    [SerializeField] private float baseScrollSpeed = 2f;
    [SerializeField] private float baseYPosition = -4f;

    [Header("Spawn Settings")]
    [SerializeField] private float pipeSpawnInterval = 2f;
    [SerializeField] private float coinSpawnInterval = 3f;
    [SerializeField] private float pipeMinY = 0.5f;
    [SerializeField] private float pipeMaxY = 2f;
    [SerializeField] private float coinMinY = -2f;
    [SerializeField] private float coinMaxY = 3f;

    private Queue<GameObject> activePipes;
    private Queue<GameObject> activeCoins;
    private GameObject base1;
    private GameObject base2;
    private bool isGameRunning = false;
    private float pipeTimer = 0f;
    private float coinTimer = 0f;
    private float spawnOffset;
    private float baseWidth;

    void Update()
    {
        if (!isGameRunning) return;

        UpdateBaseScroll(Time.deltaTime);

        pipeTimer += Time.deltaTime;
        if (pipeTimer >= pipeSpawnInterval)
        {
            SpawnPipe();
            pipeTimer = 0f;
        }

        coinTimer += Time.deltaTime;
        if (coinTimer >= coinSpawnInterval)
        {
            SpawnCoin();
            coinTimer = 0f;
        }
    }

    public void InitializeMap()
    {
        if (activePipes == null) activePipes = new Queue<GameObject>();
        if (activeCoins == null) activeCoins = new Queue<GameObject>();

        pipeTimer = pipeSpawnInterval;
        coinTimer = coinSpawnInterval;

        baseWidth = basePrefab.GetComponent<SpriteRenderer>().size.x * basePrefab.transform.lossyScale.x;
        spawnOffset = Camera.main.orthographicSize * Camera.main.aspect+5f;

        ReturnAllObjectsToPool();
        CreateBaseObjects();
    }

    private void ReturnAllObjectsToPool()
    {
        while (activePipes.Count > 0)
        {
            GameObject pipe = activePipes.Dequeue();
            if (pipe != null) pipePool.ReturnObject(pipe);
        }

        while (activeCoins.Count > 0)
        {
            GameObject coin = activeCoins.Dequeue();
            if (coin != null) coinPool.ReturnObject(coin);
        }
    }

    public void StartMapProgression()
    {
        isGameRunning = true;
    }

    public void StopMapProgression()
    {
        isGameRunning = false;
    }

    private void UpdateBaseScroll(float time)
    {
        if (base1 == null || base2 == null) return;

        base1.transform.Translate(Vector2.left * baseScrollSpeed * Time.deltaTime);
        base2.transform.Translate(Vector2.left * baseScrollSpeed * Time.deltaTime);

        if (base1.transform.position.x <= -baseWidth)
        {
            base1.transform.SetPositionAndRotation(
                Vector3.up * baseYPosition + Vector3.right * baseWidth,
                Quaternion.identity);
        }

        if (base2.transform.position.x <= -baseWidth)
        {
            base2.transform.SetPositionAndRotation(
                Vector3.up * baseYPosition + Vector3.right * baseWidth,
                Quaternion.identity);
        }
    }

    private void CreateBaseObjects()
    {
        if (base1 != null) Destroy(base1);
        if (base2 != null) Destroy(base2);

        base1 = Instantiate(basePrefab, new Vector3(0, baseYPosition, 0), Quaternion.identity, this.transform);
        base2 = Instantiate(basePrefab, new Vector3(baseWidth, baseYPosition, 0), Quaternion.identity, this.transform);
    }

    private void SpawnPipe()
    {
        GameObject pipe = pipePool.GetObject(this.transform);

        if (pipe != null)
        {
            float randomY = Random.Range(pipeMinY, pipeMaxY);
            pipe.transform.position = new Vector3(spawnOffset, randomY, 0);
            pipe.SetActive(true);

            Pipe pipeScript = pipe.GetComponent<Pipe>();
            if (pipeScript != null)
            {
                pipeScript.SetPool(pipePool);
            }

            activePipes.Enqueue(pipe);
        }
    }

    private void SpawnCoin()
    {
        GameObject coin = coinPool.GetObject();

        if (coin != null)
        {
            float randomY = Random.Range(coinMinY, coinMaxY);
            coin.transform.position = new Vector3(spawnOffset, randomY, 0);
            coin.SetActive(true);

            Coin coinScript = coin.GetComponent<Coin>();
            if (coinScript != null)
            {
                coinScript.SetPool(coinPool);
            }

            activeCoins.Enqueue(coin);
        }
    }
}
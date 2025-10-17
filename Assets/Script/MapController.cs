using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;

public class MapController : MonoBehaviour
{
    [Header("Common Parameter")]
    [SerializeField] private float mapSpeed = 2f;

    [Header("Object Pool")]
    [SerializeField] private ObjectPool pipePool;
    [SerializeField] private ObjectPool coinPool;

    [Header("Base Parameter")]
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private float baseYPosition = -4.5f;

    [Header("Spawn Parameter")]
    [SerializeField] private float pipeInitialSpawnPositionX = 2f;
    [SerializeField] private float coinInitialSpawnPositionX = 4f;
    [SerializeField] private float pipeSpawnOffset = 4f;
    [SerializeField] private float coinSpawnOffset = 4f;
    [SerializeField] private float pipeMinY = -1.5f;
    [SerializeField] private float pipeMaxY = 1.5f;
    [SerializeField] private float coinMinY = -1.2f;
    [SerializeField] private float coinMaxY = 1.2f;

    GameManager.GameState gameStateCache;
    private LinkedList<GameObject> activePipes;
    private LinkedList<GameObject> activeCoins;
    private GameObject base1;
    private GameObject base2;
    private float runtimeSpawnPositionX;
    private float runtimeDespawnPositionX;
    private float baseWidth;

    public void OnGameReady()
    {
        if (activePipes == null) activePipes = new LinkedList<GameObject>();
        if (activeCoins == null) activeCoins = new LinkedList<GameObject>();

        ReturnAllObjectsToPool();

        baseWidth = basePrefab.GetComponent<SpriteRenderer>().size.x * basePrefab.transform.lossyScale.x;

        float pipeSpriteWidth = 0;
        try
        {
            SpriteRenderer[] renderers = pipePool.pooledPrefab.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer r in renderers) pipeSpriteWidth = Mathf.Max(pipeSpriteWidth, r.size.x);
        }
        catch { Debug.LogWarning("Cannot access the SpriteRenderer component for the pooled prefab of pipePool."); throw; }

        float coinSpriteWidth = 0;
        try
        {
            SpriteRenderer[] renderers = coinPool.pooledPrefab.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer r in renderers) coinSpriteWidth = Mathf.Max(coinSpriteWidth, r.size.x);
        }
        catch { Debug.LogWarning("Cannot access the SpriteRenderer component for the pooled prefab of coinPool."); throw; }

        runtimeSpawnPositionX = Camera.main.orthographicSize * Camera.main.aspect + Mathf.Max(pipeSpriteWidth, coinSpriteWidth) / 2;
        runtimeDespawnPositionX = -runtimeSpawnPositionX;

        InitializeBase();
        InitializePipesAndCoins();
    }

    private void ReturnAllObjectsToPool()
    {
        while (activePipes.First != null)
        {
            pipePool.ReturnObject(activePipes.First.Value);
            activePipes.RemoveFirst();
        }

        while (activeCoins.First != null)
        {
            coinPool.ReturnObject(activeCoins.First.Value);
            activeCoins.RemoveFirst();
        }
    }

    private void InitializeBase()
    {
        if (base1 != null) Destroy(base1);
        if (base2 != null) Destroy(base2);

        base1 = Instantiate(basePrefab, new Vector3(0, baseYPosition, 0), Quaternion.identity, this.transform);
        base2 = Instantiate(basePrefab, new Vector3(baseWidth, baseYPosition, 0), Quaternion.identity, this.transform);
    }

    private void InitializePipesAndCoins()
    {
        for (float x = pipeInitialSpawnPositionX; x < runtimeSpawnPositionX; x += pipeSpawnOffset)
        {
            SpawnPipe(x, Random.Range(pipeMinY, pipeMaxY));
        }

        for (float x = coinInitialSpawnPositionX; x < runtimeSpawnPositionX; x += coinSpawnOffset)
        {
            SpawnCoin(x, Random.Range(coinMinY, coinMaxY));
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.GameState.Ongoing != gameStateCache) return;

        FixedUpdateBaseScroll(Time.fixedDeltaTime);
        FixedUpdatePipesAndCoins(Time.fixedDeltaTime);

        while (activePipes.Last != null && activePipes.Last.Value.transform.position.x < runtimeSpawnPositionX)
        {
            SpawnPipe(activePipes.Last.Value.transform.position.x + pipeSpawnOffset, Random.Range(pipeMinY, pipeMaxY));
        }

        while (activeCoins.Last != null && activeCoins.Last.Value.transform.position.x < runtimeSpawnPositionX)
        {
            SpawnCoin(activeCoins.Last.Value.transform.position.x + coinSpawnOffset, Random.Range(coinMinY, coinMaxY));
        }
    }

    private void FixedUpdateBaseScroll(float time)
    {
        if (base1 == null || base2 == null) return;

        base1.transform.Translate(Vector2.left * mapSpeed * time);
        base2.transform.Translate(Vector2.left * mapSpeed * time);

        if (base1.transform.position.x < -baseWidth)
        {
            base1.transform.SetPositionAndRotation(
                Vector3.up * baseYPosition + Vector3.right * (base2.transform.position.x + baseWidth),
                Quaternion.identity);
        }

        if (base2.transform.position.x < -baseWidth)
        {
            base2.transform.SetPositionAndRotation(
                Vector3.up * baseYPosition + Vector3.right * (base1.transform.position.x + baseWidth),
                Quaternion.identity);
        }
    }

    private void FixedUpdatePipesAndCoins(float time)
    {
        foreach (GameObject obj in activePipes)
        {
            obj.transform.Translate(Vector2.left * mapSpeed * time);
        }
        while (activePipes.First != null && activePipes.First.Value.transform.position.x < runtimeDespawnPositionX)
        {
            pipePool.ReturnObject(activePipes.First.Value);
            activePipes.RemoveFirst();
        }

        foreach (GameObject obj in activeCoins)
        {
            obj.transform.Translate(Vector2.left * mapSpeed * time);
        }
        while (activeCoins.First != null && activeCoins.First.Value.transform.position.x < runtimeDespawnPositionX)
        {
            coinPool.ReturnObject(activeCoins.First.Value);
            activeCoins.RemoveFirst();
        }
    }

    private void SpawnPipe(float x, float y)
    {
        GameObject pipe = pipePool.GetObject(this.transform);

        if (pipe != null)
        {
            pipe.transform.position = new Vector2(x, y);
            pipe.SetActive(true);
            activePipes.AddLast(pipe);
        }
    }

    private void SpawnCoin(float x, float y)
    {
        GameObject coin = coinPool.GetObject(this.transform);

        if (coin != null)
        {
            coin.transform.position = new Vector2(x, y);
            coin.SetActive(true);
            activeCoins.AddLast(coin);
        }
    }

    public void OnPlayerCollectCoin(GameObject coin)
    {
        LinkedListNode<GameObject> node = activeCoins.Find(coin);
        if (node != null)
        {
            coinPool.ReturnObject(node.Value);
            activeCoins.Remove(coin);
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
                OnGameReady();
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
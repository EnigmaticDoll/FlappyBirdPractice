using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreUIManager : MonoBehaviour
{
    [Header("Number Sprites")]
    [SerializeField] private Sprite[] numberSprites;

    [Header("UI")]
    [SerializeField] private Transform scoreParent;
    [SerializeField] private float numberSpacing = 50f;
    [SerializeField] private Vector2 numberSize = new Vector2(40f, 60f);

    [Header("Pool Size")]
    [SerializeField] private int poolSize = 10; 

    private Queue<Image> numberPool = new Queue<Image>();
    private List<Image> activeNumbers = new List<Image>();
    private int currentScore = 0;

    void Start()
    {
        InitializePool();
        UpdateScoreDisplay(0);
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject numberObj = new GameObject($"Number_{i}");
            numberObj.transform.SetParent(scoreParent);

            Image img = numberObj.AddComponent<Image>();
            RectTransform rect = numberObj.GetComponent<RectTransform>();
            rect.sizeDelta = numberSize;

            numberObj.SetActive(false);
            numberPool.Enqueue(img);
        }
    }

    Image GetNumberFromPool()
    {
        if (numberPool.Count > 0)
        {
            Image img = numberPool.Dequeue();
            img.gameObject.SetActive(true);
            return img;
        }
        else
        {
            GameObject numberObj = new GameObject("Number_Extra");
            numberObj.transform.SetParent(scoreParent);

            Image img = numberObj.AddComponent<Image>();
            RectTransform rect = numberObj.GetComponent<RectTransform>();
            rect.sizeDelta = numberSize;

            return img;
        }
    }
    void ReturnNumberToPool(Image img)
    {
        img.gameObject.SetActive(false);
        numberPool.Enqueue(img);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        currentScore = Mathf.Clamp(currentScore, 0, 999);
        UpdateScoreDisplay(currentScore);
    }
    


    public void OnCoinCollected()
    {
        AddScore(1);
    }

    void UpdateScoreDisplay(int score)
    {
        foreach (Image img in activeNumbers)
        {
            ReturnNumberToPool(img);
        }
        activeNumbers.Clear();

        string scoreStr = score.ToString();
        int digitCount = scoreStr.Length;

        float totalWidth = (digitCount - 1) * numberSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < digitCount; i++)
        {
            int digit = int.Parse(scoreStr[i].ToString());
            Image numberImg = GetNumberFromPool();
            numberImg.sprite = numberSprites[digit];

            RectTransform rect = numberImg.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(startX + (i * numberSpacing), 0);

            activeNumbers.Add(numberImg);
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay(0);
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
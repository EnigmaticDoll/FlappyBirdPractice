using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameOverResultUI : MonoBehaviour
{
    [Header("Number Sprites")]
    [SerializeField] private Sprite[] numberSprites;

    [Header("Score Result UI")]
    [SerializeField] private Transform normalScoreParent;
    [SerializeField] private Transform bestScoreParent;
    [SerializeField] private Transform medalParent;

    [Header("Number Settings")]
    [SerializeField] private float numberSpacing = 30f;
    [SerializeField] private Vector2 numberSize = new Vector2(30f, 45f);

    [Header("Score Offsets")]
    [SerializeField] private Vector2 normalScoreOffset = Vector2.zero;
    [SerializeField] private Vector2 bestScoreOffset = Vector2.zero;

    [Header("Medal Prefabs")]
    [SerializeField] private GameObject silverMedalPrefab;
    [SerializeField] private GameObject goldMedalPrefab;

    private const string BEST_SCORE_KEY = "BestScore";

    private List<Image> normalScoreImages = new List<Image>();
    private List<Image> bestScoreImages = new List<Image>();
    private GameObject currentMedal;

    public void ShowResult(int currentScore)
    {
        int bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        bool isNewRecord = currentScore > bestScore;

        if (currentMedal != null)
        {
            Destroy(currentMedal);
        }

        if (isNewRecord)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);
            PlayerPrefs.Save();

            if (medalParent != null && goldMedalPrefab != null)
            {
                currentMedal = Instantiate(goldMedalPrefab, medalParent);
                currentMedal.transform.localPosition = Vector3.zero;
                currentMedal.transform.localScale = Vector3.one;
            }
        }
        else
        {
            if (medalParent != null && silverMedalPrefab != null)
            {
                currentMedal = Instantiate(silverMedalPrefab, medalParent);
                currentMedal.transform.localPosition = Vector3.zero;
                currentMedal.transform.localScale = Vector3.one;
            }
        }

        DisplayScore(currentScore, normalScoreParent, normalScoreImages, normalScoreOffset);
        DisplayScore(bestScore, bestScoreParent, bestScoreImages, bestScoreOffset);
    }

    void DisplayScore(int score, Transform parent, List<Image> imageList, Vector2 offset)
    {
        foreach (Image img in imageList)
        {
            if (img != null)
                Destroy(img.gameObject);
        }
        imageList.Clear();

        string scoreStr = score.ToString();
        int digitCount = scoreStr.Length;

        float totalWidth = (digitCount - 1) * numberSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < digitCount; i++)
        {
            int digit = int.Parse(scoreStr[i].ToString());

            GameObject numberObj = new GameObject($"Number_{digit}");
            numberObj.transform.SetParent(parent);

            Image img = numberObj.AddComponent<Image>();
            img.sprite = numberSprites[digit];

            RectTransform rect = numberObj.GetComponent<RectTransform>();
            rect.sizeDelta = numberSize;
            rect.anchoredPosition = new Vector2(startX + (i * numberSpacing) + offset.x, offset.y);
            rect.localScale = Vector3.one;

            imageList.Add(img);
        }
    }
}
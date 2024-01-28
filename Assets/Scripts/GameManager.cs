using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Spawner[] spawners;
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private Image healthImage;
    [SerializeField] private Sprite[] kingSprites;
    [SerializeField] private SpriteRenderer kingRenderer;
    [SerializeField] private Transform kingContainer;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private int health;

    public int points;
    public int streak;
    public int kingSize = 1;

    private static GameManager _Instance;

    public static GameManager instance => Get();

    private void Start()
    {
        foreach (var spawner in spawners)
            StartCoroutine(spawner.Loop());

        health = healthSprites.Length - 1;
    }

    private void Update()
    {
        healthImage.sprite = healthSprites[health];
        if (health == 0)
            GameOver();
    }

    private void OnValidate()
    {
        if (health > healthSprites.Length)
            health = healthSprites.Length;
    }

    public static GameManager Get()
    {
        if (_Instance == null)
            _Instance = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        return _Instance;
    }

    public void AddPoints(int amount)
    {
        points += amount;
        pointsText.text = points.ToString();
        pointsText.GetComponent<Animator>().Play("PointsAddPop");
    }

    public void Streak(bool ok)
    {
        if (streak > 0)
            streak = ok ? streak + 1 : 0;
        else
            streak += ok ? 1 : -1;

        if (Math.Abs(streak) >= 10)
        {
            kingSize += streak / 10;
            streak = 0;
        }

        if (kingSize >= kingSprites.Length)
            Win();
        else if (kingSize < 0)
            GameOver();
        else
        {
            var delta = new Vector3(0, (kingSize - 1) * 0.03f);
            kingRenderer.sprite = kingSprites[kingSize];

            if (kingContainer != null)
            {
                kingContainer.localScale = Vector3.one + delta;
                kingContainer.localPosition = delta * 6;
            }
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("MenuScene");
    }

    private void Win()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("MenuScene");
    }
}
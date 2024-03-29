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
    [SerializeField] private TMP_Text endPointsText;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject winCanvas;


    public int health;
    public int points;
    public int streak;
    public int kingSize = 1;

    private static GameManager _Instance;

    public static GameManager instance =>
        _Instance ??= GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    private void Start()
    {
        foreach (var spawner in spawners)
            StartCoroutine(spawner.Loop());

        _Instance = null;
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

    public void AddPoints(int amount)
    {
        points += amount;
        pointsText.text = points.ToString();
        pointsText.GetComponent<Animator>().Play("PointsAddPop");
    }

    public void AddHealth(int amount)
    {
        health += amount;
        healthImage.GetComponent<Animator>().Play("PointsAddPop");
    }

    public void Streak(bool ok)
    {
        if (streak > 0)
            streak = ok ? streak + 1 : 0;
        else
            streak += ok ? 1 : -1;

        if (Math.Abs(streak) >= 10)
        {
            AudioManager.instance.Play(streak > 0 ? "hahaha" : "yegh");

            kingSize += streak / 10;
            streak = 0;
        }

        if (kingSize >= kingSprites.Length)
            Win();
        else if (kingSize < 0)
            GameOver();
        else if (kingContainer != null)
        {
            var delta = new Vector3(0, (kingSize - 1) * 0.03f);

            kingRenderer.sprite = kingSprites[kingSize];
            kingContainer.localScale = Vector3.one + delta;
            kingContainer.localPosition = delta * 6;
        }
    }

    private void GameOver()
    {
        AudioManager.instance.Play("gameover");
        gameOverCanvas.SetActive(true);
        endPointsText.text = points.ToString();
        foreach (var spawner in spawners)
            spawner.Stop();
    }

    private void Win()
    {
        AudioManager.instance.Play("hahahands");
        winCanvas.SetActive(true);
        foreach (var spawner in spawners)
            spawner.Stop();
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
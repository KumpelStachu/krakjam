using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Spawner[] spawners;
    [SerializeField] private Sprite[] hpSprites;
    [SerializeField] private Image hpImage;
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

        health = hpSprites.Length - 1;
    }

    private void Update()
    {
        hpImage.sprite = hpSprites[health];
    }

    private void OnValidate()
    {
        if (health > hpSprites.Length)
            health = hpSprites.Length;
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
            Debug.Log("Win");
        else if (kingSize < 0)
            Debug.Log("GameOver");
        else
        {
            var delta = new Vector3(0, (kingSize - 1) * 0.03f);
            kingContainer.localScale = Vector3.one + delta;
            kingContainer.localPosition = delta * 6;
            kingRenderer.sprite = kingSprites[kingSize];
        }
    }
}
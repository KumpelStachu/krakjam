using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Spawner[] spawners;
    [SerializeField] private Sprite[] hpSprites;
    [SerializeField] private Image hpImage;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private int health;

    [SerializeField] private int points;

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
}
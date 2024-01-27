using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Spawner weaponSpawner;
    [SerializeField] private Spawner powerUpSpawner;
    [SerializeField] private Spawner touchPointSpawner;
    [SerializeField] private Sprite[] hpSprites;
    [SerializeField] private Image hpImage;
    [SerializeField] private int health;

    private void Start()
    {
        StartCoroutine(weaponSpawner.Loop());
        StartCoroutine(powerUpSpawner.Loop());
        StartCoroutine(touchPointSpawner.Loop());

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
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Spawner weaponSpawner;
    [SerializeField] private Spawner powerUpSpawner;
    [SerializeField] private Spawner touchPointSpawner;

    private void Start()
    {
        StartCoroutine(weaponSpawner.Loop());
        StartCoroutine(powerUpSpawner.Loop());
        StartCoroutine(touchPointSpawner.Loop());
    }
}
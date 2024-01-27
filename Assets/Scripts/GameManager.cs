using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Spawner weaponSpawner;
    [SerializeField] private Spawner powerUpSpawner;
    [SerializeField] private Spawner touchPointSpawner;

    private void Start()
    {
        StartCoroutine(nameof(weaponSpawner.Loop));
        StartCoroutine(nameof(powerUpSpawner.Loop));
        StartCoroutine(nameof(touchPointSpawner.Loop));
    }
}
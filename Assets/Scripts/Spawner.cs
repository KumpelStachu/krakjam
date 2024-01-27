using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class Spawner
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Vector2 spawnRadius;
    [SerializeField] private float initialDelay = 5;
    [SerializeField] private float delay = 10;
    [SerializeField] private float delayMultiplier = 0.99f;

    private bool _stopped;

    private static Vector3 RandomVector(Vector2 radius) =>
        new((Random.value - 0.5f) * radius.x, (Random.value - 0.5f) * radius.y);

    private static T RandomItem<T>(IReadOnlyList<T> list) => list[Random.Range(0, list.Count)];

    public void Stop() => _stopped = true;

    public IEnumerator Loop()
    {
        if (prefabs.Length == 0 || spawnPoints.Length == 0) yield break;

        yield return new WaitForSeconds(initialDelay);

        while (!_stopped)
        {
            var spawnPoint = RandomItem(spawnPoints);
            var weapon = Object.Instantiate(RandomItem(prefabs), spawnPoint);
            weapon.transform.position += RandomVector(spawnRadius);

            yield return new WaitForSeconds(delay);
            delay *= delayMultiplier;
        }
    }
}
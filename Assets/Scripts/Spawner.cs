using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = Unity.Mathematics.Random;

[Serializable]
public class Spawner
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float initialDelay = 5;
    [SerializeField] private float delay = 10;
    [SerializeField] private float delayMultiplier = 0.99f;

    private bool _stopped;

    private static T Random<T>(IReadOnlyList<T> list) => list[new Random().NextInt(list.Count)];

    public void Stop() => _stopped = true;

    public IEnumerable Loop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (!_stopped)
        {
            var spawnPoint = Random(spawnPoints);
            Object.Instantiate(Random(prefabs), spawnPoint);

            yield return new WaitForSeconds(delay);
            delay *= delayMultiplier;
        }
    }
}
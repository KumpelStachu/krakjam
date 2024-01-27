using System.Collections;
using UnityEngine;

public class PleaseKillMe : MonoBehaviour
{
    [SerializeField] [Min(0)] private float timeToLive;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
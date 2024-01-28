using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    private void Start()
    {
        transform.Rotate(0, 0, Random.value * 360);
    }
}
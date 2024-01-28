using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GetComponent<Collider2D>().enabled = false;
        GameManager.instance.AddHealth(-1);
    }
}
using System.Collections;
using UnityEngine;

public class CircleScript : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 3;

    private bool _clicked;

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (_clicked || !other.gameObject.CompareTag("Player")) yield break;

        _clicked = true;

        var animator = GetComponent<Animator>();
        animator.speed *= speedMultiplier;

        // TODO: GameManager.instance.addPoints();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
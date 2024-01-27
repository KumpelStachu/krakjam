using System.Collections;
using UnityEngine;

public class CircleScript : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 3;
    [SerializeField] private float animationDuration = 2;

    private bool _clicked;

    private IEnumerator OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other);
        if (_clicked || !other.gameObject.CompareTag("Player")) yield break;

        _clicked = true;
        // TODO: GameManager.instance.addPoints();

        var animator = GetComponent<Animator>();
        animator.speed *= speedMultiplier;

        var remaining = animationDuration - animator.playbackTime;
        yield return new WaitForSeconds(remaining);

        Destroy(gameObject);
    }
}
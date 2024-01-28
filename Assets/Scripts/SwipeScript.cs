using System.Collections;
using UnityEngine;

public class SwipeScript : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 3;
    [SerializeField] private Collider2D enterCollider;
    [SerializeField] private Collider2D exitCollider;

    private bool _swiped, _swiping;

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (_swiped || !other.gameObject.CompareTag("Player")) yield break;

        if (enterCollider.IsTouching(other))
        {
            _swiping = true;
        }

        if (exitCollider.IsTouching(other))
        {
            _swiped = true;

            if (_swiping)
            {
                GameManager.instance.AddPoints(20);
                GameManager.instance.Streak(true);
            }

            var animator = GetComponent<Animator>();
            animator.speed *= speedMultiplier;
        }
    }

    private void OnDestroy()
    {
        if (!(_swiped && _swiping))
            GameManager.instance.Streak(false);
    }

    public void KillMe() => StartCoroutine(nameof(Die));

    public IEnumerator Die()
    {
        if (!_swiped) yield break;

        var trans = transform;
        var child = trans.GetChild(0);
        var magic = GameObject.Instantiate(child.gameObject, transform.position, Quaternion.identity);
        var particle = magic.GetComponent<ParticleSystem>();
        magic.GetComponent<PleaseKillMe>().enabled = true;
        particle.Play();

        Destroy(gameObject);
    }
}
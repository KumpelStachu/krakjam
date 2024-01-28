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
        GameManager.instance.AddPoints(10);
        GameManager.instance.Streak(true);
        var animator = GetComponent<Animator>();
        animator.speed *= speedMultiplier;
    }

    private void OnDestroy()
    {
        if (!_clicked)
            GameManager.instance.Streak(false);
    }

    public void KillMe() => StartCoroutine(nameof(Die));

    public IEnumerator Die()
    {
        if (!_clicked) yield break;

        var trans = transform;
        var child = trans.GetChild(0);
        var magic = GameObject.Instantiate(child.gameObject, transform.position, Quaternion.identity);
        var particle = magic.GetComponent<ParticleSystem>();
        magic.GetComponent<PleaseKillMe>().enabled = true;
        particle.Play();

        Destroy(gameObject);
    }
}
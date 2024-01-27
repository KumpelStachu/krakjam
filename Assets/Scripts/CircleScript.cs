using System;
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
        yield break;
        if (!_clicked) yield break;

        var child = transform.GetChild(0);
        var particle = child.GetComponent<ParticleSystem>();
        // particle.playOnAwake = true;
        // child.gameObject.SetActive(true);
        particle.time = 0;
        particle.Play();

        var trans = transform;
        trans.DetachChildren();
        trans.position = new Vector3(69, 69);

        yield return new WaitForSecondsRealtime(1);

        Destroy(child.gameObject);
        Destroy(gameObject);
    }
}
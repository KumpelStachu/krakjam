using UnityEngine;
using UnityEngine.Events;

public class HandButton : MonoBehaviour
{
    [SerializeField] private float handTime;
    [SerializeField] private UnityEvent onClick;

    private bool _clicked;
    private float _timer;

    private static bool IsPlayer(Collider2D c) => c.gameObject.CompareTag("Player");


    private void OnTriggerStay2D(Collider2D other)
    {
        if (_clicked || !IsPlayer(other)) return;

        _timer += Time.deltaTime;

        if (_timer < handTime) return;

        _clicked = true;
        onClick.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;

        _timer = 0;
        _clicked = false;
    }
}
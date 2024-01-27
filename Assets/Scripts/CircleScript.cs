using System.Collections;
using UnityEngine;

public class CircleScript : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 3;
    [SerializeField] private GameManager gameManagerScript;

    private bool _clicked;

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (_clicked || !other.gameObject.CompareTag("Player")) yield break;

        _clicked = true;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AddPoints(10);
        var animator = GetComponent<Animator>();
        animator.speed *= speedMultiplier;
        

        // TODO: GameManager.instance.addPoints();
    }

    public void Die()
    {
        
        this.gameObject.transform.parent = null;
        this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBlink : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private SpriteRenderer kingBlinkRenderererer;
    [SerializeField] private Sprite[] kingEyeSprites;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;


        kingBlinkRenderererer.sprite = kingEyeSprites[GameManager.instance.kingSize];
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;


        kingBlinkRenderererer.sprite = null;
    }
}

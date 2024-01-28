using UnityEngine;

public class KingBlink : MonoBehaviour
{
    [SerializeField] private SpriteRenderer kingBlinkRenderererer;
    [SerializeField] private Sprite[] kingEyeSprites;

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
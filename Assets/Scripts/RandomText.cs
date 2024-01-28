using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private string[] texts;

    void Start() => Randomize();

    private void OnEnable() => Randomize();

    private void Randomize()
        => text.text = texts[Random.Range(0, texts.Length)];
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject levelsPanel;

    private void Start()
    {
        levelsPanel.SetActive(false);
    }

    public void ToggleLevelSelector() => levelsPanel.SetActive(!levelsPanel.activeSelf);

    public void LoadLevel(string level) => SceneManager.LoadScene($"Level{level}");
}
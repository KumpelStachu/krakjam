using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Animator doorAnim;

    private void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        Application.Quit();     
    }

    public void StartExit()
    {
        doorAnim.Play("ExitIcon"); //tu zmienic!
    }
}
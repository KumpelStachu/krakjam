using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Animator doorAnim;
    [SerializeField] private Animator transitionAnim;
    [SerializeField] private GameObject playerFeather;

    private void Start()
    {
        Time.timeScale = 1;
        playerFeather.GetComponent<SpriteRenderer>().enabled = false;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) yield break;

        playerFeather.GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = false;

        AudioManager.instance.Play("hahahands");
        transitionAnim.Play("TransitionAnim");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartExit()
    {
        Application.Quit();
        doorAnim.Play("ExitIcon");
    }
}
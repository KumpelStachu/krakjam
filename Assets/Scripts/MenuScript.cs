using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
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

        // TODO: transitionAnimator.Play();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartExit()
    {
        doorAnimator.Play("ExitIcon");
    }
}
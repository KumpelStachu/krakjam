using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {

        GetComponent<Animator>().Play("KingMove");

        // TODO: GameManager.instance.addPoints();
    }
    public void OnTriggerExit2D(Collider2D other)
    {

        GetComponent<Animator>().Play("KingIdle");

        // TODO: GameManager.instance.addPoints();
    }
}

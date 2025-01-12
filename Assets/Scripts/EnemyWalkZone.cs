using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkZone : MonoBehaviour
{
    private Dino dino;
    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        dino = transform.parent.GetComponentInChildren<Dino>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            dino.gameObject.GetComponent<Animator>().SetBool("is_running", true);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {

            dino.gameObject.GetComponent<Dino>().RunToPlayer(col);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (dino.gameObject)
            {
                dino.gameObject.GetComponent<Animator>().SetBool("is_running", false);
            }

        }
    }


}

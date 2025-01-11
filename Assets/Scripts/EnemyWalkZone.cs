using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkZone : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject.Find("DinoSprite").gameObject.GetComponent<Animator>().SetBool("is_running", true);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject.Find("DinoSprite").gameObject.GetComponent<Dino>().RunToPlayer(col);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (GameObject.Find("DinoSprite").gameObject)
            {
                GameObject.Find("DinoSprite").gameObject.GetComponent<Animator>().SetBool("is_running", false);
            }

        }
    }


}

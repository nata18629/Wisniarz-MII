using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : EnemyController
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (transform.position.y +1.0f<= col.gameObject.transform.position.y)
            {
                
                StartCoroutine(KillParentOnAnimationEnd());
            }
        }
    }

    void Update()
    { }

    public void RunToPlayer(Collider2D col)
    {
        if (transform.position.x >= col.gameObject.transform.position.x) //left
        {
            if (is_moving_right)
            {
                is_moving_right = false;
                Flip();

            }
            MoveLeft();
        }
        else //right
        {
            if (!is_moving_right)
            {
                is_moving_right = true;
                Flip();

            }
            MoveRight();
        }
    }
    protected IEnumerator KillParentOnAnimationEnd()
    {
        yield return new WaitForSeconds(wait_to_die);
        transform.parent.gameObject.SetActive(false);
    }
}

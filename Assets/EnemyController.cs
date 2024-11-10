using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private bool is_facing_right = false;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [Range(0.1f, 20.0f)][SerializeField] private float moveRange = 1f;
    private float start_posx;
    private bool is_moving_right = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (is_moving_right)
        {
            if (this.transform.position.x <= start_posx + moveRange)
            {
                MoveRight();
            }
            else
            {
                is_moving_right = false;
                Flip();
            }
        }
        else
        {
            if (this.transform.position.x >= start_posx - moveRange)
            {
                MoveLeft();
            }
            else
            {
                is_moving_right = true;
                Flip();
            }
        }
    }
    void Awake()
    {
        //rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        start_posx=this.transform.position.x;
    }
    void Flip()
    {
        is_facing_right = !is_facing_right;
        Vector3 the_scale = transform.localScale;
        the_scale.x *= -1;
        transform.localScale = the_scale;

    }
    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (transform.position.y <= col.gameObject.transform.position.y)
            {
                animator.SetBool("is_dead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }
    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}

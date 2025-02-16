using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Movement parameteres :3")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [Range(0.01f, 20.0f)][SerializeField] private float jumpForce = 6.0f;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool is_facing_right=true;
    private bool is_walking = false;
    private bool is_climbing = false;
    private float vertical;
    private Vector2 start_pos;

    // sound
    [SerializeField] private AudioClip bonusSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip enemykillSound;
    [SerializeField] private AudioClip keySound;
    private AudioSource source;

    public LayerMask groundLayer;
    const float rayLength = 1.2f;
    const int KEYS_NUM = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.GAME && !animator.GetBool("is_hurt"))
        {
            is_walking = false;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                is_walking = true;
                if (!is_facing_right)
                {
                    Flip();
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                is_walking = true;
                if (is_facing_right)
                {
                    Flip();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            //Debug.DrawRay(transform.position, rayLength*Vector3.down,Color.white,1,false);
        }
            animator.SetBool("is_grounded", IsGrounded());
            animator.SetBool("is_walking", is_walking);
            animator.SetBool("is_climbing", is_climbing);
            vertical = Input.GetAxis("Vertical");
        
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        start_pos = transform.position;
        source = GetComponent<AudioSource>();
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    void Jump()
    {
        if(IsGrounded())
        {
            source.PlayOneShot(jumpSound, AudioListener.volume);
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Kill(bool play_animation)
    {
        animator.SetBool("is_hurt", play_animation);
        source.PlayOneShot(hurtSound, AudioListener.volume);
        StartCoroutine(Wait(play_animation));
        GameManager.instance.LoseLife();
    }

    IEnumerator Wait(bool play_animation)
    {
        if (play_animation)
        {
            yield return new WaitForSeconds(0.8f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        
        animator.SetBool("is_hurt", false);
        transform.position = start_pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "FallLevel")
        {
            Kill(false);
        }
        if (col.CompareTag("Bonus"))
        {
            source.PlayOneShot(bonusSound, AudioListener.volume);
            GameManager.instance.AddPoints(100);
            col.gameObject.SetActive(false);
        }
        if (col.CompareTag("Ladder"))
        {
            is_climbing = true;
        }
        if (col.CompareTag("Enemy") && !animator.GetBool("is_hurt"))
        {
            
            if (transform.position.y >= col.gameObject.transform.position.y+1.0f)
            {
                if (!col.gameObject.GetComponent<Animator>().GetBool("is_dead"))
                {
                    source.PlayOneShot(enemykillSound, AudioListener.volume);
                    GameManager.instance.AddEnemyKilled();
                    rigidBody.velocity = Vector2.zero;
                    rigidBody.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
                    Jump();
                }
               
            }
            else
            {
                Kill(true);
            }
        }
        if (col.CompareTag("Key"))
        {
            source.PlayOneShot(keySound, AudioListener.volume);
            GameManager.instance.AddKeys(col.gameObject.name);
            col.gameObject.SetActive(false);

            //if (keys_found == KEYS_NUM)
            //{

            //}
        }
        if (col.CompareTag("Heart"))
        {
            GameManager.instance.AddLife();
            col.gameObject.SetActive(false);
        }
        if (col.CompareTag("END"))
        {
            GameManager.instance.LevelCompleted();
        }
        if (col.CompareTag("MovingPlatform"))
        {
            transform.SetParent(col.transform);
        }


    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            is_climbing = false;
        }
        if (col.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }

    }

    void FixedUpdate()
    {
        if (is_climbing)
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * moveSpeed);
        }
        else
        {
            rigidBody.gravityScale = 1;
        }
    }

    void Flip()
    {
        is_facing_right = !is_facing_right;
        Vector3 the_scale = transform.localScale;
        the_scale.x *= -1;
        transform.localScale = the_scale;

    }
}

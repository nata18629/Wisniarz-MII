using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
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
            }
        }
    }
    void Awake()
    {
        start_posx = this.transform.position.x;
    }
    void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
}

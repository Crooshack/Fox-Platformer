using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    private float startPositionX;
    private bool isMovingRight = false;
    [Range(0.01f, 20.0f)][SerializeField] public float moveRange = 1.0f;


    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
            if (isMovingRight)
        {
            if (this.transform.position.x <= startPositionX + moveRange)
                MoveRight();
            else
            {
                Flip();
                MoveLeft();
            }
        }
        else
        {
            if (this.transform.position.x >= startPositionX - moveRange)
                MoveLeft();
            else
            {
                Flip();
                MoveRight();
            }
        }

    }
    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    private void Awake()
    {
        startPositionX = this.transform.position.x;
        //animator = GetComponent<Animator>();
    }

    private void Flip()
    {
        isMovingRight = !isMovingRight;
    }
}
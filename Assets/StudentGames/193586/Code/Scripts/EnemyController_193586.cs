using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f;
    private Animator animator;
    private bool isFacingRight = false;
    private float startPositionX;
    public float moveRange = 1.0f;
    private bool isMovingRight = false;
    private float bounceForce = 7.0f;


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


    private void Flip()
    {
        isFacingRight = !isFacingRight;
        isMovingRight = !isMovingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Awake()
    {
        startPositionX = this.transform.position.x;
        animator = GetComponent<Animator>();
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.45f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.transform.position.y > transform.position.y)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
                Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
                playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
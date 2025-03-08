using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    private Vector2 checkpointPosition;
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
    [Space(10)]
    [Range(0.01f, 10.0f)][SerializeField] private float jumpForce = 6.0f;
    [Space(10)]
    [SerializeField] private AudioClip bSound;
    [SerializeField] private AudioClip defeatSound;
    [SerializeField] private AudioClip killSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private AudioClip deathSound;
    private Rigidbody2D rigidbody;
    public LayerMask groundLayer;
    const float rayLength = 1.3f;
    private Animator animator;
    private bool isWalking = false;
    private bool isFacingRight = true;
    private const int keysNumber = 3;
    private AudioSource source;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            isWalking = false;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                isWalking = true;
                if (!isFacingRight)
                {
                    Flip();
                }
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                isWalking = true;
                if (isFacingRight)
                {
                    Flip();
                }
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                animator.SetBool("isGrounded", IsGrounded());
            }
            // END
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isGrounded", IsGrounded());
            Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
        }
    }

    private void Awake()
    {
        checkpointPosition = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
 
    }

    bool IsGrounded()
    {
        Vector2 left = this.transform.position;
        left.x -= this.GetComponent<Collider2D>().bounds.size.x / 2;
        Vector2 right = this.transform.position;
        right.x += this.GetComponent<Collider2D>().bounds.size.x / 2;
        bool result = Physics2D.Raycast(left, Vector2.down, rayLength, groundLayer.value) || Physics2D.Raycast(right, Vector2.down, rayLength, groundLayer.value);

        return result;
        //return Physics2D.BoxCast(this.transform.position, this.GetComponent<Collider2D>().bounds.size, 0f, Vector2.down, rayLength, groundLayer.value);
    }

    void Jump()
    {
        if (IsGrounded())
        {
            source.PlayOneShot(jumpSound, AudioListener.volume / 2);
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            source.PlayOneShot(bSound, AudioListener.volume);
            GameManager.instance.AddPoints(1);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Gem"))
        {
            source.PlayOneShot(bSound, AudioListener.volume);
            GameManager.instance.AddPoints(10);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Finish"))
        {
            if (GameManager.instance.keysFound == keysNumber)
            {
                source.PlayOneShot(victorySound, AudioListener.volume);
                GameManager.instance.AddPoints(GameManager.instance.lives * 100);
                GameManager.instance.LevelCompleted();
            }
            else
            {
                Debug.Log("Not enough keys, go find some more foxy");
                Debug.Log("Current number of keys: " + GameManager.instance.keysFound);

                GameManager.instance.infoText.text = "Not enough keys!";
                GameManager.instance.ShowInfo();
            }

        }
        else if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                source.PlayOneShot(killSound, AudioListener.volume);
                GameManager.instance.AddKills();
                Debug.Log("Killed an enemy");
            }
            else
            {
                Death();
            }

        }
        else if (other.CompareTag("Trap"))
        {
            float trap_size_x = this.GetComponent<Collider2D>().bounds.size.x;
            float player_size_x = other.GetComponent<Collider2D>().bounds.size.x;
            if ((other.gameObject.transform.position.x > transform.position.x - trap_size_x / 2 && other.gameObject.transform.position.x < transform.position.x + trap_size_x / 2) ||
                (other.gameObject.transform.position.x + player_size_x / 2 > transform.position.x - trap_size_x / 2 + trap_size_x / 10 && other.gameObject.transform.position.x - player_size_x / 2 < transform.position.x - trap_size_x / 2 + trap_size_x / 10) ||
                (other.gameObject.transform.position.x - player_size_x / 2 < transform.position.x + trap_size_x / 2 - trap_size_x / 10 && other.gameObject.transform.position.x + player_size_x / 2 > transform.position.x + trap_size_x / 2 - trap_size_x / 10))
            {
                Death();
            }
        }
        else if (other.CompareTag("Yellow key"))
        {
            source.PlayOneShot(bSound, AudioListener.volume);
            GameManager.instance.AddKey("Yellow key");
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Green key"))
        {
            source.PlayOneShot(bSound, AudioListener.volume);
            GameManager.instance.AddKey("Green key");
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Red key"))
        {
            source.PlayOneShot(bSound, AudioListener.volume);
            GameManager.instance.AddKey("Red key");
            other.gameObject.SetActive(false);
        }

        else if (other.CompareTag("Heart"))
        {
            source.PlayOneShot(bSound, AudioListener.volume);
            GameManager.instance.AddLife();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("FallLevel"))
        {        
            Death();
        }
        else if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
        else if (other.CompareTag("Checkpoint"))
        {
            source.PlayOneShot(checkpointSound, AudioListener.volume);
            GameManager.instance.infoText.text = "Checkpoint reached";
            GameManager.instance.ShowInfo();
            checkpointPosition = transform.position;
            other.GetComponent<Collider2D>().enabled = false;
            Debug.Log("Checkpoint");
        }
        else if (other.CompareTag("Pop-up frog"))
        {
            GameManager.instance.infoText.text = "Try jumping on the frog";
            GameManager.instance.ShowInfo();
            other.GetComponent<Collider2D>().enabled = false;
        }
        else if (other.CompareTag("Pop-up eagle"))
        {
            GameManager.instance.infoText.text = "Eagles die when you jump on top of them";
            GameManager.instance.ShowInfo();
            other.GetComponent<Collider2D>().enabled = false;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BouncePlatform"))
        {
            source.PlayOneShot(bounceSound, AudioListener.volume);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

    private void Death()
    {
        if (GameManager.instance.lives == 0)
        {
            source.PlayOneShot(defeatSound, AudioListener.volume * 2);
            rigidbody.velocity = new Vector2(0, 0);
            GameManager.instance.GameOver();
        }
        else
        {
            source.PlayOneShot(deathSound, AudioListener.volume);
            transform.position = checkpointPosition;
            GameManager.instance.DeleteLife();
            rigidbody.velocity = new Vector2(0, 0);
        }
    }

}

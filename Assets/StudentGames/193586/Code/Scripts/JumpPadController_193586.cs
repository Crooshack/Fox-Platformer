using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    [Range(1.0f, 20.0f)][SerializeField] private float bounceForce = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (other.gameObject.transform.position.y > transform.position.y)
            {
                float frog_size_x = this.GetComponent<Collider2D>().bounds.size.x;
                float player_size_x = other.GetComponent<Collider2D>().bounds.size.x;
                if ((other.gameObject.transform.position.x > transform.position.x - frog_size_x / 2 && other.gameObject.transform.position.x < transform.position.x + frog_size_x/2) || 
                    (other.gameObject.transform.position.x + player_size_x/2 > transform.position.x - frog_size_x / 2 + frog_size_x/10 && other.gameObject.transform.position.x - player_size_x / 2 < transform.position.x - frog_size_x / 2 + frog_size_x / 10) ||
                    (other.gameObject.transform.position.x - player_size_x / 2 < transform.position.x + frog_size_x / 2 - frog_size_x / 10 && other.gameObject.transform.position.x + player_size_x / 2 > transform.position.x + frog_size_x / 2 - frog_size_x / 10)) { 
                        Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                        playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
                        playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                }
                
            }
        }
    }
}
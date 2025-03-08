using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlatformUpController : MonoBehaviour
{
[Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 0.1f;
private float startPositionY;
private bool isMovingUp = false;
[Range(0.01f, 20.0f)][SerializeField] public float moveRange = 1.0f;

// Start is called before the first frame update
void Start()
{



}

// Update is called once per frame
void Update()
{
            if (isMovingUp)
            {
                if (this.transform.position.y <= startPositionY + moveRange)
                    MoveUp();
                else
                {
                    Flip();
                    MoveDown();
                }
            }
            else
            {
                if (this.transform.position.y >= startPositionY - moveRange)
                    MoveDown();
                else
                {
                    Flip();
                    MoveUp();
                }
            }
}
private void MoveUp()
{
    transform.Translate(0.0f, moveSpeed * Time.deltaTime, 0.0f, Space.World);
}

private void MoveDown()
{
    transform.Translate(0.0f, -moveSpeed * Time.deltaTime, 0.0f, Space.World);
}
private void Awake()
{
    startPositionY = this.transform.position.y;
    //animator = GetComponent<Animator>();
}

private void Flip()
{
    isMovingUp = !isMovingUp;
}
}
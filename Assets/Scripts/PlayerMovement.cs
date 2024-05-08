using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float ClimbForce;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask staircase;
    [SerializeField] private Text text;
    [SerializeField] private Text healthPlayer;
    [SerializeField] private float hurtForce = 5f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private int health = 4;

    private Rigidbody2D rib;
    private Animator anim;
    private Collider2D coll;

    bool vertical = false;
    float horizontal = 0;
    bool jump = false;
    

    private enum State { Idle , Run , Jump , Fall , Hurt , Climb ,Crouch};
    private State state = State.Idle;

    private void Start()
    {
        rib = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(state != State.Hurt)
            MovementState();
        AnimationState();
        anim.SetInteger("state", (int)state);
    }

    public void SetHorizontal(float  horizontal)
    {
        this.horizontal = horizontal;
    }
    public void SetJump(bool jump)
    {
        this.jump = jump;
    }
    public void SetVertical(bool vertical)
    {
        this.vertical = vertical;
    }


    
    private void MovementState()
    {
        float horizontalInput = horizontal;
        bool  VerticalInput = vertical;
        bool canJump = jump;
        Debug.Log(horizontalInput);
        if (!coll.IsTouchingLayers(staircase))
        {
            rib.velocity = new Vector2(speed * horizontalInput, rib.velocity.y);
            if (horizontalInput < 0f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (horizontalInput > 0f)
            {
                transform.localScale = Vector3.one;
            }

            if (canJump && coll.IsTouchingLayers(ground))
            {
                Jump();
            }

            if (VerticalInput  && coll.IsTouchingLayers(ground))
            {
                rib.velocity = Vector2.zero;
                state = State.Crouch;
            }
        }
        if (VerticalInput && coll.IsTouchingLayers(staircase))
        {
            state = State.Climb;
            rib.velocity = new Vector2(0, ClimbForce);
        }
    }
    public void AnimationState()
    {
        if(state == State.Jump)
        {
            if(rib.velocity.y < .1f)
            {
                state = State.Fall;
            }
        }
        else if(state == State.Fall)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.Idle;
            }
        }
        else if(state == State.Hurt)
        {
            if (Mathf.Abs(rib.velocity.x) < .1f)
            {
                state = State.Idle;
            }
        }
        else if (Mathf.Abs(rib.velocity.x) > 0f)
        {
            state = State.Run;
        }
        else
        {
            if (!coll.IsTouchingLayers(staircase) && !Input.GetButton("Vertical"))
            {
                state = State.Idle;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "cherry")
        {
            cherries += 1;
            Destroy(collision.gameObject);
            text.text = cherries.ToString();
        }
        else if (collision.gameObject.tag == "gem")
        {
            Destroy(collision.gameObject);
            jumpForce = 7.5f;
            GetComponent<SpriteRenderer>().color = Color.blue;
            StartCoroutine(Change());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(state == State.Fall)
            {
                Jump();
                enemy.JumpOn();
            }
            else
            {
                state = State.Hurt;
                health -= 1;
                healthPlayer.text = health.ToString();
                if(health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    rib.velocity = new Vector2(-hurtForce , rib.velocity.y);
                }
                else
                {
                    rib.velocity = new Vector2(hurtForce, rib.velocity.y);
                }
            }
        }
    }

    private void Jump()
    {
        rib.velocity = new Vector2(rib.velocity.x, jumpForce);
        state = State.Jump;
    }
    

    private IEnumerator Change()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 6f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}

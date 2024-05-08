using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoxMovement : Enemy
{
    [SerializeField] private float LeftZone;
    [SerializeField] private float RightZone;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask ground;

    private bool left = true;
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        StateAnimation();
    }

    private void Move()
    {
        if (left)
        {
            if (transform.position.x <= LeftZone)
            {
                if (coll.IsTouchingLayers(ground))
                {
                    rib.velocity = new Vector2(speed, jumpForce);
                    anim.SetBool("jump", true);
                }
                transform.localScale = new Vector3(-1, 1, 1);
                left = false;
            }
            else
            {
                if (coll.IsTouchingLayers(ground))
                {
                    rib.velocity = new Vector2(-speed, jumpForce);
                    anim.SetBool("jump", true);
                }
            }
        }
        else
        {
            if (transform.position.x >= RightZone)
            {
                if (coll.IsTouchingLayers(ground))
                {
                    rib.velocity = new Vector2(-speed, jumpForce);
                    anim.SetBool("jump", true);
                }
                transform.localScale = new Vector3(1, 1, 1);
                left = true;
            }
            else
                if (coll.IsTouchingLayers(ground))
            {
                rib.velocity = new Vector2(speed, jumpForce);
                anim.SetBool("jump", true);
            }
        }
    }

    private void StateAnimation()
    {
        if (anim.GetBool("jump"))
        {
            if(rib.velocity.y < .1)
            {
                anim.SetBool("fall", true);
                anim.SetBool("jump", false);
            }
        }
        if(coll.IsTouchingLayers(ground)&& anim.GetBool("fall"))
        {
            anim.SetBool("fall", false);
        }
    }
   
}

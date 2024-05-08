using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumMovement : Enemy
{
    [SerializeField] private float speed;
    [SerializeField] private float LeftZone;
    [SerializeField] private float RightZone;

    private bool left = true;
    protected override void Start()
    {
        base.Start();
    }

    private void Movement()
    {
        if (left)
        {
            if (transform.position.x <= LeftZone)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                left = false;
            }
            rib.velocity = new Vector2(-speed, rib.velocity.y);
        }
        else
        {
            if (transform.position.x >= RightZone)
            {
                transform.localScale = new Vector3(1, 1, 1);
                left = true;
            }
            rib.velocity = new Vector2(speed, rib.velocity.y);
        }
    }
}

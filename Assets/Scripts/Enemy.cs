using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rib;
    protected Animator anim;
    protected Collider2D coll;
    protected virtual void Start()
    {
        rib = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();   
        coll = GetComponent<Collider2D>();
    }

    public void JumpOn()
    {
        anim.SetTrigger("death");
        rib.velocity = Vector2.zero;
        rib.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}

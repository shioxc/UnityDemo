using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck pc;
    private PlayerController pcr;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PhysicsCheck>();
        pcr = GetComponent<PlayerController>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        anim.SetFloat("velocityX",math.abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround",pc.isGround);
        anim.SetBool("isDead", pcr.isDead);
        anim.SetBool("isAttack", pcr.isAttack);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void Playattack()
    {
        anim.SetTrigger("attack");
    }
}

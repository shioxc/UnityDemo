using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public bool manual;//触墙判定自动贴近胶囊体

    private CapsuleCollider2D coll;

    public Vector2 buttomOffset;

    public Vector2 leftOffset;
    public Vector2 rightOffset;

    public float checkRaduis;

    public LayerMask groundLayer;

    public bool isGround;

    public bool touchLeftWall;
    public bool touchRightWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if (manual)
        {
            rightOffset = new Vector2(coll.bounds.size.x / 2 + coll.offset.x, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x,rightOffset.y);
        }
    }
    void Update()
    {
        check();
    }

    public void check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + buttomOffset, checkRaduis, groundLayer);

        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position +  leftOffset, checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + buttomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}

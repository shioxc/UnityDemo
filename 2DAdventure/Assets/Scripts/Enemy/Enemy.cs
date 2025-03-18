using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{

    protected Rigidbody2D rb;

    [HideInInspector] public PhysicsCheck pc;

    CapsuleCollider2D coll;

    [HideInInspector] public Animator anim;

    public float normalSpeed;

    public float chaseSpeed;//×·»÷ËÙ¶È

    public float currentSpeed;

    public Vector3 faceDir;

    private Vector2 _velocity;

    public Vector2 centerOffset;

    public Vector2 checkSize;

    public float checkDistance;

    public LayerMask attackLayer;

    public float waitTime;

    public float waitTimeCounter;

    public bool wait;

    public float lostTime;

    public float lostTimeCounter;

    public Transform attacker;

    public bool isHurt;

    public bool isDead;

    public float hurtForce;

    protected BaseState currentState;

    protected BaseState patrolState;

    protected BaseState chaseState;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        waitTimeCounter = waitTime;
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        _velocity = new Vector2();
        currentSpeed = normalSpeed;

        if (faceDir.x < 0)
            pc.buttomOffset = new Vector2(pc.leftOffset.x, pc.buttomOffset.y);
        else pc.buttomOffset = new Vector2(pc.rightOffset.x, pc.buttomOffset.y);
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        faceDir.x = -transform.localScale.x;
       
        currentState.LogicUpdate(); 
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if (!isHurt & !isDead)
        {
            Move();
        }
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    public virtual void Move()
    {
        _velocity.x = currentSpeed * faceDir.x * Time.deltaTime;
        _velocity.y = rb.velocity.y;
        rb.velocity = _velocity;
    }

    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
            }
        }
        if(!FoundPlayer() & lostTimeCounter>0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else if(FoundPlayer()) 
        {
            lostTimeCounter = lostTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset, 0.2f);
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position+(Vector3)centerOffset,checkSize,0,faceDir,checkDistance,attackLayer);
    }

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        if (attacker.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1 * math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (attacker.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        isHurt = true;
        anim.SetTrigger("hurt");

        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    public void OnDie()
    {
        gameObject.layer = 2;
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.SetBool("dead", true);
        isDead = true;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
}

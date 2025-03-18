using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;

    private PlayerAnimation pa;

    public Vector2 inputDirection;//移动方向

    public float Speed;//移动速度

    public float jumpSpeed;//跳跃速度

    public bool jumping;//跳跃状态

    private float jumpHoldCounter;

    private float jumpBufferCounter;//跳跃提前输入倒计时

    private Rigidbody2D rb;

    private Vector2 _velocity;//修改缓冲

    private SpriteRenderer sR;

    private PhysicsCheck pc;

    public bool walking;

    public bool isHurt;
    public float hurtForce;

    public bool isDead;

    public bool isAttack;

    public SceneLoadEventSO SceneLoadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    private void Awake()
    {
        inputControl = new PlayerInputControl();
        Speed = 300;
        jumpSpeed = 400;
        _velocity = new Vector2();
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        pc = GetComponent<PhysicsCheck>();
        pa = GetComponent<PlayerAnimation>();
        isHurt = false;

        inputControl.GamePlay.Jump.started += StartJump;
        inputControl.GamePlay.Jump.canceled += FinishJump;
        inputControl.GamePlay.Walk.started += ChangeWalk;
        inputControl.GamePlay.Attack.started += PlayerAttack;
        walking = false;
        inputControl.Enable();
    }

    

    private void OnEnable()
    {
        
        SceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        SceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }

    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if(!isHurt)
        {
            Move();
        
            if (pc.isGround && jumpBufferCounter > 0)
            {
                jumping = true;
                jumpBufferCounter = 0;
                jumpHoldCounter = 1f;
                Jump();
            }
            else if (jumping && jumpHoldCounter>0)
            {
                Jump();
                jumpHoldCounter -= Time.fixedDeltaTime;
            }
                if (jumpBufferCounter > 0 && !jumping)
                {
                    jumpBufferCounter -= Time.fixedDeltaTime;
                }
        }
    }

    private void OnLoadEvent(GameSceneSO arg0,Vector3 arg1,bool arg2)
    {
        inputControl.GamePlay.Disable();
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputControl.GamePlay.Enable();
    }
    private void Move()
    {
        if (walking)
        {
            _velocity.x = inputDirection.x * Speed * 0.5f * Time.deltaTime;
        }
        else
        { 
            _velocity.x = inputDirection.x * Speed * Time.deltaTime; 
        }
        _velocity.y = rb.velocity.y;

        rb.velocity = _velocity;

        if(inputDirection.x > 0)
        {
            sR.flipX = false;
        }
        else if(inputDirection.x < 0)
        {
            sR.flipX = true;
        }
    }
    private void Jump()
    {
        _velocity.x = rb.velocity.x; 
        _velocity.y = jumpSpeed * Time.deltaTime * jumpHoldCounter;

        rb.velocity = _velocity;
    }
    private void StartJump(InputAction.CallbackContext obj)
    {
        jumpBufferCounter = 0.1f;
    }
    private void FinishJump(InputAction.CallbackContext obj)
    {
        jumping = false;
        jumpBufferCounter = 0;
    }
    private void ChangeWalk(InputAction.CallbackContext obj)
    {
        if(walking)
        {
            walking = false;
        }
        else
        {
            walking = true;
        }
    }
    public void PlayerAttack(InputAction.CallbackContext obj)
    {
        if(!isHurt)
        {
            pa.Playattack();
            isAttack = true;
        }
    }
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();
    }
}

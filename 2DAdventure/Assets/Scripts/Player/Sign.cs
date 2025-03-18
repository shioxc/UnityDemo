using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private PlayerInputControl pic;

    public GameObject signSpirte;

    public Transform playerTrans;

    private bool canPress;

    private Animator anim;

    private IInteractable targetItem;

    private void Awake()
    {
        anim = signSpirte.GetComponent<Animator>();

        pic = new PlayerInputControl();
        pic.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        pic.GamePlay.Confirm.started += OnConfirm;
    }

    

    private void Update()
    {
        signSpirte.GetComponent<SpriteRenderer>().enabled = canPress;
        signSpirte.transform.localScale = playerTrans.localScale;
    }
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }
    private void OnActionChange(object obj, InputActionChange change)
    {
        if(change == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;
            switch(d.device)
            {
                case Keyboard:
                    anim.Play("keyboard");
                    break;
                //case Éè±¸Ãû
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
        else
            canPress = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}

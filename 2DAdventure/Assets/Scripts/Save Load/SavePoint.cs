using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable
{
    public SpriteRenderer spriteRenderer;

    public GameObject lightObj;

    public Sprite darkSprite;
    public Sprite lightSprite;

    public bool isDone;

    public VoidEventSO SaveGameEvent;
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
        if(lightObj != null )
            lightObj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        if(!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;
            if (lightObj != null)
                lightObj.SetActive(true);

            SaveGameEvent.RasiseEvent();

            this.gameObject.tag = "Untagged";
        }
    }
}

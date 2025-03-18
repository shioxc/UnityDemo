using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour,ISaveable
{
    public float MaxHealth;
    public float CurHealth;

    public float invulnerableDuration;//无敌时间
    private float invulnerableCounter;//无敌计时器
    private bool invulnerable;//无敌状态

    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;
    public UnityEvent<Character> OnHealthChange;

    public VoidEventSO newGameEvent;

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void Start()
    {
        CurHealth = MaxHealth;
    }

    private void NewGame()
    {
        CurHealth = MaxHealth;
        OnHealthChange?.Invoke(this);
        invulnerable = false;
    }

    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Water"))
        {
            if(CurHealth >0)
            {
                CurHealth = 0;
                OnHealthChange?.Invoke(this);
                OnDie?.Invoke();
            }
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if(invulnerable)
            return;
        if (CurHealth > attacker.attack)
        {
            CurHealth -= attacker.attack;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            CurHealth = 0;
            OnDie?.Invoke(); 
        }
        OnHealthChange?.Invoke(this);
    }

    private void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }
    public void GetSaveData(Data data)
    {
        string id = GetDataID().ID;
        bool positionUpdated = false;
        for (int i = 0; i < data.characterPosDict.Count; i++)
        {
            if (data.characterPosDict[i].key == id)
            {
                data.characterPosDict[i] = new Data.KeyValuePair<string, Vector3>(id, transform.position);
                positionUpdated = true;
                break;
            }
        }
        if (!positionUpdated)
        {
            data.characterPosDict.Add(new Data.KeyValuePair<string, Vector3>(id, transform.position));
        }
        bool healthUpdated = false;
        string healthKey = id + "health";
        for (int i = 0; i < data.floatSaveData.Count; i++)
        {
            if (data.floatSaveData[i].key == healthKey)
            {
                data.floatSaveData[i] = new Data.KeyValuePair<string, float>(healthKey, this.CurHealth);
                healthUpdated = true;
                break;
            }
        }
        if (!healthUpdated)
        {
            data.floatSaveData.Add(new Data.KeyValuePair<string, float>(healthKey, this.CurHealth));
        }
    }
    public void LoadData(Data data)
    {
        string id = GetDataID().ID;
        CurHealth = MaxHealth;
        OnHealthChange?.Invoke(this);
        foreach (var entry in data.characterPosDict)
        {
            if (entry.key == id)
            {
                transform.position = entry.value;
                break;
            }
        }

        string healthKey = id + "health";

        foreach (var entry in data.floatSaveData)
        {

            if (entry.key == healthKey)
            {
                this.CurHealth = entry.value;
                OnHealthChange?.Invoke(this);
                break;
            }
        }
    }
}
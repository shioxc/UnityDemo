using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IInteractable,ISaveable
{
    private SpriteRenderer spriteRenderer;

    public Sprite openSprite;

    public Sprite closeSprite;

    public bool isDone;

    private void Awake()
    {
        spriteRenderer =GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }
    public void TriggerAction()
    {
        Debug.Log("Open Chest");
        if (!isDone)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        spriteRenderer.sprite = openSprite;
        isDone = true;
        this.gameObject.tag = "Untagged";
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        string id = GetDataID().ID;

        UpdateOrAddData(data.chestState, id, isDone);
    }

    public void LoadData(Data data)
    {
        string id = GetDataID().ID;

        foreach (var entry in data.chestState)
        {
            if (entry.key == id)
            {
                isDone = entry.value;
                break;
            }
        }
    }
    private void UpdateOrAddData<T>(List<Data.KeyValuePair<string, T>> list, string key, T value)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].key == key)
            {
                list[i] = new Data.KeyValuePair<string, T>(key, value);
                return;
            }
        }
        list.Add(new Data.KeyValuePair<string, T>(key, value));
    }
}

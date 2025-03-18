using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private List<ISaveable> saveableList = new List<ISaveable>();

    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    private Data saveData;
    public GameObject player;
    private string savePath;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            saveData = new Data();
            savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }

    private void Update()
    {
        
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
            Debug.Log($"ע���������: {saveable.GetType().Name}, ID: {saveable.GetDataID().ID}");
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        Debug.Log($"��������: {saveable.GetType().Name}, ID: {saveable.GetDataID().ID}");
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
        Debug.Log("��Ϸ�ѱ���: " + savePath);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath); // ��ȡ�ļ�
            Data data = JsonUtility.FromJson<Data>(json); // �����л�

            Debug.Log("��Ϸ�Ѽ���: " + savePath);
            saveData = data;
        }
        else
        {
            Debug.LogWarning("δ�ҵ��浵�ļ�");
            saveData = new Data();
        }
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        player.GetComponent<SpriteRenderer>().enabled = false;
        foreach (var saveable in saveableList)
        {
            if(saveData != null)
            saveable.LoadData(saveData);
        }
        player.GetComponent<SpriteRenderer>().enabled = true;

    }
}

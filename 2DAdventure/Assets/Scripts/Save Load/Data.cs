using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    [System.Serializable]
    public class KeyValuePair<TKey, TValue>
    {
        public TKey key;
        public TValue value;

        public KeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    // ��ɫλ���ֵ�
    public List<KeyValuePair<string, Vector3>> characterPosDict = new List<KeyValuePair<string, Vector3>>();

    // ���������ֵ�
    public List<KeyValuePair<string, float>> floatSaveData = new List<KeyValuePair<string, float>>();

    // ����״̬�ֵ�
    public List<KeyValuePair<string, bool>> chestState = new List<KeyValuePair<string, bool>>();

    // ������Ϣ
    public string sceneToSave;

    // ���泡��
    public void SaveGameScene(GameSceneSO savedScene)
    {
        sceneToSave = JsonUtility.ToJson(savedScene);
    }

    // ���س���
    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }

    // ��ӽ�ɫλ��
    public void AddCharacterPosition(string characterId, Vector3 position)
    {
        characterPosDict.Add(new KeyValuePair<string, Vector3>(characterId, position));
    }

    // ��Ӹ�������
    public void AddFloatData(string key, float value)
    {
        floatSaveData.Add(new KeyValuePair<string, float>(key, value));
    }

    // ��ӱ���״̬
    public void AddChestState(string chestId, bool isOpen)
    {
        chestState.Add(new KeyValuePair<string, bool>(chestId, isOpen));
    }
}
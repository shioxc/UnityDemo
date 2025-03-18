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

    // 角色位置字典
    public List<KeyValuePair<string, Vector3>> characterPosDict = new List<KeyValuePair<string, Vector3>>();

    // 浮点数据字典
    public List<KeyValuePair<string, float>> floatSaveData = new List<KeyValuePair<string, float>>();

    // 宝箱状态字典
    public List<KeyValuePair<string, bool>> chestState = new List<KeyValuePair<string, bool>>();

    // 场景信息
    public string sceneToSave;

    // 保存场景
    public void SaveGameScene(GameSceneSO savedScene)
    {
        sceneToSave = JsonUtility.ToJson(savedScene);
    }

    // 加载场景
    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }

    // 添加角色位置
    public void AddCharacterPosition(string characterId, Vector3 position)
    {
        characterPosDict.Add(new KeyValuePair<string, Vector3>(characterId, position));
    }

    // 添加浮点数据
    public void AddFloatData(string key, float value)
    {
        floatSaveData.Add(new KeyValuePair<string, float>(key, value));
    }

    // 添加宝箱状态
    public void AddChestState(string chestId, bool isOpen)
    {
        chestState.Add(new KeyValuePair<string, bool>(chestId, isOpen));
    }
}
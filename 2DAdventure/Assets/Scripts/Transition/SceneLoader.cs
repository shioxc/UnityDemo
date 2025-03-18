using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour,ISaveable
{
    public SceneLoadEventSO loadEventSO;

    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    
    public GameSceneSO currentLoadScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;
    private bool fadeScrenen;

    public float fadeDuration;

    public Transform playerTrans;
    public SpriteRenderer spriteRenderer;
    public PlayerStatBar ps;

    private bool isLoading;

    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;


    public Vector3 firstPosition;
    public Vector3 menuPosition;

    public VoidEventSO newGameEvent;
    public Camera Camera;

    public VoidEventSO backToMenuEvent;
    public VoidEventSO saveDataEvent;
    private Dictionary<string, Vector3> characterPosCache;

    private void Awake()
    {
        
    }

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame();
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnBackToMenuEvent()
    {
        sceneToLoad = menuScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad,menuPosition, true);
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad,firstPosition,true);
    }
    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if(isLoading)
        {
            return;
        }
        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScrenen = fadeScreen;

        if (currentLoadScene != null)
            StartCoroutine(UnLoadPreviousScene());
        else
            LoadNewScene();
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if(fadeScrenen)
        {
            fadeEvent.FadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);
        yield return currentLoadScene.sceneReference.UnLoadScene();

        playerTrans.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingoption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingoption.Completed += OnLoadCompleted;
    }
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadScene = sceneToLoad;

        playerTrans.position = positionToGo;
        if (currentLoadScene.sceneType == SceneType.Location)
        {
            ps.gameObject.SetActive(true);
            Camera.gameObject.SetActive(true);
        }
        else
        {
            ps.gameObject.SetActive(false);
            spriteRenderer.flipX = false;
            Camera.gameObject.SetActive(false);
        }

        playerTrans.gameObject.SetActive(true);
        if (fadeScrenen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;
        if (currentLoadScene.sceneType == SceneType.Location)
            afterSceneLoadedEvent.RasiseEvent();
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefination>().ID;

        if (characterPosCache == null)
        {
            characterPosCache = new Dictionary<string, Vector3>();
            foreach (var entry in data.characterPosDict)
            {
                characterPosCache[entry.key] = entry.value;
            }
        }

        // ≤È’“ÕÊº“Œª÷√
        if (characterPosCache.TryGetValue(playerID, out Vector3 position))
        {
            positionToGo = position; 
            sceneToLoad = data.GetSavedScene(); 
            OnLoadRequestEvent(sceneToLoad, positionToGo, true); 
        }
    }
}

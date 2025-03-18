using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar ps;

    public CharacterEventSO healthEvent;

    public SceneLoadEventSO unLoadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;

    public GameObject gameOverPanel;
    public GameObject restartBtn;
    public VoidEventSO backToMenuEvent;

    public Button settingsBtn;
    public GameObject pausePanel;

    public VoidEventSO pauseEvent;
    public FloatEventSO syncVolumeEvent;
    public Slider volumeSlider;

    private void Awake()
    {
        settingsBtn.onClick.AddListener(TogglePausePanel);
    }

    private void OnEnable()
    {
        healthEvent.onEventRasised += OnHealthEvent;
        unLoadedSceneEvent.LoadRequestEvent += OnUnloadSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

    private void OnDisable()
    {
        healthEvent.onEventRasised -= OnHealthEvent;
        unLoadedSceneEvent.LoadRequestEvent -= OnUnloadSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }

    private void TogglePausePanel()
    {
        if(pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseEvent.OnEventRaised();
            pausePanel.SetActive(true);
            Time.timeScale = 0;

        }
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }
    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }

    private void OnUnloadSceneEvent(GameSceneSO sceneToLoad,Vector3 arg1,bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        ps.gameObject.SetActive(!isMenu);
    }
    private void OnHealthEvent(Character character)
    {
        var persentage = character.CurHealth / character.MaxHealth;
        ps.OnHealthChange(persentage);
    }
}

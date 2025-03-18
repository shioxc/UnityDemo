using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeleportPoint : MonoBehaviour, IInteractable
{
    public Vector3 positonToGo;

    public GameSceneSO sceneToGo;

    public SceneLoadEventSO loadEventSO;
    public void TriggerAction()
    {
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positonToGo, true);
    }
}

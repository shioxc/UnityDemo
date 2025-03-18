using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaised;

    //深色化
    public void FadeIn(float duration)
    {
        RaisedEvent(Color.black, duration, true);
    }
    //透明化 
    public void FadeOut(float duration) 
    {
        RaisedEvent(Color.clear, duration, false);
    }

    public void RaisedEvent(Color target,float duration,bool fadeIn)
    {
        OnEventRaised?.Invoke(target,duration,fadeIn);
    }
}

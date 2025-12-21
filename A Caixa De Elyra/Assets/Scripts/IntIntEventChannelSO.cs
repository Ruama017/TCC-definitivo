using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Int Int Event Channel")]
public class IntIntEventChannelSO : ScriptableObject
{
    public UnityAction<int, int> OnEventRaised;

    public void RaiseEvent(int value1, int value2)
    {
        OnEventRaised?.Invoke(value1, value2);
    }
}

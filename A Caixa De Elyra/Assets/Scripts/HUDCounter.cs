using UnityEngine;
using TMPro;

public class HUDCounter : MonoBehaviour
{
    public IntIntEventChannelSO counterEvent;
    public TMP_Text counterText;

    private void OnEnable()
    {
        counterEvent.OnEventRaised += UpdateCounter;
    }

    private void OnDisable()
    {
        counterEvent.OnEventRaised -= UpdateCounter;
    }

    void UpdateCounter(int current, int total)
    {
        counterText.text = current + " / " + total;
    }
}

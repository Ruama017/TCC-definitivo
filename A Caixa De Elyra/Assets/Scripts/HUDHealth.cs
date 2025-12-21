using UnityEngine;
using TMPro;

public class HUDHealth : MonoBehaviour
{
    public IntIntEventChannelSO healthEvent;
    public TMP_Text healthText;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += UpdateHealth;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= UpdateHealth;
    }

    void UpdateHealth(int current, int max)
    {
        healthText.text = current + " / " + max;
    }
}

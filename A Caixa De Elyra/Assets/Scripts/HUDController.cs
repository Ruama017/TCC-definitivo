using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Event Channels")]
    public IntIntEventChannelSO healthEvent;   // vida atual / vida máxima
    public IntEventChannelSO crystalEvent;     // cristais
    public IntIntEventChannelSO counterEvent;  // coletados / total

    [Header("Vida")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Cristais")]
    public TMP_Text crystalText;

    [Header("Contador de Almas")]
    public TMP_Text counterText;

    // ------------------------------------------
    void OnEnable()
    {
        if (healthEvent != null)
            healthEvent.OnEventRaised += UpdateHealthUI;

        if (crystalEvent != null)
            crystalEvent.OnEventRaised += UpdateCrystalUI;

        if (counterEvent != null)
            counterEvent.OnEventRaised += UpdateCounterUI;
    }

    void OnDisable()
    {
        if (healthEvent != null)
            healthEvent.OnEventRaised -= UpdateHealthUI;

        if (crystalEvent != null)
            crystalEvent.OnEventRaised -= UpdateCrystalUI;

        if (counterEvent != null)
            counterEvent.OnEventRaised -= UpdateCounterUI;
    }

    // ------------------------------------------
    void UpdateHealthUI(int current, int max)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < current)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    void UpdateCrystalUI(int amount)
    {
        if (crystalText != null)
            crystalText.text = amount.ToString();
    }

    void UpdateCounterUI(int current, int total)
    {
        if (counterText != null)
            counterText.text = current + " / " + total;
    }
}

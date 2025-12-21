using UnityEngine;
using TMPro;

public class CounterManager : MonoBehaviour
{
    // ------------------------------------------
    // 🔵 SINGLETON
    // ------------------------------------------
    public static CounterManager Instance;

    // ------------------------------------------
    // 🔵 EVENT CHANNEL (Observer)
    // ------------------------------------------
    [Header("Event Channel")]
    public IntIntEventChannelSO counterEvent;
    // ------------------------------------------

    [Header("Contador")]
    public int totalCount = 5;
    public int currentCount = 0;

    [Header("UI (visual local)")]
    public TMP_Text counterText;

    [Header("Portal")]
    public PortalController portal;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (counterText == null)
            counterText = FindObjectOfType<TMP_Text>();

        if (portal == null)
            portal = FindObjectOfType<PortalController>();
    }

    void Start()
    {
        UpdateUI();

        if (counterEvent != null)
            counterEvent.RaiseEvent(currentCount, totalCount);
    }

    public void Increment()
    {
        currentCount++;

        UpdateUI();

        if (counterEvent != null)
            counterEvent.RaiseEvent(currentCount, totalCount);

        if (currentCount >= totalCount && portal != null)
            portal.ActivatePortal();
    }

    void UpdateUI()
    {
        if (counterText != null)
            counterText.text = currentCount + " / " + totalCount;
    }

    public bool AllSoulsCollected()
    {
        return currentCount >= totalCount;
    }
}

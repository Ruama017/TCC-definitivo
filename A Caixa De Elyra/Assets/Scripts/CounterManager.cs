using UnityEngine;
using TMPro; // Para TextMeshPro

public class CounterManager : MonoBehaviour
{
    public static CounterManager Instance; // Singleton

    [Header("Contador")]
    public int totalCount = 5;     // Total de Boglins da fase
    private int currentCount = 0;  // Quantos já foram coletados

    [Header("UI")]
    public TMP_Text counterText;   // Texto do Canvas mostrando "coletados / total"

    [Header("Portal")]
    public PortalController portal; // Referência ao portal da fase

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// Chamar quando um Boglin morrer e a alma chegar no contador
    /// </summary>
    public void Increment()
    {
        currentCount++;

        UpdateUI();

        // Ativa portal se todos os Boglins forem coletados
        if (currentCount >= totalCount)
        {
            if (portal != null)
                portal.ActivatePortal();
        }
    }

    private void UpdateUI()
    {
        if (counterText != null)
            counterText.text = currentCount + " / " + totalCount;
    }

    /// <summary>
    /// Retorna true se todas as almas foram coletadas
    /// </summary>
    public bool AllSoulsCollected()
    {
        return currentCount >= totalCount;
    }
}
using UnityEngine;
using TMPro;

public class CounterManager : MonoBehaviour
{
    public static CounterManager Instance; // Singleton

    //  Evento Observer
    public static event System.Action<int, int> OnCountChanged;

    [Header("Contador")]
    public int totalCount = 5;     // Total de Boglins da fase
    public int currentCount = 0;  // Quantos já foram coletados

    [Header("UI")]
    public TMP_Text counterText;   // Texto do Canvas mostrando "coletados / total"

    [Header("Portal")]
    public PortalController portal; // Referência ao portal da fase

    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Se não tiver sido arrastado pelo Inspector, procura na cena
        if (counterText == null)
        {
            counterText = FindObjectOfType<TMP_Text>();
            if (counterText == null)
                Debug.LogWarning(" CounterManager: Não encontrou TMP_Text na cena!");
        }

        if (portal == null)
        {
            portal = FindObjectOfType<PortalController>();
            if (portal == null)
                Debug.LogWarning(" CounterManager: Não encontrou PortalController na cena!");
        }
    }

    private void Start()
    {
        UpdateUI();

        //  Dispara evento inicial
        OnCountChanged?.Invoke(currentCount, totalCount);
    }

    /// <summary>
    /// Chamar quando um Boglin morrer e a alma chegar no contador
    /// </summary>
    public void Increment()
    {
        currentCount++;

        UpdateUI();

        // Dispara evento Observer
        OnCountChanged?.Invoke(currentCount, totalCount);

        // Ativa portal se todos os Boglins forem coletados
        if (currentCount >= totalCount)
        {
            if (portal != null)
                portal.ActivatePortal();
        }
    }

    public void UpdateUI()
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

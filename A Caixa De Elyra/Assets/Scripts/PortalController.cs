using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [Header("Sprites do Portal")]
    public SpriteRenderer spriteRenderer; // arraste o SpriteRenderer do portal aqui
    public Sprite activeSprite;           // sprite quando portal ativo
    public Sprite inactiveSprite;         // sprite quando portal inativo

    [Header("Configuração")]
    public string nextSceneName = "fase2"; // nome da próxima fase
    [HideInInspector] public bool isActive = false;

    [Header("Counter Manager")]
    public CounterManager counterManager;  // arraste o CounterManager aqui

    void Start()
    {
        // Inicializa portal como inativo
        isActive = false;
        if (spriteRenderer != null && inactiveSprite != null)
            spriteRenderer.sprite = inactiveSprite;
    }

    void Update()
    {
        // Verifica se todas as almas foram coletadas
        if (!isActive && counterManager != null && counterManager.AllSoulsCollected())
        {
            ActivatePortal();
        }
    }

    /// <summary>
    /// Ativa o portal visualmente e funcionalmente
    /// </summary>
    public void ActivatePortal()
    {
        isActive = true;

        // Troca o sprite para mostrar que está ativo
        if (spriteRenderer != null && activeSprite != null)
            spriteRenderer.sprite = activeSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Só permite passar se estiver ativo e o player entrar
        if (isActive && collision.CompareTag("Player"))
        {
            // Carrega a próxima fase
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

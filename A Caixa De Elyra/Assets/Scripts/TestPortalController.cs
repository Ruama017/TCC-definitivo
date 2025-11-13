using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPortalController : MonoBehaviour
{
    [Header("Sprites do Portal")]
    public SpriteRenderer spriteRenderer; 
    public Sprite activeSprite;           
    public Sprite inactiveSprite;         

    [Header("Configuração")]
    public string nextSceneName = "fase2"; 
    [HideInInspector] public bool isActive = false;

    void Start()
    {
        // FORÇA portal ativo para teste
        isActive = true;
        if (spriteRenderer != null && activeSprite != null)
            spriteRenderer.sprite = activeSprite;

        Debug.Log("Portal ativado para teste!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.CompareTag("Player"))
        {
            Debug.Log("Player entrou no portal!");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

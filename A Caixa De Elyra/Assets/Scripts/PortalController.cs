using UnityEngine;

public class PortalController : MonoBehaviour
{
    [Header("Sprites do Portal")]
    public Sprite inactiveSprite;
    public Sprite activeSprite;

    [Header("Configuração")]
    public bool isActive = false;  // o portal começa desativado

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = inactiveSprite;  // garante que comece inativo
    }

    void Update()
    {
        // Apenas atualiza sprite se isActive mudou
        if (isActive)
            sr.sprite = activeSprite;
        else
            sr.sprite = inactiveSprite;
    }

    // Função para ativar portal
    public void ActivatePortal()
    {
        isActive = true;
        sr.sprite = activeSprite;
    }

    // Função chamada quando o player colide com o portal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.CompareTag("Player"))
        {
            Debug.Log("Player passou pelo portal!");
            // Aqui você pode colocar a lógica de mudar de fase
        }
    }
}

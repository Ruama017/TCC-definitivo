using UnityEngine;

public class PonteQuebravel : MonoBehaviour
{
    [Header("Configuração")]
    public Animator animator;          // Arraste o Animator do GameObject da ponte
    public string triggerQuebrar = "Quebrar"; // Nome do trigger no Animator
    private bool jaQuebrou = false;    // Evita que quebre mais de uma vez

    [Header("Destruição")]
    public float tempoParaDestruir = 1f; // Tempo para destruir após começar a animação

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!jaQuebrou && collision.collider.CompareTag("Player"))
        {
            jaQuebrou = true;

            // Ativa a animação
            if (animator != null)
                animator.SetTrigger(triggerQuebrar);

            // Desativa o collider para o player cair
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;

            // Destroi o objeto após a animação
            Destroy(gameObject, tempoParaDestruir);
        }
    }
}

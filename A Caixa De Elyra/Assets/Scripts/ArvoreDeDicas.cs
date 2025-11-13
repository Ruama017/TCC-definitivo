using UnityEngine;

public class ArvoreDeDicas : MonoBehaviour
{
    public GameObject baloes; // referência ao grupo de balões

    private void Start()
    {
        if (baloes != null)
            baloes.SetActive(false); // começa invisível
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (baloes != null)
                baloes.SetActive(true); // mostra os balões
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (baloes != null)
                baloes.SetActive(false); // esconde de novo
        }
    }
}

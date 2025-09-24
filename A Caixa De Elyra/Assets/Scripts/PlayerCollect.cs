using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    private CristalManager manager;

    void Start()
    {
        // Procura o CristalManager automaticamente
        manager = FindObjectOfType<CristalManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cristal"))
        {
            manager.ColetarCristal();
            Destroy(collision.gameObject);
        }
    }
}

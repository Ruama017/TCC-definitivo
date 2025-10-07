using UnityEngine;

public class CollectibleCrystal : MonoBehaviour
{
    [Header("Configurações do cristal")]
    public int healthAmount = 1;   // quantos corações curar
    public int crystalAmount = 1;  // quantos cristais adicionar

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Pega o script PlayerHealth
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Cura o player
                if (healthAmount > 0)
                    playerHealth.Heal(healthAmount);

                // Adiciona cristais
                if (crystalAmount > 0)
                    playerHealth.CollectCrystal(crystalAmount);
            }

            // Destrói o cristal após coleta
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class FlowerCollectible : MonoBehaviour
{
    public int shieldDuration = 15; // duração do escudo em segundos

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerShield playerShield = other.GetComponent<PlayerShield>();
            if(playerShield != null)
            {
                playerShield.CollectShield(shieldDuration);
            }
            Destroy(gameObject); // remove a flor após coletar
        }
    }
}

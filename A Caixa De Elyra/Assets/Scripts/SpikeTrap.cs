using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 1; // caso queira usar dano normal também
    public AudioSource spikeSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            // Toca som de espinho
            if (spikeSound != null)
                spikeSound.Play();

            // Mata o player instantaneamente
            player.InstantDeath();
        }
    }
}
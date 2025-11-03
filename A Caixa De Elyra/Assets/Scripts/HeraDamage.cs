using UnityEngine;

public class HeraDamage : MonoBehaviour
{
    public int damage = 1;
    private bool isDestroyed = false; // evita múltiplos golpes
    private Animator anim;
    private Collider2D col;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return; // já destruída, não repete
        isDestroyed = true;

        // 1️⃣ Dispara animação
        if (anim != null)
        {
            anim.SetTrigger("Destroyed"); // nome exato do trigger no Animator
        }

        // 2️⃣ Desativa collider para evitar múltiplos ataques
        if (col != null)
            col.enabled = false;

        // 3️⃣ Espera duração do clip "Destroyed" antes de destruir
        float waitTime = 0.6f; // fallback
        if (anim != null)
        {
            AnimationClip clip = null;
            foreach (var c in anim.runtimeAnimatorController.animationClips)
            {
                if (c.name == "Destroyed")
                {
                    clip = c;
                    break;
                }
            }

            if (clip != null)
                waitTime = clip.length;
        }

        Destroy(gameObject, waitTime);
    }
}

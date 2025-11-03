using UnityEngine;
using System.Collections;

public class HeraBarrier : MonoBehaviour
{
    public int damage = 1;
    public Animator animator;

    private bool isDestroyed = false;
    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            if (player.IsAttacking && !isDestroyed)
            {
                StartCoroutine(DestroyHera());
            }
            else if (!player.IsAttacking && playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    private IEnumerator DestroyHera()
    {
        isDestroyed = true;

        // 1️⃣ Desativa colisores e comportamento
        if (col != null) col.enabled = false;

        // 2️⃣ Dispara a animação
        if (animator != null)
            animator.SetTrigger("Destroyed");

        // 3️⃣ Espera pelo fim da animação de forma simples (só o tempo do clip real)
        if (animator != null)
        {
            AnimationClip clip = null;
            foreach (var c in animator.runtimeAnimatorController.animationClips)
            {
                if (c.name == "Destroyed")
                {
                    clip = c;
                    break;
                }
            }

            if (clip != null)
                yield return new WaitForSeconds(clip.length);
            else
                yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        // 4️⃣ Destrói o objeto só depois da animação
        Destroy(gameObject);
    }
}

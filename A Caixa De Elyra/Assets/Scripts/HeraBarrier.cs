using UnityEngine;
using System.Collections;

public class HeraBarrier : MonoBehaviour
{
    public int damage = 1;
    public Animator animator;
    public float destroyDelay = 0.5f;

    private bool isDestroyed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            if (player.IsAttacking && !isDestroyed)
            {
                // Player atacou: animação e destruição
                StartCoroutine(DestroyHera());
            }
            else if (!player.IsAttacking && playerHealth != null)
            {
                // Player não atacou: leva dano
                playerHealth.TakeDamage(damage);
            }
        }
    }

    private IEnumerator DestroyHera()
    {
        isDestroyed = true;

        if (animator != null)
            animator.SetTrigger("Destroyed"); // trigger "Destroy" na animação

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}

using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [Header("Configurações")]
    public int damage = 1;              // Dano base do player
    public float activeTime = 0.2f;     // Tempo que considera para ataques rápidos

    private PlayerController playerController;

    private void Awake()
    {
        // Garante que a referência ao PlayerController do pai seja correta
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryDamage(collision);
    }

    private void TryDamage(Collider2D collision)
    {
        if (playerController == null)
            return;

        // Voglin
        VoglinController voglin = collision.GetComponent<VoglinController>();
        if (voglin != null)
        {
            voglin.TakeDamage(damage);
            return;
        }

        // Boglin
        BoglinController boglin = collision.GetComponent<BoglinController>();
        if (boglin != null)
        {
            boglin.TakeDamage(damage);
            return;
        }

        // Hera de espinhos (script HeraDamage)
        HeraDamage hera = collision.GetComponent<HeraDamage>();
        if (hera != null)
        {
            hera.TakeDamage(damage);
            return;
        }

        // NitroMortis (boss real)
        NitroMortis nitro = collision.GetComponentInParent<NitroMortis>();
        if (nitro != null)
        {
            // Passa apenas o bool do super
            nitro.TakeDamage(playerController.hasSuper);

            Debug.Log("[DEBUG] Player atingiu NitroMortis! Super ativo: " + playerController.hasSuper);
            return;
        }

        // ThorneBossController
        ThorneBossController thorne = collision.GetComponent<ThorneBossController>();
        if (thorne != null)
        {
            bool playerHasSuper = playerController.hasSuper; // pega super
            thorne.TakeDamage(damage, playerHasSuper);       // passa os 2 parâmetros
            Debug.Log("[DEBUG] Player atingiu Thorne! Dano: " + damage + ", Super ativo: " + playerHasSuper);
            return;
        }
    }
}

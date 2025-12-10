using UnityEngine;
using System.Collections.Generic;

public class PlayerAttackHitbox : MonoBehaviour
{
    [Header("Configurações")]
    public int damage = 1;

    private PlayerController playerController;

    // lista para evitar dano múltiplo no mesmo ataque
    private HashSet<Collider2D> alreadyHit = new HashSet<Collider2D>();

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        // sempre limpa lista quando o ataque começa
        alreadyHit.Clear();
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

        // impede hits repetidos no mesmo ataque
        if (alreadyHit.Contains(collision))
            return;

        alreadyHit.Add(collision);

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

        // Hera de espinhos
        HeraDamage hera = collision.GetComponent<HeraDamage>();
        if (hera != null)
        {
            hera.TakeDamage(damage);
            return;
        }

        // NitroMortis (boss)
        NitroMortis nitro = collision.GetComponentInParent<NitroMortis>();
        if (nitro != null)
        {
            nitro.TakeDamage(playerController.hasSuper);
            return;
        }

        // Thorne
        ThorneBossController thorne = collision.GetComponent<ThorneBossController>();
        if (thorne != null)
        {
            thorne.TakeDamage(damage, playerController.hasSuper);
            return;
        }
    }
}

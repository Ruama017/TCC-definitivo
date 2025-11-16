using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [Header("Configurações")]
    public int damage = 1;              // Dano base do player
    public float activeTime = 0.2f;     // Tempo que considera para ataques rápidos

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

        // NitroMortisBoss
        NitroMortisBoss nitro = collision.GetComponent<NitroMortisBoss>();
        if (nitro != null)
        {
            nitro.TakeDamage(damage, false); // false = player sem super
            Debug.Log("[DEBUG] Player atingiu NitroMortis! Dano: " + damage);
            return;
        }

        // ThorneBossController
        ThorneBossController thorne = collision.GetComponent<ThorneBossController>();
        if (thorne != null)
        {
            thorne.TakeDamage(damage, false);
            Debug.Log("[DEBUG] Player atingiu Thorne! Dano: " + damage);
            return;
        }
    }
}

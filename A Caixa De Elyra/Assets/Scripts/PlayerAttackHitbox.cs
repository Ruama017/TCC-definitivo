using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [Header("Configurações")]
    public int damage = 1;             
    public float activeTime = 0.2f;    

    private bool isActive = false;

    private void OnEnable()
    {
        isActive = true;
        Invoke(nameof(DeactivateHitbox), activeTime);
    }

    private void DeactivateHitbox()
    {
        isActive = false;
        gameObject.SetActive(false); 
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
        if (!isActive) return;

        // Voglin
        VoglinController voglin = collision.GetComponent<VoglinController>();
        if (voglin != null)
        {
            voglin.TakeDamage(damage);
            DeactivateHitbox();
            return;
        }

        // Boglin
        BoglinController boglin = collision.GetComponent<BoglinController>();
        if (boglin != null)
        {
            boglin.TakeDamage(damage);
            DeactivateHitbox();
            return;
        }

        // Hera de espinhos (script HeraDamage)
        HeraDamage hera = collision.GetComponent<HeraDamage>();
        if (hera != null)
        {
            hera.TakeDamage(damage); // ou o método correto do seu script
            DeactivateHitbox();
            return;
        }
    }
}

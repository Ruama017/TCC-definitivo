using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [Header("Configurações")]
    public int damage = 1;             // Dano que o ataque causa
    public float activeTime = 0.2f;    // Tempo que a hitbox fica ativa
    private bool isActive = false;

    private void OnEnable()
    {
        isActive = true;
        Invoke(nameof(DeactivateHitbox), activeTime);
    }

    private void DeactivateHitbox()
    {
        isActive = false;
        gameObject.SetActive(false); // Desativa a hitbox após o tempo
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        // Só ataca inimigos com BoglinController
        BoglinController boglin = collision.GetComponent<BoglinController>();
        if (boglin != null)
        {
            boglin.TakeDamage(damage);
        }
    }
}

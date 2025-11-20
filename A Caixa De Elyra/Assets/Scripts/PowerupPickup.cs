using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerupPickup : MonoBehaviour
{
    public float superDuration = 6f;
    public GameObject pickupVFX;   // você não tem efeito visual, pode deixar vazio
    public AudioClip pickupSfx;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) Ativa o componente PlayerSuper (se existir)
        PlayerSuper ps = other.GetComponent<PlayerSuper>();
        if (ps == null) ps = other.GetComponentInChildren<PlayerSuper>();

        if (ps != null)
            ps.ActivateSuper(superDuration);

        // 2) Marca também no PlayerHealth (se o seu PlayerHealth usar hasBootSuper)
        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.CollectBoot(); // método que você já tem em PlayerHealth
        }

        // efeitos e som
        // REMOVIDO: Instantiate(pickupVFX, transform.position, Quaternion.identity);
        if (pickupSfx != null) AudioSource.PlayClipAtPoint(pickupSfx, Camera.main.transform.position);

        Destroy(gameObject);
    }
}

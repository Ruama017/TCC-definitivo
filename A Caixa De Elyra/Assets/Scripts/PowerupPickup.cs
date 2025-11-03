using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerupPickup : MonoBehaviour
{
    public float superDuration = 6f;
    public GameObject pickupVFX;
    public AudioClip pickupSfx;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerSuper ps = other.GetComponent<PlayerSuper>();
        if (ps == null) ps = other.GetComponentInChildren<PlayerSuper>();

        if (ps != null) ps.ActivateSuper(superDuration);

        if (pickupVFX != null) Instantiate(pickupVFX, transform.position, Quaternion.identity);
        if (pickupSfx != null) AudioSource.PlayClipAtPoint(pickupSfx, Camera.main.transform.position);

        Destroy(gameObject);
    }
}

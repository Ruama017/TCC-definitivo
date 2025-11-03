using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SoulCollectible : MonoBehaviour
{
    public GameObject collectVFX;
    public AudioClip collectSfx;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Incrementa contador via CounterManager
        if (CounterManager.Instance != null)
            CounterManager.Instance.Increment();

        // Feedback visual e sonoro
        if (collectVFX != null)
            Instantiate(collectVFX, transform.position, Quaternion.identity);

        if (collectSfx != null)
            AudioSource.PlayClipAtPoint(collectSfx, Camera.main.transform.position);

        // Destrói a alma
        Destroy(gameObject);
    }
}

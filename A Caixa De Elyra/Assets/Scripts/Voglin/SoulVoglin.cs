using UnityEngine;

public class SoulVoglin : MonoBehaviour
{
    [Header("Coleta")]
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Toca som de coleta
            if (collectSound != null)
                audioSource.PlayOneShot(collectSound);

            // Atualiza o contador
            if (CounterManager.Instance != null)
                CounterManager.Instance.Increment();

            // Destrói a alma
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Para acessar de qualquer lugar
    private AudioSource sfxSource;       // Efeitos sonoros

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // opcional: mantém entre cenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Cria o AudioSource automaticamente se não tiver
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Toca um efeito sonoro por cima da música
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
}


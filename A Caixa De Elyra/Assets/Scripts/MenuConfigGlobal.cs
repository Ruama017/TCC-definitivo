using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuConfigGlobal : MonoBehaviour
{
    public GameObject painelConfig;
    public Slider sliderVolume;
    public AudioSource musicaFundo;

    private static MenuConfigGlobal instance;

    void Awake()
    {
        // Singleton para não duplicar
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (musicaFundo != null && sliderVolume != null)
            sliderVolume.value = musicaFundo.volume;

        sliderVolume.onValueChanged.AddListener(AlterarVolume);
    }

    public void AbrirConfig()
    {
        painelConfig.SetActive(true);
    }

    public void FecharConfig()
    {
        painelConfig.SetActive(false);
    }

    public void AlterarVolume(float valor)
    {
        if (musicaFundo != null)
            musicaFundo.volume = valor;
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu"); // substitua pelo nome exato da cena do menu
    }
}


using UnityEngine;
using UnityEngine.UI;

public class PainelConfigController : MonoBehaviour
{
    [Header("UI")]
    public GameObject painelConfig;
    public Button btnAbrir;
    public Button btnFechar;
    public Slider sliderVolume;

    [Header("Audio")]
    public AudioSource musicaFundo;  // Trilha sonora
    public AudioSource sfxClick;     // Efeito de clique
    public AudioClip somClique;      // Clip do clique

    void Start()
    {
        btnAbrir.onClick.AddListener(AbrirPainel);
        btnFechar.onClick.AddListener(FecharPainel);
        sliderVolume.onValueChanged.AddListener(delegate { AjustaVolume(); });
        
        // Música toca automaticamente pelo AudioSource (Play On Awake)
    }

    void AbrirPainel()
    {
        sfxClick.PlayOneShot(somClique);  // toca som sem interferir na trilha
        painelConfig.SetActive(true);
    }

    void FecharPainel()
    {
        sfxClick.PlayOneShot(somClique);  // toca som sem interferir na trilha
        painelConfig.SetActive(false);
    }

    void AjustaVolume()
    {
        musicaFundo.volume = sliderVolume.value;
    }
}




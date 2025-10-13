using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // precisa pra trocar de cena

public class PainelConfigController : MonoBehaviour
{
    [Header("UI")]
    public GameObject painelConfig;
    public Button btnAbrir;
    public Button btnFechar;
    public Button btnMenu; // botão de voltar pro menu

    [Header("Audio")]
    public AudioSource musicaFundo;  // Trilha sonora
    public AudioSource sfxClick;     // Efeito de clique
    public AudioClip somClique;      // Clip do clique

    [Header("Cena")]
    public string nomeCenaMenu = "Menu"; // nome da cena do menu

    void Start()
    {
        btnAbrir.onClick.AddListener(AbrirPainel);
        btnFechar.onClick.AddListener(FecharPainel);
        btnMenu.onClick.AddListener(VoltarMenu); // adiciona evento ao botão
        // sliderVolume removido
    }

    void AbrirPainel()
    {
        if (sfxClick != null && somClique != null)
            sfxClick.PlayOneShot(somClique);  // toca som sem interferir na trilha
        painelConfig.SetActive(true);
    }

    void FecharPainel()
    {
        if (sfxClick != null && somClique != null)
            sfxClick.PlayOneShot(somClique);  // toca som sem interferir na trilha
        painelConfig.SetActive(false);
    }

    void VoltarMenu()
    {
        if (sfxClick != null && somClique != null)
            sfxClick.PlayOneShot(somClique);  // toca som antes de sair
        SceneManager.LoadScene(nomeCenaMenu); // carrega a cena do menu
    }
}





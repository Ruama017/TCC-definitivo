using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    [Header("Canvases")]
    public GameObject victoryCanvas;      // Tela de vitória
    public GameObject cutsceneCanvas;     // Cutscene

    [Header("Cutscene")]
    public Image cutsceneImage;           // Imagem da cutscene
    public Sprite[] cutsceneSprites;
    public float timePerImage = 4f;
    public AudioSource cutsceneMusic;

    [Header("Botões")]
    public UnityEngine.UI.Button restartButton;
    public UnityEngine.UI.Button menuButton;

    [Header("Flags")]
    public bool ThorneIsDead = false;     // Flag pública para morte do Thorne

    private void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Desativa canvases no início
        if (victoryCanvas != null)
            victoryCanvas.SetActive(false);
        if (cutsceneCanvas != null)
            cutsceneCanvas.SetActive(false);

        // Configura botões
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        if (menuButton != null)
            menuButton.onClick.AddListener(ReturnToMenu);
    }

    /// <summary>
    /// Dispara a cutscene + vitória
    /// </summary>
    public void TriggerVictorySequence()
    {
        if (!CounterManager.Instance.AllSoulsCollected())
        {
            Debug.Log("[VictoryManager] Nem todos os monstros foram coletados. Vitória bloqueada.");
            return;
        }

        StartCoroutine(VictoryFlow());
    }

    private IEnumerator VictoryFlow()
    {
        // 1: Ativa a cutscene
        if (cutsceneCanvas != null)
            cutsceneCanvas.SetActive(true);

        if (cutsceneMusic != null)
            cutsceneMusic.Play();

        // Loop apenas pelas imagens
        for (int i = 0; i < cutsceneSprites.Length; i++)
        {
            if (cutsceneImage != null)
                cutsceneImage.sprite = cutsceneSprites[i];

            yield return new WaitForSecondsRealtime(timePerImage);
        }

        // 2: Ativa a tela de vitória
        if (victoryCanvas != null)
            victoryCanvas.SetActive(true);

        // Pausa o jogo
        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Cena1"); // primeira fase
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}

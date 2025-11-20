using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    [Header("Canvases")]
    public GameObject victoryCanvas;      // Tela de vitória
    public GameObject cutsceneCanvas;     // Cutscene

    [Header("Cutscene")]
    public Image cutsceneImage;           // Imagem da cutscene
    public TextMeshProUGUI cutsceneText;  // Texto da cutscene
    public Sprite[] cutsceneSprites;
    public float timePerImage = 4f;
    public AudioSource cutsceneMusic;

    [Header("Botões")]
    public Button restartButton;
    public Button menuButton;

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
    /// Chamar quando o player entrar na Caixa de Elyra
    /// </summary>
    public void TryTriggerVictory()
    {
        // Só continua se todos os monstros já foram coletados
        if (!CounterManager.Instance.AllSoulsCollected())
            return;

        StartCoroutine(VictoryFlow());
    }

    /// <summary>
    /// Sequência de vitória
    /// </summary>
    private IEnumerator VictoryFlow()
    {
        // 1: Ativa a cutscene primeiro
        if (cutsceneCanvas != null)
            cutsceneCanvas.SetActive(true);

        if (cutsceneMusic != null)
            cutsceneMusic.Play();

        for (int i = 0; i < cutsceneSprites.Length; i++)
        {
            if (cutsceneImage != null)
                cutsceneImage.sprite = cutsceneSprites[i];

            if (cutsceneText != null)
                cutsceneText.text = "Cena " + (i + 1);

            yield return new WaitForSecondsRealtime(timePerImage); // usa real time para ignorar timeScale
        }

        // 2: Ativa a tela de vitória
        if (victoryCanvas != null)
            victoryCanvas.SetActive(true);

        // Pausa o jogo
        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        // Despausa o jogo antes de reiniciar
        Time.timeScale = 1f;
        SceneManager.LoadScene("Cena1"); // primeira fase
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // colocar o nome da cena do menu
    }
}

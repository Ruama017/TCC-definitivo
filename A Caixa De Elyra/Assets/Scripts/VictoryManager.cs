using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    [Header("Canvases")]
    public GameObject victoryCanvas;
    public GameObject cutsceneCanvas;

    [Header("Cutscene")]
    public Image cutsceneImage;
    public TextMeshProUGUI cutsceneText;
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

    public void TriggerVictorySequence()
    {
        StartCoroutine(VictoryFlow());
    }

    private IEnumerator VictoryFlow()
    {
        // 1. Ativa cutscene
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

            yield return new WaitForSecondsRealtime(timePerImage);
        }

        // 2. Ativa tela de vitória
        if (cutsceneCanvas != null)
            cutsceneCanvas.SetActive(false);

        if (victoryCanvas != null)
            victoryCanvas.SetActive(true);

        Time.timeScale = 0f; // pausa o jogo
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Cena1");
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}

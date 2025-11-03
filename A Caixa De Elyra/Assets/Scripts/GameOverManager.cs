using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button continueButton;
    public Button menuButton;
    public PlayerHealth playerHealth;

    [Header("Áudio de Game Over")]
    public AudioSource gameOverMusic;   // arraste a trilha de Game Over aqui
    public float fadeDuration = 1.0f;   // duração do fade-in em segundos

    private bool isGameOverTriggered = false;

    void Start()
    {
        gameOverPanel.SetActive(false);
        continueButton.onClick.AddListener(RestartLevel);
        menuButton.onClick.AddListener(GoToMenu);

        if (gameOverMusic != null)
            gameOverMusic.volume = 0f; // começa silenciado
    }

    void Update()
    {
        if (playerHealth.currentHealth <= 0 && playerHealth.currentCrystals == 0 && !isGameOverTriggered)
        {
            isGameOverTriggered = true;
            StartCoroutine(GameOverDelay());
        }
    }

    IEnumerator GameOverDelay()
    {
        // Espera 1.2 segundos para deixar a animação de morte tocar
        yield return new WaitForSeconds(1.2f);

        GameOver();
    }

    void GameOver()
    {
        if (!gameOverPanel.activeSelf)
        {
            // Reseta os cristais do player
            playerHealth.currentCrystals = 0;
            playerHealth.CollectCrystal(0); // atualiza a UI

            gameOverPanel.SetActive(true);

            // Congela a cena
            Time.timeScale = 0f;

            // Toca a música de Game Over com fade
            if (gameOverMusic != null)
            {
                gameOverMusic.Play();
                StartCoroutine(FadeInMusic(gameOverMusic, fadeDuration));
            }
        }
    }

    IEnumerator FadeInMusic(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime; // usar unscaledDeltaTime porque Time.timeScale = 0
            audioSource.volume = Mathf.Lerp(0f, 1f, currentTime / duration);
            yield return null;
        }
        audioSource.volume = 1f; // garante volume máximo no final
    }

    public void ShowGameOver()
    {
        GameOver();
    }

    void RestartLevel()
    {
        // Retoma o tempo antes de reiniciar
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoToMenu()
    {
        // Retoma o tempo antes de ir para o menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}

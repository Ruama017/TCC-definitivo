using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button continueButton;
    public Button menuButton;
    public PlayerHealth playerHealth;

    private AudioSource audioSource;

    void Start()
    {
        gameOverPanel.SetActive(false);
        continueButton.onClick.AddListener(RestartLevel);
        menuButton.onClick.AddListener(GoToMenu);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerHealth.currentHealth <= 0 && playerHealth.currentCrystals == 0)
        {
            GameOver();
        }
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

            if(audioSource != null)
                audioSource.Play();
        }
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

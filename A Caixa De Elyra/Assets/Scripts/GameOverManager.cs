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

        audioSource = GetComponent<AudioSource>(); // pega o AudioSource
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
            if(audioSource != null)
                audioSource.Play(); // toca o som de game over
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoToMenu()
    {
        SceneManager.LoadScene("Menu"); // ou o nome da sua cena de menu
    }
}

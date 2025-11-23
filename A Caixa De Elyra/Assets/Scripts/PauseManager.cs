using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;       // painel opcional do menu de pause
    public PlayerHealth playerHealth;  // arraste aqui o script do player que tem a flag isDead
    private bool isPaused = false;

    public void TogglePause()
    {
        // verifica se o player está morto
        if (playerHealth != null && playerHealth.isDead)
            return; // não faz nada se o player morreu

        // inverte o estado
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // pausa
            if (pauseMenu != null)
                pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // despausa
            if (pauseMenu != null)
                pauseMenu.SetActive(false);
        }
    }
}

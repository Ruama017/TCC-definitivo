using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // painel opcional do menu de pause
    private bool isPaused = false;

    public void TogglePause()
    {
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

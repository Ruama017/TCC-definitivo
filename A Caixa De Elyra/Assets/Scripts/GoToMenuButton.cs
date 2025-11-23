using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenuButton : MonoBehaviour
{
    public string menuSceneName = "MainMenu"; // nome da cena do menu

    // Chame este método no OnClick do botão
    public void GoToMenu()
    {
        Time.timeScale = 1f; // garante que o jogo não fique pausado
        SceneManager.LoadScene(menuSceneName);
    }
}

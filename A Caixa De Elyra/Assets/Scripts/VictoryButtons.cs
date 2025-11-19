using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryButtons : MonoBehaviour
{
    // REINICIAR → volta para a primeira fase (Cena1)
    public void RestartGame()
    {
        Time.timeScale = 1f; // DESPAUSA o jogo
        SceneManager.LoadScene("Cena1");
    }

    // MENU → vai para o menu principal. Coloque o nome da sua cena de menu aqui!
    public void GoToMenu()
    {
        Time.timeScale = 1f; // DESPAUSA o jogo
        SceneManager.LoadScene("Menu");
    }

    // SAIR (opcional)
    public void QuitGame()
    {
        Application.Quit();
    }
}

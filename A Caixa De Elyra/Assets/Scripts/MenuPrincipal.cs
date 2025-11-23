using UnityEngine;
using UnityEngine.SceneManagement; // carregar cenas

public class MenuPrincipal : MonoBehaviour
{
    // Carregar cena de jogo (Cutscene ou outra)
    public void Jogar()
    {
        SceneManager.LoadScene("Cutscene");
    }

    // Sair do jogo
    public void Sair()
    {
        Debug.Log("Você Saiu"); // apenas para testar no Editor
        Application.Quit();      // encerra o jogo na build
    }

    // Ir para o menu principal (caso precise chamar de outro botão)
    public void VoltarParaMenu(string menuSceneName)
    {
        SceneManager.LoadScene(menuSceneName);
    }
}

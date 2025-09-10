using UnityEngine;
using UnityEngine.SceneManagement; //carregar cenas

public class MenuPrincipal : MonoBehaviour
{
    // carregar cena 1
    public void Jogar()
    {
        SceneManager.LoadScene("Cutscene");
    }

    // sair 
    public void Sair()
    {
        Debug.Log("Você Saiu");
        Application.Quit();
    }
}

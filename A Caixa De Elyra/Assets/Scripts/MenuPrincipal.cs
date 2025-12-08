using UnityEngine;
using UnityEngine.SceneManagement;


//  INTERFACE DO COMMAND

public interface ICommand
{
    void Execute();
}


//  COMMANDS CONCRETOS


// Command para iniciar o jogo
public class StartGameCommand : ICommand
{
    private string sceneName;
    public StartGameCommand(string sceneName) { this.sceneName = sceneName; }
    public void Execute() { SceneManager.LoadScene(sceneName); }
}

// Command para sair do jogo
public class ExitGameCommand : ICommand
{
    public void Execute()
    {
        Debug.Log("Você Saiu");
        Application.Quit();
    }
}

// Command para voltar para outro menu
public class GoToMenuCommand : ICommand
{
    private string menuSceneName;
    public GoToMenuCommand(string menuSceneName) { this.menuSceneName = menuSceneName; }
    public void Execute() { SceneManager.LoadScene(menuSceneName); }
}


//  MENU PRINCIPAL

public class MenuPrincipal : MonoBehaviour
{
    private ICommand jogarCommand;
    private ICommand sairCommand;

    private void Start()
    {
        // Inicializa os Commands
        jogarCommand = new StartGameCommand("Cutscene");
        sairCommand = new ExitGameCommand();
    }

    // Botão Jogar
    public void Jogar() => jogarCommand.Execute();

    // Botão Sair
    public void Sair() => sairCommand.Execute();

    // Botão Voltar (pode passar o nome da cena do menu)
    public void VoltarParaMenu(string menuSceneName)
    {
        ICommand goToMenu = new GoToMenuCommand(menuSceneName);
        goToMenu.Execute();
    }
}

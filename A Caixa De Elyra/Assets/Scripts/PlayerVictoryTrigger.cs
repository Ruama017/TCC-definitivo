using UnityEngine;

public class PlayerVictoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se quem entrou no trigger é o Player
        if (!collision.CompareTag("Player"))
            return;

        // Confere se este objeto é a própria Caixa de Elyra
        if (!gameObject.CompareTag("CaixaElyra"))
            return;

        // Segurança: garantir que existe VictoryManager
        if (VictoryManager.instance == null)
        {
            Debug.LogError("[PlayerVictoryTrigger] VictoryManager.instance está NULL!");
            return;
        }

        // Precisa verificar se o Thorne já morreu
        if (!VictoryManager.instance.ThorneIsDead)
        {
            Debug.Log("[PlayerVictoryTrigger] Thorne ainda vivo. Caixa bloqueada.");
            return;
        }

        // Se chegou aqui, está tudo ok → dispara a vitória
        Debug.Log("[PlayerVictoryTrigger] Caixa ativada! Disparando cutscene e vitória.");
        VictoryManager.instance.TriggerVictorySequence();
    }
}

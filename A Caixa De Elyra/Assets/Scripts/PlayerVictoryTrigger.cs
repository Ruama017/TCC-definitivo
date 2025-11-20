using UnityEngine;

public class PlayerVictoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se quem entrou no trigger é o Player
        if (collision.CompareTag("Player"))
        {
            // Verifica se é a Caixa de Elyra
            if (gameObject.CompareTag("CaixaElyra"))
            {
                // Tenta disparar a sequência de vitória
                if (VictoryManager.instance != null)
                {
                    VictoryManager.instance.TryTriggerVictory();
                }
                else
                {
                    Debug.LogError("[PlayerVictoryTrigger] VictoryManager.instance está NULL!");
                }
            }
        }
    }
}

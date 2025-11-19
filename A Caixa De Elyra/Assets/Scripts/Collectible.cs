using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum Type { ShieldFlower, LifeCrystal, SuperPower }
    public Type collectibleType;

    public int amount = 1; // se precisar (ex: +1 flor)
    public float superDuration = 6f; // duração do super, só usado se for SuperPower

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // chama o script do player para atualizar o inventário
        PlayerInventory inventory = collision.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.Collect(this);
        }

        // ativa o super se for do tipo SuperPower
        if (collectibleType == Type.SuperPower)
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.ActivateAttackSuper(superDuration); // ativa o super do ataque
            }
        }

        // destrói o coletável
        Destroy(gameObject);
    }
}

using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum Type { ShieldFlower, LifeCrystal, SuperPower }
    public Type collectibleType;

    public int amount = 1; // se precisar (ex: +1 flor)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // chama o script do player
            collision.GetComponent<PlayerInventory>().Collect(this);

            // DESTROI o coletável
            Destroy(gameObject);
        }
    }
}

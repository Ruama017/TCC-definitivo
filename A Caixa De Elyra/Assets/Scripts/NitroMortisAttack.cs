using UnityEngine;

public class NitroMortisAttack : MonoBehaviour
{
    public int baseDamage = 2;
    public float poisonDuration = 4f;
    public float poisonTick = 1f;
    public float poisonInterval = 1f;

    public void ApplyDamageAndPoison(GameObject player)
    {
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(baseDamage);
            ph.ApplyPoison(poisonDuration, poisonTick, poisonInterval);
        }
    }
}

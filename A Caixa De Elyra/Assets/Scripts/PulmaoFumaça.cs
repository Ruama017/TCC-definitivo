using UnityEngine;

public class PulmaoFumaça : MonoBehaviour
{
  [Header("Dano")]
  public int damage =1;
  public float damageInterval=1f;
  private float damageTimer=0f;

  private void OnTriggerStay2D(Collider2D other){
    PlayerHealth player = other.GetComponent<PlayerHealth>();
    if(player!=null){
        damageTimer +=Time.deltaTime;

        if(damageTimer >=damageInterval){
            player.TakeDamage(damage);
            damageTimer =0f;

        }
    }
  }

  private void OnTriggerExit2D(Collider2D other){
    PlayerHealth player = other.GetComponent<PlayerHealth>();
    if(player != null){
        damageTimer=0f;
    }
  }
    
}

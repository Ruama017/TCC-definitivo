using UnityEngine;

public class MonsterHintTrigger : MonoBehaviour
{
    public GameObject hintPanel;
    public float displayTime=3f;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            if(hintPanel !=null){
                hintPanel.SetActive(true);
                Invoke(nameof(HidePanel),displayTime);
            }
        }
    }

    void HidePanel(){
        if(hintPanel !=null)
        hintPanel.SetActive(false);
    }
}

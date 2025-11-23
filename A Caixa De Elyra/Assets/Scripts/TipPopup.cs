using UnityEngine;
using System.Collections;

public class TipPopup : MonoBehaviour
{
    public GameObject tipImage;  // arraste sua imagem aqui
    public float duration = 3f;  // tempo que a dica ficará visível

    public void ShowTip()
    {
        StartCoroutine(TipRoutine());
    }

    IEnumerator TipRoutine()
    {
        tipImage.SetActive(true);
        yield return new WaitForSeconds(duration);
        tipImage.SetActive(false);
    }
}

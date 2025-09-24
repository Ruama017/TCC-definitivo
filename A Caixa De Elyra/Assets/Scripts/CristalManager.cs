using UnityEngine;
using TMPro;

public class CristalManager : MonoBehaviour
{
    public int cristaisColetados = 0;
    public TextMeshProUGUI cristalText;

    void Start()
    {
        cristalText.text = "Cristais: 0";
    }

    public void ColetarCristal()
    {
        cristaisColetados++;
        cristalText.text = "Cristais: " + cristaisColetados;
    }
}

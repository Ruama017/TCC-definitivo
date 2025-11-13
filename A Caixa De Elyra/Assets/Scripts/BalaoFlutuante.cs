using UnityEngine;

public class BalaoFlutuante : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float velocidade = 2f;
    private Vector3 posInicial;

    void Start()
    {
        posInicial = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = posInicial + Vector3.up * Mathf.Sin(Time.time * velocidade) * amplitude;
    }
}

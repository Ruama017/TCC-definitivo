using UnityEngine;

public class BridgeBreak : MonoBehaviour
{
    [Header("Configurações")]
    public float fallGravity = 2f;       // Quão rápido os pedaços caem
    public float destroyDelay = 2f;      // Tempo até os pedaços sumirem
    public string playerTag = "Player";  // Tag do Player
    private bool bridgeBroken = false;

    private Rigidbody2D[] bridgePieces;

    void Start()
    {
        // Pega todos os Rigidbody2D dos pedaços da ponte (assumindo que cada pedaço tem Rigidbody2D)
        bridgePieces = GetComponentsInChildren<Rigidbody2D>();

        // Inicialmente, a ponte não cai
        foreach (var piece in bridgePieces)
        {
            piece.isKinematic = true; // impede que caia
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se o Player pisou e a ponte ainda não quebrou
        if (!bridgeBroken && other.CompareTag(playerTag))
        {
            BreakBridge();
        }
    }

    void BreakBridge()
    {
        bridgeBroken = true;

        foreach (var piece in bridgePieces)
        {
            piece.isKinematic = false;        // deixa cair
            piece.gravityScale = fallGravity; // controla a velocidade da queda
        }

        // Destrói os pedaços depois de um tempo
        foreach (var piece in bridgePieces)
        {
            Destroy(piece.gameObject, destroyDelay);
        }
    }
}
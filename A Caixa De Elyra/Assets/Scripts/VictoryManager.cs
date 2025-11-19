using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    [Header("Canvases")]
    public GameObject victoryCanvas;
    public GameObject cutsceneCanvas;

    [Header("Cutscene")]
    public Image cutsceneImage;              // IMAGEM NORMAL
    public TextMeshProUGUI cutsceneText;     // **AGORA É TMP CORRETO**
    public Sprite[] cutsceneSprites;
    public float timePerImage = 4f;
    public AudioSource cutsceneMusic;

    private void Awake()
    {
        instance = this;
    }

    public void PlayVictorySequence()
    {
        StartCoroutine(VictoryFlow());
    }

    IEnumerator VictoryFlow()
    {
        // 1: Ativa a tela de vitória
        victoryCanvas.SetActive(true);

        // Espera 10 segundos
        yield return new WaitForSeconds(10f);

        // Some com a tela de vitória
        victoryCanvas.SetActive(false);

        // 2: Inicia a cutscene
        StartCoroutine(CutsceneFlow());
    }

    IEnumerator CutsceneFlow()
    {
        cutsceneCanvas.SetActive(true);
        cutsceneMusic.Play();

        for (int i = 0; i < cutsceneSprites.Length; i++)
        {
            cutsceneImage.sprite = cutsceneSprites[i];
            cutsceneText.text = "Texto da cena " + (i + 1);

            yield return new WaitForSeconds(timePerImage);
        }
    }
}

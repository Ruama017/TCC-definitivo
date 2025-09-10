using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuConfig : MonoBehaviour
{
    public GameObject painelConfig; // painel de config.
    public Slider sliderVolume; //volume
    public AudioSource musicaFundo; //musica do jogo

    void Start(){
        //garante que o volume inicial do slider corresponde ao volume do audio
        if(musicaFundo !=null && sliderVolume != null)
        sliderVolume.value = musicaFundo.volume;
    }
    //abrir painel
    public void AbrirConfig(){
        painelConfig.SetActive(true);
    }
    //fechar painel
    public void FecharConfig(){
        painelConfig.SetActive(false);
    }
    //ajustar volume
    public void AlterarVolume(float valor){
        if(musicaFundo != null)
        musicaFundo.volume = valor;
    }


    
}

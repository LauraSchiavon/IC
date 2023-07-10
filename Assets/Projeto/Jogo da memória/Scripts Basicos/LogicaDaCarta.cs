using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaDaCarta : MonoBehaviour
{
    CartaData cartaData;
    string cartaNome;
    Sprite cartaImagem;
    JogoDaMemoria tabuleiro;
    bool selecionada;
    


    void Start()
    {
    cartaNome = cartaData.cartaNome;
    cartaImagem = cartaData.cartaImagem;
    
    tabuleiro = GameObject.Find("Tabuleiro").GetComponent<JogoDaMemoria>();
    }

    void Update()
    {
        
    }
}

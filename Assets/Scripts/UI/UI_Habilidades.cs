using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Habilidades : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Button escudo, saltoDoble, vidaExtra, paracaidas;
    [SerializeField] private TextMeshProUGUI txEscudo, txSaltoDoble, txVidaExtra, txParacaidas, txCoins;
    [SerializeField]private int valorEscudo, valorSaltoDoble, valorVidaExtra, valorParacaidas;
   
    private PlayerController playerController;
    [SerializeField] LevelManager levelManager;


    private void Awake()
    {
       playerController = FindAnyObjectByType<PlayerController>();
        txEscudo.text = valorEscudo.ToString();
        txSaltoDoble.text = valorSaltoDoble.ToString();
        txVidaExtra.text = valorVidaExtra.ToString();
        txParacaidas.text = valorParacaidas.ToString();
        txCoins.text = playerController.coinsAmount.ToString();

    }

    private void Start()
    {

    }
    public void CheckAvailable()
    {
      //  if (valorEscudo >= playerController.coinsAmount) escudo.IsInteractable();
      //  if (valorSaltoDoble >= playerController.coinsAmount) saltoDoble.IsInteractable();
       // if (valorVidaExtra >= playerController.coinsAmount) vidaExtra.IsInteractable();
       // if (valorParacaidas >= playerController.coinsAmount) paracaidas.IsInteractable();
    }

    public void BuyEscudo()
    {
        playerController.TakeUICoin(-valorEscudo);
        playerController.coinsAmount -= valorEscudo;
        playerController.escudo = true;
        levelManager.ResetLevel();
    }
    public void BuySaltoDoble()
    {
        playerController.TakeUICoin(-valorSaltoDoble);
        playerController.coinsAmount -= valorSaltoDoble;
        playerController.saltoDoble = true;
        levelManager.ResetLevel();
    }
    public void BuyVidaExtra()
    {
        playerController.TakeUICoin(-valorVidaExtra);
        playerController.coinsAmount -= valorVidaExtra;
        playerController.vidaExtra = true;
        levelManager.ResetLevel();
    }
    public void BuyParacaidas()
    {
        playerController.TakeUICoin(-valorParacaidas);
        playerController.coinsAmount -= valorParacaidas;
        playerController.paracaidas = true;
        levelManager.ResetLevel();
    }
}

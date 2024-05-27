using TMPro;
using UnityEngine;

public class UI_Habilidades : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Button escudo, saltoDoble, vidaExtra, paracaidas;
    [SerializeField] private TextMeshProUGUI txEscudo, txSaltoDoble, txVidaExtra, txParacaidas, txCoins;
    [SerializeField] private int valorEscudo, valorSaltoDoble, valorVidaExtra, valorParacaidas;
   
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UI_Coins uiCoins;
    [SerializeField] LevelManager levelManager;


    private void Awake()
    {
      
       txEscudo.text = valorEscudo.ToString();
       txSaltoDoble.text = valorSaltoDoble.ToString();
       txVidaExtra.text = valorVidaExtra.ToString();
       txParacaidas.text = valorParacaidas.ToString();
       txCoins.text = uiCoins.coinCount.ToString();

    }

    private void Start()
    {

    }
    public void CheckAvailable()
    {
        if (valorEscudo <= uiCoins.coinCount) escudo.GetComponent<UnityEngine.UI.Button>().interactable = true;
        if (valorSaltoDoble <= uiCoins.coinCount) saltoDoble.GetComponent<UnityEngine.UI.Button>().interactable = true;
        if (valorVidaExtra <= uiCoins.coinCount) vidaExtra.GetComponent<UnityEngine.UI.Button>().interactable = true;
        if (valorParacaidas <= uiCoins.coinCount) paracaidas.GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    public void BuyEscudo()
    {
        uiCoins.UpdateCoins(-valorEscudo);
        playerController.escudo = true;
        levelManager.ResetLevel();
    }
    public void BuySaltoDoble()
    {
        uiCoins.UpdateCoins(-valorSaltoDoble);
        playerController.saltoDoble = true;
        levelManager.ResetLevel();
    }
    public void BuyVidaExtra()
    {
        uiCoins.UpdateCoins(-valorVidaExtra);
        playerController.vidaExtra = true;
        levelManager.ResetLevel();
    }
    public void BuyParacaidas()
    {
        uiCoins.UpdateCoins(-valorParacaidas);
        playerController.paracaidas = true;
        levelManager.ResetLevel();
    }
}

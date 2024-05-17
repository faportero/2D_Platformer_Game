
using TMPro;
using UnityEngine;

public class UI_Coins : MonoBehaviour
{
    
    private float maxCoins = 999;
    public float coinCount = 0;
    [SerializeField] private TextMeshProUGUI coinCountText;


    public void UpdateCoins()
    {
        coinCount += 1;
        coinCount = Mathf.Clamp(coinCount, 0f, maxCoins);
        coinCountText.text = coinCount.ToString();
    }
}

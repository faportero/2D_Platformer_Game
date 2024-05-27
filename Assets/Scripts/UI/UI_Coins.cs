
using TMPro;
using UnityEngine;

public class UI_Coins : MonoBehaviour
{   
    private int maxCoins = 9999999;
    public float coinCount = 0;
    public TextMeshProUGUI coinCountText;

    public void UpdateCoins(int value)
    {
        coinCount += value;
        coinCount = Mathf.Clamp(coinCount, 0f, maxCoins);
        coinCountText.text = coinCount.ToString();
    }

}


using TMPro;
using UnityEngine;

public class UI_Coins : MonoBehaviour
{
    
    private float maxCoins = 999;
    public float coinCount = 0;
    [SerializeField] private TextMeshProUGUI coinCountText;

    private void Start()
    {
        coinCountText.text = UserData.coins.ToString();
        
    }

    public void UpdateCoins(int value)
    {
        coinCount += value;
        coinCount = Mathf.Clamp(coinCount, 0f, maxCoins);
        coinCountText.text = coinCount.ToString();
    }

}

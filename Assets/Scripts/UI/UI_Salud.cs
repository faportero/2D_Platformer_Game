
using TMPro;
using UnityEngine;

public class UI_Salud : MonoBehaviour
{
    
    private float maxSalud = 3;
    public float saludCount = 0;
    [HideInInspector] public TextMeshProUGUI coinCountText;


    public void UpdateSalud(int amount)
    {
        saludCount += amount;
        saludCount = Mathf.Clamp(saludCount, 0f, maxSalud);
        coinCountText.text = saludCount.ToString();
    }
}

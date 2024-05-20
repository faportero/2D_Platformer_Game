
using TMPro;
using UnityEngine;

public class UI_Salud : MonoBehaviour
{
    
    private float maxSalud = 4;
    public float saludCount = 0;
    [HideInInspector] public TextMeshProUGUI coinCountText;


    public void UpdateSalud()
    {
        saludCount += 1;
        saludCount = Mathf.Clamp(saludCount, 0f, maxSalud);
        coinCountText.text = saludCount.ToString();
    }
}

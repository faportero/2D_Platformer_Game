using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarruselNivel : MonoBehaviour
{
    [SerializeField] private int distance = 2045;
    [SerializeField] private int val = 1;
    [SerializeField] private Sprite[] activeSprite, desactiveSprite;
    RectTransform rectTransform;
    Vector2 _valuePos;
    int nivel;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        _valuePos = new Vector2(0, 0);
        nivel = ControlDatos._nivel;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < nivel + 1)
                transform.GetChild(i).GetComponent<Image>().sprite = activeSprite[i];
            else
                transform.GetChild(i).GetComponent<Image>().sprite = desactiveSprite[i];
        }
        for (int i = 2; i < 6; i++)
        {
            if (i <= nivel + 1)
            {
                transform.GetChild(i - 1).GetComponent<Button>().interactable = true;
            }
            else
            {
                transform.GetChild(i - 1).GetComponent<Button>().interactable = false;
            }
        }
    }
    private void Update()
    {
        if (rectTransform.anchoredPosition.x != _valuePos.x)
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, _valuePos, .25f);
    }
    public void MoverCarruselIzqDer(bool derecha)
    {
        if (val == 1 && !derecha)
            val = 5;
        else if (val == 5 && derecha)
            val = 1;
        else if (derecha)
            val++;
        else
            val--;
        if (val == 1)
            _valuePos = new Vector2(0, 0);
        if (val == 2)
            _valuePos = new Vector2(-distance, 0);
        if (val == 3)
            _valuePos = new Vector2(-distance * 2, 0);
        if (val == 4)
            _valuePos = new Vector2(-distance * 3, 0);
        if (val == 5)
            _valuePos = new Vector2(-distance * 4, 0);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == val - 1)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}

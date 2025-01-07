using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EnemyNew : MonoBehaviour
{
    public GameObject effectPanel;
    private Animator effectPanelAnimator;
    public GameObject effectPanelMensaje;
    private TextMeshProUGUI effectText;
    private TextMeshProUGUI effectTextMensajeTitulo;
    private TextMeshProUGUI effectTextMensajeDesc;
    private Image dieEffectImage;

    public bool isAdict = false;
    private List<EnemyNew> e;
    [SerializeField] private List <Sprite> spriteRenderers = new List<Sprite>();
    private PlayerControllerNew PlayerControllerNew;
    private PlayerMovementNew playerMovement;
    [SerializeField] private bool isRandom;
    private UI_SaludBar saludBar;

    [HideInInspector] public string currentEffectTag = "CurrentEffect";
    [HideInInspector] public string dieEffectTag = "DieEffect";

    [SerializeField] private List <Sprite> effectPanelSprites = new List<Sprite>();
    private Image effectPanelImage;

    [SerializeField] Material[] flameMaterials;
    Transform square;
    public enum SustanceType
    {
        Cannabis,
        Cocaina,
        Extasis,
        Metanfetamina,
        Heroina,
        Psilocibina,
        Alcohol,
        Tabaco
    }
    public SustanceType sustanceType;



    private void Awake()
    {
        //square = transform.Find("Square");
        //AssignSprite();
        PlayerControllerNew = FindAnyObjectByType<PlayerControllerNew>();

        // Encuentra todos los objetos en la escena, incluyendo los inactivos
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        // Inicializa los objetos con los tags específicos
        effectPanel = FindObjectWithTag(allObjects, currentEffectTag);
        effectPanelMensaje = FindObjectWithTag(allObjects, dieEffectTag);   

    GameObject FindObjectWithTag(GameObject[] allObjects, string tag)
    {
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }
        return null;
    }

}
    private void OnValidate()
    {
 
       AssignSprite();     

    }
    private void Start()
    {

        e = new List<EnemyNew>();
        effectText = effectPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        effectTextMensajeTitulo = effectPanelMensaje.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        effectTextMensajeDesc = effectPanelMensaje.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        dieEffectImage = effectPanelMensaje.transform.GetChild(2).GetComponent<Image>();

        saludBar = transform.GetChild(0).GetComponent<UI_SaludBar>();

        effectPanelAnimator = effectPanel.GetComponent<Animator>();

        effectPanelImage = effectPanel.transform.GetChild(0).GetComponent<Image>();
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision !=null && collision.tag == ("Player"))
    //    {
    //        if (e.Count == 0) e = collision.GetComponent<PlayerControllerNew>().enemies;
    //        PlayerControllerNew = collision.GetComponent<PlayerControllerNew>();
    //        playerMovement = collision.GetComponent<PlayerMovementNew>();


    //        isAdict = true;
    //        foreach (EnemyNew enemy in e)
    //        {
    //            if (enemy.sustanceType == sustanceType && !PlayerControllerNew.isAttack)
    //            {
    //                if (!playerMovement.doingSmash && !PlayerControllerNew.escudo)
    //                {
    //                    if (playerMovement.canMove) PlayerControllerNew.StartBlinking();
    //                    collision.GetComponent<PlayerControllerNew>().TakeAdiccion(enemy);
    //                    collision.GetComponent<PlayerControllerNew>().SaludAmount = 0;
    //                    collision.GetComponent<PlayerControllerNew>().uiSalud.saludCount = 0;
    //                    collision.GetComponent<PlayerMovementNew>().canSmash = false;
    //                    collision.GetComponent<PlayerControllerNew>().uiSalud.UpdateSalud(0);
    //                    collision.GetComponent<PlayerControllerNew>().LoseLife();
    //                    Effect();
    //                    EnemyDie();
    //                }
    //                else
    //                {
    //                  //  EnemyDie();
    //                }
    //            }
    //        }

    //    }

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision != null && collision.tag == ("Player"))
        //{
        //    Effect();
        //}
    }


    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    isAdict = false;
    //}
    public void EnemyDie()
    {
        //gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //gameObject.SetActive(false);
        isAdict = false;
    }
        
    public void Effect()
    {

        //sustanceType = SustanceType.Cannabis;
        switch (sustanceType)
        {
            case SustanceType.Cannabis:
                //print("cannabisssss");
                //transform.GetChild(0).GetComponent<SpriteRenderer>().material = flameMaterials[0];
                PlayerControllerNew.isPsilo = true;
                
                //PlayerControllerNew.isCannabis = true;


               // effectPanel.GetComponent<Image>().color = new Color(0, 1, 0, .25f);
               // effectText.text = "Cannabis";
                effectTextMensajeTitulo.text = "Cannabis";
                effectTextMensajeDesc.text = "Las drogas distorsionan la realidad";
                dieEffectImage.sprite = spriteRenderers[0];
                
                effectPanelImage.sprite = effectPanelSprites[0];
                break;

            case SustanceType.Cocaina:
               // transform.GetChild(0).GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //print("Cocaa");
                PlayerControllerNew.isPsilo = true;
                
                //PlayerControllerNew.isCocaMetaHero = true;

                //effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
               // effectText.text = "Cocaina";
                effectTextMensajeTitulo.text = "Cocaina";
                effectTextMensajeDesc.text = "Las drogas distorsionan la realidad";
                dieEffectImage.sprite = spriteRenderers[1];

                effectPanelImage.sprite = effectPanelSprites[1];
                break;
            case SustanceType.Extasis:
              //  transform.GetChild(0).GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //print("Exta");
                PlayerControllerNew.isPsilo = true;
                
                //PlayerControllerNew.isCocaMetaHero = true;

                //effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
               // effectText.text = "Éxtasis";
                effectTextMensajeTitulo.text = "Extasis";
                effectTextMensajeDesc.text = "Las drogas distorsionan la realidad";
                dieEffectImage.sprite = spriteRenderers[2];
                
                effectPanelImage.sprite = effectPanelSprites[2];
                break;
            case SustanceType.Metanfetamina:
             //   transform.GetChild(0).GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //print("Metanfetamina");
                PlayerControllerNew.isPsilo = true;

                //PlayerControllerNew.isCocaMetaHero = true;
                //effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                //effectText.text = "Metanfetamina";
                effectTextMensajeTitulo.text = "Metanfetamina";
                effectTextMensajeDesc.text = "Las drogas distorsionan la realidad";
                dieEffectImage.sprite = spriteRenderers[3];
                
                effectPanelImage.sprite = effectPanelSprites[3];
                break;
            case SustanceType.Heroina:
              //  transform.GetChild(0).GetComponent<SpriteRenderer>().material = flameMaterials[0];
                // print("Heroinaaaa");
                PlayerControllerNew.isPsilo = true;

               // PlayerControllerNew.isCocaMetaHero = true;
                //effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                //effectText.text = "Heroina";
                effectTextMensajeTitulo.text = "Heroina";
                effectTextMensajeDesc.text = "Las drogas distorsionan la realidad";
                dieEffectImage.sprite = spriteRenderers[4];
                
                effectPanelImage.sprite = effectPanelSprites[4];
                break;

            case SustanceType.Psilocibina:
             //   transform.GetChild(0).GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //print("Psilocibinaaaa");
                PlayerControllerNew.isPsilo = true;
                //effectPanel.GetComponent<Image>().color = new Color(.5f, 0, .75f, .25f);
               // effectText.text = "Psilocibina";
                effectTextMensajeTitulo.text = "Psilocibina";
                effectTextMensajeDesc.text = "Las drogas distorsionan la realidad";
                dieEffectImage.sprite = spriteRenderers[5];
                
                effectPanelImage.sprite = effectPanelSprites[5];
                break;

            case SustanceType.Alcohol:
              //  transform.GetChild(0).GetComponent<SpriteRenderer>().material = flameMaterials[1];
                //print("Alcoholllll");
                PlayerControllerNew.isCannabis = true;
                //PlayerControllerNew.isAlcohol = true;
                //effectPanel.GetComponent<Image>().color = new Color(1, 1, 0, .25f);
                //effectText.text = "Alcohol";
                effectTextMensajeTitulo.text = "Alcohol";
                effectTextMensajeDesc.text = "El alcohol te vuelve torpe";
                dieEffectImage.sprite = spriteRenderers[6];
                
                effectPanelImage.sprite = effectPanelSprites[6];
                break;
            case SustanceType.Tabaco:
             //   transform.GetChild(0).GetComponent<SpriteRenderer>().material = flameMaterials[2];
                //print("Tabacooooo");
                PlayerControllerNew.isTabaco = true;
                //effectPanel.GetComponent<Image>().color = new Color(0, 0, 1, .25f);
                //effectPanelAnimator.SetBool("Smoke", true);

                //effectText.text = "Tabaco";
                effectTextMensajeTitulo.text = "Tabaco";
                effectTextMensajeDesc.text = "El tabaco te hace toser fuerte";
                dieEffectImage.sprite = spriteRenderers[7];
                
                effectPanelImage.sprite = effectPanelSprites[7];
                break;

        }
    }

    private void AssignSprite()
    {
        switch (sustanceType)
        {
            case SustanceType.Cannabis:

                if(gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null && flameMaterials.Length > 0) gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //square.gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[0];
                break;

            case SustanceType.Cocaina:
                if (gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null && flameMaterials.Length > 0) gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //square.gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[1];

                break;
            case SustanceType.Extasis:
                if (gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null && flameMaterials.Length > 0) gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //square.gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[2];

                break;
            case SustanceType.Metanfetamina:
                if (gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null && flameMaterials.Length > 0) gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //square.gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[3];

                break;
            case SustanceType.Heroina:
                if (gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null && flameMaterials.Length > 0) gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //square.gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[4];

                break;

            case SustanceType.Psilocibina:
                if (gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null && flameMaterials.Length > 0) gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                //square.gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[0];
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[5];

                break;

            case SustanceType.Alcohol:
                if (gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null && flameMaterials.Length > 0) gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[1];
                //square.gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[1];
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[6];
                break;
            case SustanceType.Tabaco:
                if (gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null && flameMaterials.Length > 0) gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[2];
                //square.gameObject.GetComponent<SpriteRenderer>().material = flameMaterials[2];
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[7];
                break;

        }
    }
}

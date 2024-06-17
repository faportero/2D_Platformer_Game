using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static Enemy;





#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class Health : MonoBehaviour
{
    public enum HealthType
    {
        Futbol,
        Gym,
        Trotar,
        No,
        SoloHoy,
        Meditacion,
        Agua,
        Manzana,
        Pescado
    }
    public HealthType healthType;
    [SerializeField] private bool isRandom;
    public List<Sprite> spriteRenderers = new List<Sprite>();
    [SerializeField]private SpriteRenderer spriteRenderer;
    [SerializeField]private Image panelSpriteRenderer;

    private Vector3 lastPosition;
    public bool isShowPanel;
    private Coroutine coroutineFeedback;

    [SerializeField] private GameObject panelFeedback;
    private PlayerController playerController;
    private List<Health> h;
    [SerializeField]private GameObject imagePanelFeedback;

   // [SerializeField] private UI_FeedbackSalud ui_feedback;

    [SerializeField] private List<Sprite> imageFeedbackPanel = new List<Sprite>();
    public int spriteIndex;
    private int randomIndex;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
   
    }
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        h = new List<Health>();
        //spriteIndex = randomIndex;
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        
        if (isRandom) 
            EditorApplication.update += OnEditorUpdate;
    }

    void OnDisable()
    {
        if (isRandom) 
            EditorApplication.update -= OnEditorUpdate;
    }

    void OnEditorUpdate()
    {
       
        if (!Application.isPlaying && transform.position != lastPosition)
        {
            UpdateSprite();
            
            lastPosition = transform.position;
        }
    }
#endif

    void OnValidate()
    {
        

       
       
        if (!isRandom) AssignSprite();
    }

    private void UpdateSprite()
    {
        if (spriteRenderers.Count == 0)
        {
            Debug.LogWarning("La lista de sprites está vacía.");
            return;
        }

        if (transform.position != Vector3.zero)
        {
          
             randomIndex = Random.Range(0, spriteRenderers.Count);
            
          
            spriteRenderer.sprite = spriteRenderers[randomIndex];
          
            // panelSpriteRenderer.sprite = spriteRenderers[randomIndex];
        }
        else
        {
            spriteRenderer.sprite = spriteRenderers[0];
            //panelSpriteRenderer.sprite = spriteRenderers[0];
        }
        

#if UNITY_EDITOR
        // Marcar el objeto como modificado para asegurarse de que los cambios se reflejen en el editor
        //spriteIndex = randomIndex;
        EditorUtility.SetDirty(spriteRenderer);
       
#endif
    }
    
    public void AssignSprite()
    {
        switch (healthType)
        {
            case HealthType.Futbol:               
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[0];
               spriteIndex = 0;
                break;
            case HealthType.Gym:               
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[1];
               spriteIndex = 1;
                break;
            case HealthType.Trotar:
             
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[2];
               spriteIndex = 2;
                break;
            case HealthType.No:               
               spriteIndex = 3;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[3];
                break;
            case HealthType.SoloHoy:                
               spriteIndex = 4;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[4];
                break;
            case HealthType.Meditacion:                
             spriteIndex = 5;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[5];
                break;
            case HealthType.Agua:               
            spriteIndex = 6;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[6];
                break;
            case HealthType.Manzana:               
              spriteIndex = 7;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[7];
                break;
            case HealthType.Pescado:               
              spriteIndex = 8;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[8];
                break;
        }
    }
    public void AssignPanelSprite()
    {
        switch (healthType)
        {
            case HealthType.Futbol:
                //  ui_feedback.AssignFeedbackSprite();
                panelSpriteRenderer.sprite = imageFeedbackPanel[0]; 

                //imagePanelFeedback.GetComponent<Image>().sprite = imageFeedbackPanel[0];
                break;
            case HealthType.Gym:
                //  ui_feedback.AssignFeedbackSprite();
                panelSpriteRenderer.sprite = imageFeedbackPanel[1];
                //imagePanelFeedback.GetComponent<Image>().sprite = imageFeedbackPanel[1];
                break;
            case HealthType.Trotar:
                //  ui_feedback.AssignFeedbackSprite(); panelSpriteRenderer.sprite = imageFeedbackPanel[0]; 
                panelSpriteRenderer.sprite = imageFeedbackPanel[2];
                //imagePanelFeedback.GetComponent<Image>().sprite = imageFeedbackPanel[2];
                break;
            case HealthType.No:
                //   ui_feedback.AssignFeedbackSprite();
                panelSpriteRenderer.sprite = imageFeedbackPanel[3];
                //imagePanelFeedback.GetComponent<Image>().sprite = imageFeedbackPanel[3];
                break;
            case HealthType.SoloHoy:
                //  ui_feedback.AssignFeedbackSprite();
                panelSpriteRenderer.sprite = imageFeedbackPanel[4];
               // imagePanelFeedback.GetComponent<Image>().sprite = imageFeedbackPanel[4];
                break;
            case HealthType.Meditacion:
                //    ui_feedback.AssignFeedbackSprite();
                panelSpriteRenderer.sprite = imageFeedbackPanel[5];
               // imagePanelFeedback.GetComponent<Image>().sprite = imageFeedbackPanel[5];
                break;
            case HealthType.Agua:
                //   ui_feedback.AssignFeedbackSprite();
                panelSpriteRenderer.sprite = imageFeedbackPanel[6];
               // imagePanelFeedback.GetComponent<Image>().sprite = imageFeedbackPanel[6];
                break;
            case HealthType.Manzana:
                //  ui_feedback.AssignFeedbackSprite();
                panelSpriteRenderer.sprite = imageFeedbackPanel[7];
                imagePanelFeedback.GetComponent<Image>().sprite = imageFeedbackPanel[7];
                break;
            case HealthType.Pescado:
                //   ui_feedback.AssignFeedbackSprite();
                panelSpriteRenderer.sprite = imageFeedbackPanel[8];
               // imagePanelFeedback.GetComponent<Image>().sprite = imageFeedbackPanel[8];
                break;
        }
    }

    public void HealthDie()
    {
        Destroy(gameObject, .2f);
       //gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag == ("Player"))
        {
           // ShowFeedback();
            HealthDie();
            // print("Spritee"+ spriteIndex);

            //    if (h.Count == 0) h = collision.GetComponent<PlayerController>().healhts;
            //    playerController = collision.GetComponent<PlayerController>();
            //    foreach (Health health in h)
            //    {
            //        if (health.healthType == healthType)
            //        {
            //            // ui_feedback.image.sprite
            //            // collision.GetComponent<PlayerController>().TakeHealth(health);
            //            // AssignPanelSprite();
            //            // HealthDie();
            //            // ShowFeedback();
            //            //AssignPanelSprite();

            //        }
            //    }

        }

    }
}

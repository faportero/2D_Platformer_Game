using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerMovementNew;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public static Vector3 lastPosition;

    [SerializeField] private float adictionAmount;
    public float currentAdiction;
    [SerializeField] public int SaludAmount = 0;
    [SerializeField] private UI_AdiccionBar adiccionBar;
    public UI_Coins uiCoins;

    [SerializeField] private UI_Lifes uiLifes;
    [SerializeField] public UI_Salud uiSalud;
    [SerializeField] public UI_Habilidades uiHabilidades;
    [HideInInspector] public List<Enemy> enemies;
    [HideInInspector] public List<Health> healhts;
    [HideInInspector] public List<Ability> abilities;
    public GameObject effectPanel;
    private PlayerMovementNew playerMovement;

    [SerializeField] private GameObject inmunidad;

    public bool isDie = false;

    public bool isCannabis, isCocaMetaHero, isPsilo, isAlcohol, isTabaco;
    public bool escudo, saltoDoble, vidaExtra, paracaidas;

    private bool isEnemy;
    public bool isDrugged;

    public SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine;
    private Coroutine enemyEffectCoroutine;
    private CapsuleCollider2D capsuleCollider;
    public bool isAttack = true;
    public bool isEnemyAttack;
    private float attackkDuration = 1;
    private int spritePanelIndex;
    private DistanceTracker distanceTracker;

    GhostController ghostController;
    [SerializeField] private UI_FeedbackSalud ui_feedback;
    [SerializeField] private GameObject panelFeedback;

    private void Start()
    {
        //inmunidad = transform.GetChild(0).gameObject;
        //inmunidad.transform.SetParent(transform);
        if (lastPosition != Vector3.zero)
        {
            transform.position = lastPosition;
        }
        if (escudo) inmunidad.SetActive(true);

        ghostController = GetComponent<GhostController>();
       // ghostController.enabled = false;
        currentAdiction = adiccionBar.currentAdiccion;

        enemies = new List<Enemy>();
        healhts = new List<Health>();
        abilities = new List<Ability>();
        playerMovement = GetComponent<PlayerMovementNew>();
        FindEnemies();
        FindAbilities();
        FindHealth();

        //print("escudo" + UserData.escudo);
        //print("salto doble" + UserData.saltoDoble);
        //print("Vida extra" + UserData.vidaExtra);
        //print("paracaidas" + UserData.paracaidas);

        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        distanceTracker = FindAnyObjectByType<DistanceTracker>();

    }

    private void FindEnemies()
    {
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemies.Clear();

        foreach (GameObject enemyObject in foundEnemies)
        {
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Add(enemy);

            }
        }
    }

    private void FindHealth()
    {
        GameObject[] foundHealth = GameObject.FindGameObjectsWithTag("Salud");
        healhts.Clear();

        foreach (GameObject healthObject in foundHealth)
        {
            Health health = healthObject.GetComponent<Health>();
            if (health != null)
            {
                healhts.Add(health);

            }
        }
       // print(healhts);
    }

    private void FindAbilities()
    {
        GameObject[] foundAbilities = GameObject.FindGameObjectsWithTag("Ability");
        abilities.Clear();

        foreach (GameObject abilityObject in foundAbilities)
        {
            Ability ability = abilityObject.GetComponent<Ability>();
            if (ability != null)
            {
                abilities.Add(ability);

            }
        }
    }

    void Update()
    {
        EnemyEffect();
    }

    public void TakeAdiccion(Enemy enemy)
    {
        if (enemy.isAdict)
        {
            adiccionBar.UpdateAdiccion(adictionAmount);
            //enemy.Effect();
            //enemy.EnemyDie();
            currentAdiction = adiccionBar.currentAdiccion;
        }

    }
    public void TakeAbility(Ability ability)
    {
        adiccionBar.UpdateAdiccion(adictionAmount);
        ability.NewAbility();
        ability.AbilityDie();
        currentAdiction = adiccionBar.currentAdiccion;
    }

    public void TakeHealth(Health health)
    {
       // health.AssignPanelSprite();
       // health.ShowFeedback();
        //adiccionBar.UpdateAdiccion(adictionAmount);
        //ability.NewAbility();
        //ability.AbilityDie();
        //currentAdiction = adiccionBar.currentAdiccion;
    }

    public void TakeSalud()
    {
        //UserData.health += 1;
        //UserData.health = Mathf.Clamp(UserData.health, 0, 3);
        SaludAmount += 1;
        SaludAmount = Mathf.Clamp(SaludAmount, 0, 3);
        if (SaludAmount == 3)
        {
            playerMovement.canSmash = true;
        }
        else
        {
            playerMovement.canSmash = false;
        }

        //StopAllCoroutines();
        effectPanel.SetActive(false);
        isCannabis = false;
        isCocaMetaHero = false;
        isPsilo = false;
        isAlcohol = false;
        isTabaco = false;

    }
    public void LoseLife()
    {
        if (uiLifes.lifesCount > 0)
        {
            uiLifes.UpdateLife();
            if (uiLifes.lifesCount == 0)
            {
                uiLifes.UpdateLife();
                playerMovement.canMove = false;
                uiHabilidades.CheckAvailable();
                playerMovement.Die();
                playerMovement.DieMaterialAnim();
            }
        }
    
    }


    private void EnemyEffect()
    {
        if (isCannabis)
        {
            isDrugged = true;
            playerMovement.inputsEnabled = false;
           // if (enemyEffectCoroutine != null) StopCoroutine(enemyEffectCoroutine);
            enemyEffectCoroutine = StartCoroutine(CurrentEffect(2.5f));
        }
        else if (isCocaMetaHero)
        {
            isDrugged = true;
            playerMovement.inputsEnabled = false;
           // if (enemyEffectCoroutine != null) StopCoroutine(enemyEffectCoroutine);
            enemyEffectCoroutine = StartCoroutine(CurrentEffect(2.5f));
        }
        else if (isAlcohol)
        {
            isDrugged = true;
            playerMovement.inputsEnabled = false;
          //  if (enemyEffectCoroutine != null) StopCoroutine(enemyEffectCoroutine);
            enemyEffectCoroutine = StartCoroutine(CurrentEffect(2.5f));
        }
        else if (isPsilo)
        {
            isDrugged = true;
            playerMovement.inputsEnabled = false;
         //   if (enemyEffectCoroutine != null) StopCoroutine(enemyEffectCoroutine);
            enemyEffectCoroutine = StartCoroutine(CurrentEffect(2.5f));
        }
        else if (isTabaco)
        {
            isDrugged = true;
            playerMovement.inputsEnabled = false;
          //  if (enemyEffectCoroutine != null) StopCoroutine(enemyEffectCoroutine);
            enemyEffectCoroutine = StartCoroutine(CurrentEffect(2.5f));

        }
    }

    private IEnumerator CurrentEffect(float delay)
    {
        if (!isDie && playerMovement.canMove)
        {
            effectPanel.SetActive(true);
            yield return new WaitForSecondsRealtime(delay);
            //StopAllCoroutines();
            effectPanel.SetActive(false);
            isCannabis = false;
            isCocaMetaHero = false;
            isPsilo = false;
            isAlcohol = false;
            isTabaco = false;
            isDrugged = false;
            playerMovement.inputsEnabled = true;
        }
    }

    #region Parpadeo

    public void StartBlinking()
    {

        //if(isDrugged) playerMovement.inputsEnabled = false;
        if (playerMovement.canMove)
        {

            if (!isAttack)
            {
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }

                blinkCoroutine = StartCoroutine(BlinkAlpha());
            }
        } 
    }

    private IEnumerator BlinkAlpha()
    {
        //capsuleCollider.enabled = false;
        isAttack = true;       
        float blinkSpeed = 6.0f; // Velocidad del parpadeo
        float elapsedTime = 0.0f;

        while (elapsedTime < attackkDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.PingPong(elapsedTime * blinkSpeed, 1.0f);
            SetAlpha(alpha);
            yield return null;
        }

        // Asegúrate de que el sprite esté completamente visible al final
        //capsuleCollider.enabled = false;
        SetAlpha(1.0f);
        isAttack = false;
        //if (!isDrugged) playerMovement.inputsEnabled = true;
        blinkCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    #endregion

    public void StarEnemyAttack()
    {
        StartCoroutine(EnemyAttack());
    }
    private IEnumerator EnemyAttack()
    {
        isEnemyAttack = true;         
        float elapsedTime = 0.0f;
        while (elapsedTime < attackkDuration)
        {
            elapsedTime += Time.deltaTime;  
            yield return null;
        }
        isEnemyAttack = false;
    }

    public void ShowFeedback()
    {
        if (isShowPanel) StopCoroutine(coroutineFeedback);
        coroutineFeedback = StartCoroutine(Feedback());
    }

    private IEnumerator Feedback()
    {
        isShowPanel = true;
        panelFeedback.SetActive(true);
        yield return new WaitForSeconds(2f);
        panelFeedback.SetActive(false);
        isShowPanel = false;


    }
    int protaCount = 0;
    private Coroutine coroutineFeedback;
    private bool isShowPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //if (collision.gameObject.layer == 7)
        if (collision.tag == "BadFloor" && !isAttack)
        {
            if (playerMovement.canMove) StartBlinking();
            //print("badlayer" + collision.gameObject.name);
            LoseLife();
           // return;
        }
        //if(collision.tag == "Enemy")
        //{
        //    StarEnemyAttack();
        //}
        

       //float hue, saturation, value;        
       //Color.RGBToHSV(hearthColor, out hue, out saturation, out value);


        if (collision.tag == "Salud")
        {
           // if(escudo)Physics2D.IgnoreCollision(inmunidad.GetComponent<BoxCollider2D>(), GetComponent<CapsuleCollider2D>());

            uiSalud.UpdateSalud(1);
            TakeSalud();
            ShowFeedback();





            //for (int i = 0; i < uiLifes.transform.childCount; i++)
            //{
            //    if (uiLifes.transform.GetChild(i).GetComponent<Image>().color != Color.white)
            //    {
            //        List<GameObject> blackHearths = new List<GameObject>();
            //        var child = uiLifes.transform.GetChild(i);
            //        blackHearths.Add(child.gameObject);
            //        Color newColor = blackHearths[0].GetComponent<Image>().color;
            //        newColor.r += .33f;
            //        newColor.g += .33f;
            //        newColor.b += .33f;
            //        blackHearths[0].GetComponent<Image>().color = newColor;
                  
            //    }
            //}




            //Color hearthColor = uiLifes.lifes.Last().GetComponent<Image>().color;
            //print(uiLifes.lifes.Last());
            //print(hearthColor);
            //hearthColor.r += .33f;
            //hearthColor.g += .33f;
            //hearthColor.b += .33f;
            //uiLifes.lifes[uiLifes.lifes.Count - 1].GetComponent<Image>().color = hearthColor;
            //print(uiLifes.lifes[uiLifes.lifes.Count - 1].name);
            //print(hearthColor);

            protaCount++;
            if (protaCount == 3)
            {
                uiLifes.CreateLife();
                protaCount = 0;
            }
            // print(collision.GetComponent<Health>().spriteIndex);
            spritePanelIndex = collision.GetComponent<Health>().spriteIndex;
            ui_feedback.image.sprite = ui_feedback.feedbackSprites[spritePanelIndex];
            //print(uiLifes.lifesCount);
            //print(uiLifes.lifes.Count);
            // StartCoroutine(ShowFeedbackPanel());






            //if (currentAdiction != 0)
            //{
            //    currentAdiction -= adictionAmount;
            //    adiccionBar.UpdateAdiccion(-adictionAmount);
            //    // print(adiccionBar.adiccionFillBar.fillAmount);
            //}
            //if (currentAdiction <= 0)
            //{
               
              

            //    //print(currentAdiction);
            //}


        }
       // print(protaCount);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerMovementNew;

public class PlayerControllerNew : MonoBehaviour
{
    [HideInInspector] public static Vector3 lastPosition;

    [SerializeField] private float saludAmount;
    public float currentAdiction;
    [SerializeField] public int SaludAmount = 0;
    [SerializeField] private  UI_SaludBar saludBar;
    public UI_Coins uiCoins;

    [SerializeField] private UI_Lifes uiLifes;
    [SerializeField] public UI_Salud uiSalud;
    [SerializeField] public UI_Habilidades uiHabilidades;
    [HideInInspector] public List<EnemyNew> enemies;
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
    private GameObject currentItem;

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
        saludBar.currentAdiccion = currentAdiction;

        enemies = new List<EnemyNew>();
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
            EnemyNew enemy = enemyObject.GetComponent<EnemyNew>();
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

    public void TakeAdiccion(EnemyNew enemy)
    {
        if (enemy.isAdict)
        {
            saludBar.UpdateHealth(saludAmount);
            //enemy.Effect();
            //enemy.EnemyDie();
            currentAdiction = saludBar.currentAdiccion;
        }

    }
    public void TakeAbility(Ability ability)
    {
        saludBar.UpdateHealth(saludAmount);
        ability.NewAbility();
        ability.AbilityDie();
        currentAdiction = saludBar.currentAdiccion;
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
    private Vector3 startPosition;
    private Vector3 startScale;
    private Vector3 targetPosition;
    private Vector3 targetScale;
    public AnimationCurve animationItemPositionCurve, animationItemScaleCurve;
    private Image healthFillBar;
    [SerializeField] private Gradient colorEnemyGradient;

    private IEnumerator TakeEnemyBarAnim(float duration)
    {
        float startFillAmount = healthFillBar.fillAmount;
        float targetFillAmount = 1/100;
        float elapsedTime = 0;        

        healthFillBar.color = colorEnemyGradient.Evaluate(targetFillAmount);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthFillBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            yield return null;
        }
       currentItem.gameObject.SetActive(false);

    }

    private IEnumerator TakeSaludAnim()
    {
        Vector3 startPosition = currentItem.transform.position;
        Vector3 targetPosition = GetWorldPositionFromUI(saludBar.GetComponent<RectTransform>());
        targetPosition = targetPosition + new Vector3(0, -.25f, 0);
        Vector3 startScale = currentItem.transform.localScale;

        float elapsedTime = 0;
        float duration = 0.5f;
        float duration2 = 1f;

        while (elapsedTime < duration2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float t1 = elapsedTime / duration;
            float curvePosValue = animationItemPositionCurve.Evaluate(t);
            float curveScaleValue = animationItemScaleCurve.Evaluate(t1);
            currentItem.transform.position = Vector3.Lerp(startPosition, targetPosition, curvePosValue);
            currentItem.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, curveScaleValue);
            yield return null;
        }

        // Asegurarse de que el valor final sea el targetPosition
        currentItem.transform.position = targetPosition;
    }

    private Vector3 GetWorldPositionFromUI(RectTransform uiElement)
    {
        // Obtener la posición en pantalla del elemento UI
        Vector3 screenPos = uiElement.position;
        // Convertir la posición en pantalla a posición en el mundo
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiElement, screenPos, Camera.main, out worldPos);
        return worldPos;
    }
    IEnumerator ActivarEnfasis()
    {
        yield return new WaitForSeconds(.5f);
        Animator animator = saludBar.GetComponent<Animator>();
        if (!animator.isActiveAndEnabled) animator.enabled = true;
        animator.Rebind();
        animator.SetBool("Enfasis", true);
    }
    IEnumerator DeactivateEnfasis()
    {
        yield return new WaitForSeconds(.5f);
        Animator animator = saludBar.GetComponent<Animator>();
        if (!animator.isActiveAndEnabled) animator.enabled = true;
        animator.Rebind();
        animator.SetBool("Enfasis", false);
    }
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

        if (collision.tag == "Salud")
        {
            currentItem = collision.gameObject;
            StartCoroutine(TakeSaludAnim());
            StartCoroutine(ActivarEnfasis());
            ShowFeedback();

            currentAdiction += saludAmount;
            saludBar.UpdateHealth(saludAmount);
            currentAdiction = Mathf.Clamp(currentAdiction, 0f, 100);
            saludBar.currentAdiccion = currentAdiction;
            print(currentAdiction);
            //return;
        }

        if (collision.tag == "Enemy")
        {
            collision.GetComponent<BoxCollider2D>().enabled = false;
            collision.transform.GetChild(0).gameObject.SetActive(true);
            //collision.gameObject.GetComponent<Enemy>().Effect();
            healthFillBar = collision.transform.GetChild(0).GetChild(1).GetComponent<Image>();
            currentItem = collision.gameObject;
            collision.gameObject.transform.parent = transform;
            StartCoroutine(DeactivateEnfasis());
            StartCoroutine(TakeEnemyBarAnim(2.5f));



            currentAdiction += -saludAmount;
            saludBar.UpdateHealth(-saludAmount);
            currentAdiction = Mathf.Clamp(currentAdiction, 0f, 100);
            saludBar.currentAdiccion = currentAdiction;
            print(currentAdiction);
        }
    }

}

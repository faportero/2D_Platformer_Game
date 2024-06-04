using System;
using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector] public List<Ability> abilities;
    public GameObject effectPanel;
    private PlayerMovementNew playerMovement;

    private GameObject inmunidad;

    public bool isDie = false;

    public bool isCannabis, isCocaMetaHero, isPsilo, isAlcohol, isTabaco;
    public bool escudo, saltoDoble, vidaExtra, paracaidas;

    private bool isEnemy;
    public bool isDrugged;

    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine;
    private CapsuleCollider2D capsuleCollider;
    public bool isAttack = true;

    private void Awake()
    {

        inmunidad = transform.GetChild(0).gameObject;
        if (lastPosition != Vector3.zero)
        {
            transform.position = lastPosition;
        }
        if (escudo) inmunidad.SetActive(true);
    }
    private void Start()
    {
        
        currentAdiction = adiccionBar.currentAdiccion;

        enemies = new List<Enemy>();
        abilities = new List<Ability>();
        playerMovement = GetComponent<PlayerMovementNew>();
        FindEnemies();
        FindAbilities();

        //print("escudo" + UserData.escudo);
        //print("salto doble" + UserData.saltoDoble);
        //print("Vida extra" + UserData.vidaExtra);
        //print("paracaidas" + UserData.paracaidas);

        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();


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
            enemy.Effect();
            enemy.EnemyDie();
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
        if (uiLifes.lifesCount > 1)
        {
            uiLifes.UpdateLife();
        }
        else
        {
            playerMovement.canMove = false;
            uiHabilidades.CheckAvailable();
            playerMovement.Die();
        }
    }


    private void EnemyEffect()
    {
        if (isCannabis)
        {
            isDrugged = true;
            StartCoroutine(CurrentEffect(5));
        }
        else if (isCocaMetaHero)
        {
            isDrugged = true;
            StartCoroutine(CurrentEffect(5));
        }
        else if (isAlcohol)
        {
            isDrugged = true;
            StartCoroutine(CurrentEffect(5));
        }
        else if (isPsilo)
        {
            isDrugged = true;
            StartCoroutine(CurrentEffect(5));
        }
        else if (isTabaco)
        {
            isDrugged = true;
            StartCoroutine(CurrentEffect(5));

        }
    }

    private IEnumerator CurrentEffect(float delay)
    {


        if (!isDie)
        {
            effectPanel.SetActive(true);
            yield return new WaitForSecondsRealtime(delay);
            StopAllCoroutines();
            effectPanel.SetActive(false);
            isCannabis = false;
            isCocaMetaHero = false;
            isPsilo = false;
            isAlcohol = false;
            isTabaco = false;
            isDrugged = false;
       }


    }

    #region Parpadeo

    public void StartBlinking()
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

    private IEnumerator BlinkAlpha()
    {
        //capsuleCollider.enabled = false;
        isAttack = true;
        float blinkDuration = 1.0f; // Duración total de 3 segundos
        float blinkSpeed = 6.0f; // Velocidad del parpadeo
        float elapsedTime = 0.0f;

        while (elapsedTime < blinkDuration)
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
        blinkCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.layer == 7)
        if (collision.tag == "BadFloor" && !isAttack)
        {
            StartBlinking();
            //print("badlayer" + collision.gameObject.name);
            LoseLife();
           // return;
        }
        if (collision.tag == "Salud")
        {
            if (currentAdiction != 0)
            {
                currentAdiction -= adictionAmount;
                adiccionBar.UpdateAdiccion(-adictionAmount);
                // print(adiccionBar.adiccionFillBar.fillAmount);
            }
            if (currentAdiction <= 0)
            {
                uiSalud.UpdateSalud(1);
                TakeSalud();
               
                //print(currentAdiction);
            }
        }
    }
}

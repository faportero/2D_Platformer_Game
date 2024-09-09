using System.Collections;
using UnityEngine;

public class PlayerGuide : MonoBehaviour
{
    [SerializeField] BoxCollider2D guideCollider, guideColliderExit;
    [SerializeField] GameObject playerGuide;

    [HideInInspector] public SpriteRenderer playerGuideSprite, playerGlowSprite;
    private Animator playerGuideAnimator;
    private Coroutine guideSolidify, guideDissolve;

    public bool isFacingRight;
    private void Start()
    {
        playerGuideSprite = playerGuide.GetComponent<SpriteRenderer>();
        playerGlowSprite = playerGuideSprite.transform.GetChild(0).GetComponent<SpriteRenderer>();
        playerGuideAnimator = playerGuide.GetComponent<Animator>();
    }

    private void Update()
    {
        // Puedes descomentar esta línea para depurar el valor de disolución
        // print("Guide: " + playerGuideSprite.material.GetFloat("_DissolveAmmount") + ". Glow: " + playerGlowSprite.material.GetFloat("_DissolveAmmount"));
    }

    public void ShowPlayerGuide()
    {
        guideCollider.enabled = false;
        playerGuide.SetActive(true);
        playerGlowSprite.gameObject.SetActive(true);
        GuideSolidify(); // Llama a la corrutina para solidificar ambos sprites
        playerGuideAnimator.enabled = true;
        playerGuideAnimator.SetBool("Guide", true);
    }

    public void HidePlayerGuide()
    {
        guideColliderExit.enabled = false;
        playerGuideAnimator.enabled = false;
        playerGuideAnimator.SetBool("Guide", false);
        GuideDisolve(); // Llama a la corrutina para disolver ambos sprites
    }

    public void GuideSolidify()
    {
        // Detenemos cualquier corrutina en curso para evitar conflictos
        if (guideSolidify != null)
        {
            StopCoroutine(guideSolidify);
        }
        // Inicia la corrutina para solidificar ambos sprites
        guideSolidify = StartCoroutine(GuideSolidifyAnim(new SpriteRenderer[] { playerGuideSprite, playerGlowSprite }));
    }

    public void GuideDisolve()
    {
        // Detenemos cualquier corrutina en curso para evitar conflictos
        if (guideDissolve != null)
        {
            StopCoroutine(guideDissolve);
        }
        // Inicia la corrutina para disolver ambos sprites
        guideDissolve = StartCoroutine(GuideDisolveAnim(new SpriteRenderer[] { playerGuideSprite, playerGlowSprite }));
    }

    private IEnumerator GuideSolidifyAnim(SpriteRenderer[] spriteRenderers)
    {
        float dissolveAmount = 0;
        float duration = 2f; // Duración de la animación
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(1, 0, elapsedTime / duration);
            foreach (var sr in spriteRenderers)
            {
                sr.material.SetFloat("_DissolveAmmount", dissolveAmount);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de que el valor final sea 0
        foreach (var sr in spriteRenderers)
        {
            sr.material.SetFloat("_DissolveAmmount", 0);
        }
    }

    private IEnumerator GuideDisolveAnim(SpriteRenderer[] spriteRenderers)
    {
        float dissolveAmount = 0;
        float duration = 2f; // Duración de la animación
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(0, 1, elapsedTime / duration);
            foreach (var sr in spriteRenderers)
            {
                sr.material.SetFloat("_DissolveAmmount", dissolveAmount);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de que el valor final sea 1
        foreach (var sr in spriteRenderers)
        {
            sr.material.SetFloat("_DissolveAmmount", 1);
        }

        // Desactiva los objetos al final de la animación
        gameObject.SetActive(false);
        playerGlowSprite.gameObject.SetActive(false);
    }
}

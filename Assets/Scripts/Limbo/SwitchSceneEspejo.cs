using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SwitchSceneEspejo : MonoBehaviour
{
    [SerializeField] GameObject canvasFog;
    [SerializeField] string nivel;
    private float progress;
    private PlayerMovementNew playerMovementNew;
    private Material playerMaterial;
    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        playerMaterial = playerMovementNew.GetComponent<SpriteRenderer>().material;

    }
    private void Update()
    {
        print(nivel);
    }
    public void SwitchSceneFromUI()
    {
        StartCoroutine(SwitchScene());
    }
    private IEnumerator SwitchScene()
    {
        AudioManager.Instance.ToggleMusic();

        // playerMovementNew.isMoving = false;
        //playerMovementNew.canMove = false;
        //FogTransicion


        //yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(PlayerDisolve());

        yield return new WaitForSecondsRealtime(0.5f);
        LevelManager.isFogTransition = true;
        canvasFog.SetActive(true);
        canvasFog.transform.GetChild(0).gameObject.SetActive(true);
        canvasFog.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        canvasFog.transform.GetChild(0).GetComponent<Animator>().Play("FogTransition");
        AudioManager.Instance.PlaySfx("Fog_Transition");

        yield return new WaitForSecondsRealtime(4.5f);
        SceneManager.LoadScene(nivel);
        //AsyncOperation asyncOperation;
        //asyncOperation = SceneManager.LoadSceneAsync(nivel);
        //asyncOperation.allowSceneActivation = false;
        //while (!asyncOperation.isDone)
        //{
        //    progress = asyncOperation.progress;
        //    if (progress == .9f)
        //    {
        //        yield return new WaitForSecondsRealtime(3f);
        //        asyncOperation.allowSceneActivation = true;
        //        AudioManager.Instance.PlayMusic("Bg_Nivel_1", 0);
        //    }
        //}
    }
    private IEnumerator PlayerDisolve()
    {
        AudioManager.Instance.PlaySfx("Dissolve");

        float dissolveAmount = 0;
        float duration = 1f;  // Duración total de la animación en segundos
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(0, 1, elapsedTime / duration);
            playerMaterial.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            yield return null;  // Esperar al siguiente frame
        }

        // Asegurarse de que el valor final sea exactamente 1
        playerMaterial.SetFloat("_DissolveAmmount", 1);
    }
}

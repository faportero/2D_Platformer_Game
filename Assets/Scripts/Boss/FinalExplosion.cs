using UnityEngine;

public class FinalExplosion : MonoBehaviour
{
    [SerializeField] private GameObject enteGodRay, witheFadePanel;
    private Animator enteGodRayAnim, witheFadePanelAnim;
    private bool isFadeAnimationDone = false;

    private void Awake()
    {
        enteGodRayAnim = enteGodRay.GetComponent<Animator>();
        witheFadePanelAnim = witheFadePanel.GetComponent<Animator>();
    }

    private void OnEnable()
    {

    }
    public void StartExplosion()
    {
        witheFadePanel.SetActive(true);
        enteGodRayAnim.enabled = true;
        witheFadePanelAnim.enabled = true;
        enteGodRayAnim.Play("Expand");
        witheFadePanelAnim.Play("Fade");
        isFadeAnimationDone = false;
    }
    private void Update()
    {
        if (isFadeAnimationDone) return;

        AnimatorStateInfo stateInfo = witheFadePanelAnim.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo stateInfo2 = enteGodRayAnim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Fade") && stateInfo.normalizedTime >= 1.0f)
        {
            isFadeAnimationDone = true;
            witheFadePanel.SetActive(false);
        }
        if (stateInfo2.IsName("Expand") && stateInfo2.normalizedTime >= 1.0f)
        {
            enteGodRay.SetActive(false);
        }
    }
}

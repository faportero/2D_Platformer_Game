using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    public GameObject ghostPrefab;
    public float delay = 1;
    float delta = 0;


    PlayerControllerNew player;
    SpriteRenderer spriteRenderer;

    public float destroyTime = .1f;
    public Color color;
    public Material material = null;

    private void Start()
    {       
        player = GetComponent<PlayerControllerNew>();

    }
    private void Update()
    {
        if (delta > 0) { delta -= Time.deltaTime; }
        else { delta = delay; createGhost(); }
        //print(transform.position);
    }
    void createGhost()
    {
        GameObject ghostObj = Instantiate(ghostPrefab, transform.position, transform.rotation);        
        ghostObj.transform.localScale = player.transform.localScale;
        Destroy(ghostObj, destroyTime);

        spriteRenderer = ghostObj.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = player.spriteRenderer.sprite;
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = player.spriteRenderer.sortingOrder;
        if (material != null) spriteRenderer.material = material;
        spriteRenderer.sortingOrder = player.spriteRenderer.sortingOrder - 1;
    }
}

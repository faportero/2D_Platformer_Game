using UnityEngine.Tilemaps;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public GameObject[] platformPrefabs;
    public GameObject[] itemPrefabs;
    public ObjectPool platformPool;
    public ObjectPool itemPool;
    public Transform spawnParent;

    public AnimationCurve difficultyCurve; // Curva de dificultad para el intervalo de spawn
    public float initialSpawnInterval = 10f; // Intervalo de spawn inicial
    public float minSpawnInterval = 3f; // Intervalo de spawn mínimo

    private float spawnInterval;
    private float timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        platformPool.SetPrefabs(platformPrefabs);
        itemPool.SetPrefabs(itemPrefabs);

        // Inicializar el intervalo de spawn
        spawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        // Código para el manejo de spawn basado en dificultad (comentado)
    }

    private void SpawnObject()
    {
        if (Random.value < 0.5f)
        {
            SpawnPlatform();
        }
        else
        {
            SpawnItem();
        }
    }

    private void SpawnPlatform()
    {
        GameObject platformPrefab = GetRandomPlatformPrefab();
        if (platformPrefab != null)
        {
            GameObject platform = platformPool.GetObjectFromPool(platformPrefab);
            platform.transform.position = new Vector2(spawnParent.transform.position.x, spawnParent.transform.position.y);

            // Ajustar la posición basado en el TilemapRenderer o TilemapCollider2D
            TilemapRenderer tilemapRenderer = platform.GetComponent<TilemapRenderer>();
            TilemapCollider2D tilemapCollider = platform.GetComponent<TilemapCollider2D>();

            if (tilemapRenderer != null)
            {
                Vector3 pivotAdjustment = tilemapRenderer.bounds.center - platform.transform.position;
                platform.transform.position -= pivotAdjustment;
            }
            else if (tilemapCollider != null)
            {
                Vector3 pivotAdjustment = tilemapCollider.bounds.center - platform.transform.position;
                platform.transform.position -= pivotAdjustment;
            }
        }
    }

    public GameObject SpawnItem()
    {
        GameObject itemPrefab = GetRandomItemPrefab();
        if (itemPrefab != null)
        {
            GameObject item = itemPool.GetObjectFromPool(itemPrefab);
            item.transform.position = new Vector2(spawnParent.transform.position.x, spawnParent.transform.position.y);

            // Ajustar la posición basado en el SpriteRenderer
            SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Vector3 pivotAdjustment = spriteRenderer.bounds.center - item.transform.position;
                item.transform.position -= pivotAdjustment;
            }

            // Devuelve el ítem instanciado
            return item;
        }

        // Devuelve null si no se pudo crear el ítem
        return null;
    }

    private GameObject GetRandomPlatformPrefab()
    {
        return platformPrefabs[Random.Range(0, platformPrefabs.Length)];
    }

    private GameObject GetRandomItemPrefab()
    {
        return itemPrefabs[Random.Range(0, itemPrefabs.Length)];
    }
}

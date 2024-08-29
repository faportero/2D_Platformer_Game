using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public GameObject[] itemPrefabs;
    public ObjectPool platformPool;
    public ObjectPool itemPool;
    public Transform spawnParent;

    public Vector2[] platformSpawnPositions;
    public Vector2[] itemSpawnPositions;

    public AnimationCurve difficultyCurve; // Curva de dificultad para el intervalo de spawn
    public float initialSpawnInterval = 10f; // Intervalo de spawn inicial
    public float minSpawnInterval = 3f; // Intervalo de spawn mínimo

    private float spawnInterval;
    private float timer;

    private void Start()
    {
        platformPool.SetPrefabs(platformPrefabs);
        itemPool.SetPrefabs(itemPrefabs);

        // Inicializar el intervalo de spawn
        spawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Actualizar el intervalo de spawn basado en la curva de dificultad
        float normalizedTime = Mathf.Clamp01(timer / (initialSpawnInterval * 2)); // Normaliza el tiempo en un rango de 0 a 1
        spawnInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, difficultyCurve.Evaluate(normalizedTime));

        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
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
        Vector2 spawnPosition = platformSpawnPositions[Random.Range(0, platformSpawnPositions.Length)];
        GameObject platformPrefab = GetRandomPlatformPrefab();
        if (platformPrefab != null)
        {
            GameObject platform = platformPool.GetObjectFromPool(platformPrefab);
            platform.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x, spawnPosition.y);

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

    private void SpawnItem()
    {
        Vector2 spawnPosition = itemSpawnPositions[Random.Range(0, itemSpawnPositions.Length)];
        GameObject itemPrefab = GetRandomItemPrefab();
        if (itemPrefab != null)
        {
            GameObject item = itemPool.GetObjectFromPool(itemPrefab);
            item.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x, spawnPosition.y);

            // Ajustar la posición basado en el SpriteRenderer
            SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Vector3 pivotAdjustment = spriteRenderer.bounds.center - item.transform.position;
                item.transform.position -= pivotAdjustment;
            }
        }
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

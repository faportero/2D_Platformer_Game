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

    }

    void Update()
    {
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

    public GameObject SpawnPlatform()
    {
        GameObject platformPrefab = GetRandomPlatformPrefab();
        if (platformPrefab != null)
        {

            GameObject platform = platformPool.GetObjectFromPool(platformPrefab);
            platform.transform.position = new Vector2(spawnParent.transform.position.x, spawnParent.transform.position.y);



            return platform; // Devuelve la instancia creada
        }
        return null;
    }

    public GameObject SpawnItem()
    {
        GameObject itemPrefab = GetRandomItemPrefab();
        if (itemPrefab != null)
        {
            GameObject item = itemPool.GetObjectFromPool(itemPrefab);
            item.transform.position = new Vector2(spawnParent.transform.position.x, spawnParent.transform.position.y);

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

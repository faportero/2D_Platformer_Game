using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public GameObject[] itemPrefabs;
    public Transform spawnParent;

    public Vector3[] platformSpawnPositions;
    public Vector3[] itemSpawnPositions;

    public float spawnInterval = 5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    private void SpawnObject()
    {
        // Decide randomly whether to spawn a platform or an item
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
        Vector3 spawnPosition = GetRandomPlatformSpawnPoint();
        GameObject prefab = GetRandomPlatformPrefab();
        GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity, spawnParent);

        // Ajustar la posición basado en el TilemapRenderer o TilemapCollider2D
        TilemapRenderer tilemapRenderer = spawnedObject.GetComponent<TilemapRenderer>();
        TilemapCollider2D tilemapCollider = spawnedObject.GetComponent<TilemapCollider2D>();

        if (tilemapRenderer != null)
        {
            Vector3 pivotAdjustment = tilemapRenderer.bounds.center - spawnedObject.transform.position;
            spawnedObject.transform.position -= pivotAdjustment;
        }
        else if (tilemapCollider != null)
        {
            Vector3 pivotAdjustment = tilemapCollider.bounds.center - spawnedObject.transform.position;
            spawnedObject.transform.position -= pivotAdjustment;
        }
    }

    private void SpawnItem()
    {
        Vector3 spawnPosition = GetRandomSpawnPoint();
        GameObject prefab = GetRandomItemPrefab();
        GameObject spawnedItem = Instantiate(prefab, spawnPosition, Quaternion.identity, spawnParent);

        // Ajustar la posición basado en el SpriteRenderer
        SpriteRenderer spriteRenderer = spawnedItem.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Vector3 pivotAdjustment = spriteRenderer.bounds.center - spawnedItem.transform.position;
            spawnedItem.transform.position -= pivotAdjustment;
        }
    }

    private Vector3 GetRandomPlatformSpawnPoint()
    {
        // Asegúrate de que este método devuelva una posición válida para las plataformas
        return platformSpawnPositions[Random.Range(0, platformSpawnPositions.Length)];
    }

    private Vector3 GetRandomSpawnPoint()
    {
        // Asegúrate de que este método devuelva una posición válida para los ítems
        return itemSpawnPositions[Random.Range(0, itemSpawnPositions.Length)];
    }

    private GameObject GetRandomPlatformPrefab()
    {
        // Asegúrate de que este método devuelva un prefab de plataforma válido
        return platformPrefabs[Random.Range(0, platformPrefabs.Length)];
    }

    private GameObject GetRandomItemPrefab()
    {
        // Asegúrate de que este método devuelva un prefab de ítem válido
        return itemPrefabs[Random.Range(0, itemPrefabs.Length)];
    }
}

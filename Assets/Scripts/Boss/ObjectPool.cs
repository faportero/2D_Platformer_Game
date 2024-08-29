using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject[] prefabs;    // Lista de prefabs para diferentes tipos de objetos
    [SerializeField] private int poolSize = 10;       // Tamaño del pool para cada tipo de objeto

    private Dictionary<GameObject, Queue<GameObject>> pools;  // Diccionario para almacenar las colas de objetos

    protected virtual void Awake()
    {
        pools = new Dictionary<GameObject, Queue<GameObject>>();
    }

    public void SetPrefabs(GameObject[] prefabs)
    {
        this.prefabs = prefabs;
        InitializePool();
    }

    private void InitializePool()
    {
        pools.Clear();
        foreach (var prefab in prefabs)
        {
            Queue<GameObject> queue = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            pools[prefab] = queue;
        }
    }

    public GameObject GetObjectFromPool(GameObject prefab)
    {
        if (pools.TryGetValue(prefab, out Queue<GameObject> poolQueue))
        {
            if (poolQueue.Count > 0)
            {
                GameObject obj = poolQueue.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                Debug.LogWarning("Object pool is empty for prefab: " + prefab.name);
                GameObject obj = Instantiate(prefab);
                return obj;
            }
        }
        else
        {
            Debug.LogError("No pool found for prefab: " + prefab.name);
            return null;
        }
    }

    public void ReturnObjectToPool(GameObject prefab, GameObject obj)
    {
        if (pools.TryGetValue(prefab, out Queue<GameObject> poolQueue))
        {
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
        else
        {
            Debug.LogError("No pool found for prefab: " + prefab.name);
        }
    }

    public GameObject GetRandomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }
}

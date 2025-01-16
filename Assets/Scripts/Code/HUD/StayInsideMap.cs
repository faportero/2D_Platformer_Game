using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInsideMap : MonoBehaviour
{
    private Transform _miniMapCam;
    public Vector2 _minMapSize;
    Vector3 _tempV3;
    // Start is called before the first frame update
    void Start()
    {
        //layer 7 es MapCamera!!!!
        _miniMapCam = FindFirstObjectByTypeInLayer<Camera>(7).transform;
    }

    // Update is called once per frame
    void Update()
    {
        _tempV3 = transform.parent.position;
        _tempV3.z = transform.position.z;
        transform.position = _tempV3;
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, _miniMapCam.position.x - _minMapSize.x, _minMapSize.x + _miniMapCam.position.x),
            Mathf.Clamp(transform.position.y, _miniMapCam.position.y - _minMapSize.y, _minMapSize.y + _miniMapCam.position.y),
            0
            );
    }
    public GameObject FindFirstObjectByTypeInLayer<T>(int layer) where T : Component
    {
        GameObject[] objectsInLayer = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objectsInLayer)
        {
            if (obj.layer == layer)
            {
                T component = obj.GetComponent<T>();
                if (component != null)
                {
                    // Se encontró el componente en este GameObject, devolverlo
                    return obj;
                }
            }
        }

        // No se encontró ningún GameObject con el componente y la capa especificados
        return null;
    }
}

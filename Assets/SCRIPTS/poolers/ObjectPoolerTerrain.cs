using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerTerrain : MonoBehaviour {

    private Transform PlayerTransform;

    public static ObjectPoolerTerrain current;
    public GameObject pooledObject;
    public int pooledAmount = 5;
    public bool willGrow = true;

    List<GameObject> pooledObjects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }
        return null;
    }

    public GameObject GivePooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy)
            {
                if(PlayerTransform.transform.position.z - 700f > pooledObjects[i].transform.position.z)
                    return pooledObjects[i];
            }
        }
        return null;
    }
}

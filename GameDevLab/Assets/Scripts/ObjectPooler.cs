using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public List<ObjectPoolItem> itemsToPool; // types of differnet object to pool
    public List<ExistingPoolItem> pooledObjects; // a list of all objects in the pool, of all types
    public static ObjectPooler SharedInstance;

    public class ExistingPoolItem
    {
        public GameObject gameObject;
        public ObjectType type;

        public ExistingPoolItem(GameObject gameObject, ObjectType type)
        {
            this.gameObject = gameObject;
            this.type = type;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        SharedInstance = this;

        pooledObjects = new List<ExistingPoolItem>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amount; i++)
            {
                // this "pickup" a local variable, but Unity will not remove it since it exists in the scene
                GameObject pickup = (GameObject)Instantiate(item.prefab);
                pickup.SetActive(false);
                pickup.transform.parent = this.transform;
                ExistingPoolItem e = new ExistingPoolItem(pickup, item.type);
                pooledObjects.Add(e);
            }
        }
    }

    public GameObject GetPooledObject(ObjectType type)
    {
        // return inactive pooled object if it matches the type
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy && pooledObjects[i].type == type)
            {
                return pooledObjects[i].gameObject;
            }
        }

        // this will be called when no more active object is present, item to expand pool if required
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.type == type)
            {
                if (item.expandPool)
                {
                    GameObject pickup = (GameObject)Instantiate(item.prefab);
                    pickup.SetActive(false);
                    pickup.transform.parent = this.transform;
                    pooledObjects.Add(new ExistingPoolItem(pickup, item.type));
                    return pickup;
                }
            }
        }

        return null;
    }
}


public enum ObjectType
{
    gombaEnemy = 0,
    greenEnemy = 1
}

[System.Serializable]
public class ObjectPoolItem
{
    public int amount;
    public GameObject prefab;
    public bool expandPool;
    public ObjectType type;
}



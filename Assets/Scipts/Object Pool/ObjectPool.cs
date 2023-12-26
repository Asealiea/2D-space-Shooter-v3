using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectPoolID = ObjectPoolEnum.ObjectPoolID;
using System.Linq;


[Serializable]
public class ObjectPoolItem
{
    public string name;
    public int amountToPool;
    public GameObject objectToPool;
    public bool canBeExpanded;
    public ObjectPoolID enumTag;
}


public class ObjectPool : MonoBehaviour
{
    public List<ObjectPoolItem> itemsToPoolList;

    public static ObjectPool SharedInstance;
    public List<GameObject> pooledObjectList;
    private Dictionary<ObjectPoolID,GameObject> _pooledObjectDictionary;
    

    public Transform poolParent;

    private void Awake()
    {
        SharedInstance = this;
        _pooledObjectDictionary = new Dictionary<ObjectPoolID, GameObject>();
    }

    private void Start()
    {
        pooledObjectList = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPoolList)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool,poolParent.transform);
                obj.SetActive(false);
                pooledObjectList.Add(obj); //just making able to see list in inspector.
                _pooledObjectDictionary.Add(item.enumTag,obj);
            }
        }
    }

    public GameObject GetPooledObject(ObjectPoolID enumTag)
    {
        //if one is not active, use that, else check if another one can be spawned in.
        foreach (var pool in _pooledObjectDictionary.Where(pool => !pool.Value.activeInHierarchy && pool.Key == enumTag))
        {
            return pool.Value;
        }

        foreach (ObjectPoolItem item in itemsToPoolList)
        {
            if (item.enumTag != enumTag) continue;
            if (!item.canBeExpanded) continue;
            GameObject obj = Instantiate(item.objectToPool, poolParent.transform);
            obj.SetActive(false);
            pooledObjectList.Add(obj);//just to see in inspector
            _pooledObjectDictionary.Add(item.enumTag,obj);
            return obj;
        }
        Debug.Log("Can't find any free ones");
        return null;
    }

    private GameObject GetPooledObject(string tags)
    {
        //if one is not active, use that, else check if another one can be spawned in.
        foreach (var t in pooledObjectList.Where(t => !t.activeInHierarchy && t.CompareTag(tags)))
        {
            return t;
        }

        //If the object can expand then expand the number of objects in the pool
        foreach (var obj in from item in itemsToPoolList where item.objectToPool.CompareTag(tags) where item.canBeExpanded select Instantiate(item.objectToPool,poolParent.transform))
        {
            obj.SetActive(false);
            pooledObjectList.Add(obj);
            return obj;
        }
        Debug.Log("Can't find any free ones");
        return null;
    }

    public static void BackToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private static GameObject obj;
    public static void SpawnObject(Vector3 position, Quaternion rotation, string objectTag)
    {
        obj = SharedInstance.GetPooledObject(objectTag);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
    }
}


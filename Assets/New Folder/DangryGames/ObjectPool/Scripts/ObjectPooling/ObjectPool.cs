using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace DangryGames.ObjectPooling
{
    [Serializable]
    public class ObjectPoolItem
    {
        //Number of items to pool
        public int _amountToPool;
        //Gameoobject to pool
        public GameObject _objectToPool;
        //Expand the number of items pooled?
        public bool _shouldExpand;
    }


    public class ObjectPool : MonoBehaviour
    {
        //New List of ObjectPoolItem
        public List<ObjectPoolItem> _itemsToPool;

        //Single type of ObjectPool, makes it availabe from any script without reference
        public static ObjectPool SharedInstance;
        public List<GameObject> _pooledObjects;

        //A gameobject transform in scene for housing the instantiated pooled objects
        [Tooltip("Create an empty gameobject in the scene to house the instantiated objects once pooled")]
        public Transform _poolPosition;

        private void Awake()
        {
            SharedInstance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            //creates a list of pooled objects to instantiate from the _itemsToPool List
            _pooledObjects = new List<GameObject>();
            foreach (ObjectPoolItem item in _itemsToPool)
            {
                for (int i = 0; i < item._amountToPool; i++)
                {
                    //Create a gameobject
                    GameObject obj = (GameObject)Instantiate(item._objectToPool);
                    //Set the transform of the object
                    obj.transform.parent = _poolPosition.transform;
                    //Turn off the object
                    obj.SetActive(false);
                    //Add the obj to the _pooledObjects List
                    _pooledObjects.Add(obj);
                }
            }
        }

        public GameObject GetPooledObject(string tag)
        {
            //Checks to see if the current object is active in the scene. If is active move to the next object in the list. 
            //If not, then you exit the method and hand the object to the method caller

            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy && _pooledObjects[i].tag == tag)
                {
                    return _pooledObjects[i];
                }
            }

            //If the object can expand then expand the number of objects in the pool
            foreach (ObjectPoolItem item in _itemsToPool)
            {
                if (item._objectToPool.tag == tag)
                {
                    if (item._shouldExpand)
                    {
                        GameObject obj = (GameObject)Instantiate(item._objectToPool);
                        obj.transform.parent = _poolPosition.transform;
                        obj.SetActive(false);
                        _pooledObjects.Add(obj);
                        return obj;
                    }
                }
            }
            return null;
        }

        public void BackToPool(GameObject GO)
        {
            GO.SetActive(false);
        }

    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool _instance;

    List<GameObject> _pooledObjects = new List<GameObject>();
    int amountToPool = 80;

    [SerializeField] GameObject[] _ballPrefabList;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    CreateBallPool();
    }


    public void CreateBallPool()
    {
                for (int i=0; i< amountToPool; i++)
        {
            GameObject _poolObject = Instantiate(_ballPrefabList[Random.Range(0, 10)]);
            _poolObject.SetActive(false);
            _poolObject.transform.SetParent(gameObject.transform);
            _pooledObjects.Add(_poolObject);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i=0; i < _pooledObjects.Count; i++)
        {
            if(!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }
        return null;
    }

    public void ActivateAndSetPooledObjectPosition(GameObject pooledObject, Vector3 position, float scale)
    {
        if (pooledObject != null)
        {
            pooledObject.transform.position = position;
            pooledObject.transform.localScale = scale * new Vector3 (pooledObject.transform.localScale.x, pooledObject.transform.localScale.y, pooledObject.transform.localScale.z);
            pooledObject.SetActive(true);
        }
    }
    

}
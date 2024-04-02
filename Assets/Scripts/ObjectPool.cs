using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            foreach (GameObject ball in _ballPrefabList)
        {
            for (int i = 0; i<20; i++)
            {
                GameObject _poolObject = Instantiate(ball);
                _poolObject.SetActive(false);
                _poolObject.transform.SetParent(gameObject.transform);
                _pooledObjects.Add(_poolObject);
            }
        }
    }

    public GameObject GetRandomPooledBall()
    {
        for (int i=0; i < _pooledObjects.Count; i++)
        {
            int randomNumber = Random.Range(0,_pooledObjects.Count);
            if(!_pooledObjects[randomNumber].activeInHierarchy && int.Parse(_pooledObjects[randomNumber].tag)<=64)
            {
                return _pooledObjects[randomNumber];
            }
        }
        return null;
    }

    public GameObject GetPooledBallByTag(int tag)
    {
        for (int i=0; i < _pooledObjects.Count; i++)
        {
            if(!_pooledObjects[i].activeInHierarchy && _pooledObjects[i].tag == tag.ToString())
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

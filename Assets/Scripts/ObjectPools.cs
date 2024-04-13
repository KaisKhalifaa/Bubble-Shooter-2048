using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPools : MonoBehaviour
{
    public static ObjectPools Instance;

    List<GameObject> _pooledBalls = new List<GameObject>();
    List<GameObject> _pooledParticles = new List<GameObject>();
    [SerializeField] GameObject[] _ballPrefabList;
    [SerializeField] GameObject _particleSystemPrefab;
    [SerializeField] GameObject _particlePoolParent;
    [SerializeField] int _maximumBallTag = 128, _minimumBallTag = 2;
    public int MaximumSpawnedBallTag { private get{return _maximumBallTag;} set{_maximumBallTag = value;} }
    public int MinimumSpawnedBallTag { private get{return _minimumBallTag;} set{_minimumBallTag = value;} }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    CreateBallPool();
    CreateParticlePool();
    }
    public void CreateBallPool()
    {
            foreach (GameObject ball in _ballPrefabList)
        {
            for (int i = 0; i<20; i++)
            {
                GameObject poolBallObject = Instantiate(ball);
                poolBallObject.SetActive(false);
                poolBallObject.transform.SetParent(gameObject.transform);
                _pooledBalls.Add(poolBallObject);
            }
        }
    }
    public void CreateParticlePool()
    {
        for (int j = 0; j < 25; j++)
        {
            GameObject poolParticleObject = Instantiate(_particleSystemPrefab);
            poolParticleObject.SetActive(false);
            poolParticleObject.transform.SetParent(_particlePoolParent.transform);
            _pooledParticles.Add(poolParticleObject);
        }
    }

    public GameObject GetParticleSystem()
    {
        for (int i=0; i < _pooledParticles.Count; i++)
        {   if(!_pooledParticles[i].activeInHierarchy)
            {
                return _pooledParticles[i];
            }
        }
        return null;
    }

    public GameObject GetRandomPooledBall()
    {
        for (int i=0; i < _pooledBalls.Count; i++)
        {
            int randomNumber = Random.Range(0,_pooledBalls.Count);
            if(!_pooledBalls[randomNumber].activeInHierarchy && int.Parse(_pooledBalls[randomNumber].tag) <= _maximumBallTag && int.Parse(_pooledBalls[randomNumber].tag) >= _minimumBallTag)
            {
                ResetBallBooleans(_pooledBalls[randomNumber]);
                return _pooledBalls[randomNumber];
            }
        }
        return null;
    }
    public void ResetBallBooleans(GameObject ball)
    {
        Ball ballScript = ball.GetComponent<Ball>();
        ballScript.dropped = false;
        ballScript.VisitedToDrop = false;
        ballScript.VisitedToMerge = false;
    }

    public GameObject GetPooledBallByTag(int tag)
    {
        for (int i=0; i < _pooledBalls.Count; i++)
        {
            if(!_pooledBalls[i].activeInHierarchy && _pooledBalls[i].tag == tag.ToString())
            {
                return _pooledBalls[i];
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

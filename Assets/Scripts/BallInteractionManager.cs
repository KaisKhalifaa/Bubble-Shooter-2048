using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class BallInteractionManager : MonoBehaviour
{   
    public static BallInteractionManager Instance { get; private set; }

    BallShooter _ballShooter;
    ObjectPools _objectPooler;
    [SerializeField] List<GameObject> _connectedSimilarBalls = new List<GameObject>();
    [SerializeField] int _numberOfShotsBeforeNextRow = 3;
    [SerializeField] GameObject _ballPrefab;
    GameObject _ball;
    public bool BallMergingIsFinished = true, RowShouldSpawn = false, _ballVisitedForMerge=false, _popSoundShouldPlay=false;
    [SerializeField] int _spawnedRowsInCycle = 0;
    public float BallCollisionRadius;

    public GameObject _resultingBall;

    public int NumberOfShotsBeforeNextRow {private get { return _numberOfShotsBeforeNextRow; } set { _numberOfShotsBeforeNextRow = value; } }
    public GameObject ResultingBall { get {return _resultingBall; } private set { _resultingBall = value; }}
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        _ballShooter = BallShooter.Instance;
        _objectPooler = ObjectPools.Instance;
        BallCollisionRadius = _ballPrefab.GetComponent<CircleCollider2D>().radius + 0.2f;

    }
    void Update()
    {
        if (_ballShooter.BallArrived)
        {
            _ballShooter.BallArrived = false;
            _ball = _ballShooter.BallToShoot;
            StartCoroutine(RepeatMergeBallsAnimation(_ball));
        }
    }
    
    IEnumerator RepeatMergeBallsAnimation(GameObject ball)
    {   
        if (HasSimilarNeighbor(ball))
        {
            PutSimilarConnectedBallsInList(ball);
            yield return StartCoroutine(MergeConnectedBalls(int.Parse(ball.tag)));
            yield return StartCoroutine(RepeatMergeBallsAnimation(_resultingBall));
        }
        else
        {   
            //yield return StartCoroutine(MergeConnectedBalls(int.Parse(ball.tag))); // Ensure the last merge animation is completed
            BallMergingIsFinished = true;
            _spawnedRowsInCycle+=1;
            if (_spawnedRowsInCycle == _numberOfShotsBeforeNextRow)
            {
                RowShouldSpawn = true; 
                _spawnedRowsInCycle = 0;
            }
            _connectedSimilarBalls.Clear();
            yield return null;
        }
    }

    bool HasSimilarNeighbor(GameObject ball) 
    {
        var neighborsSimilarNumberColliders = Physics2D.OverlapCircleAll(ball.transform.position, BallCollisionRadius).ToList().FindAll(x => x.CompareTag(ball.tag) && x.gameObject != ball);

        if (!neighborsSimilarNumberColliders.Any())
        {
            return false;
        }
        else 
        return true;
    }

    void PutSimilarConnectedBallsInList(GameObject ball)
    {
        //List<GameObject> similarSurroundingBalls = new List<GameObject>();
        Collider2D[] neighborsColliders = Physics2D.OverlapCircleAll(ball.transform.position, BallCollisionRadius);
        Ball ballComponent = ball.GetComponent<Ball>();

        ballComponent.VisitedToMerge = true;

        foreach (Collider2D collider in neighborsColliders)
        {
            if (collider.gameObject.GetComponent<Ball>() != null)
            {
            _ballVisitedForMerge = collider.gameObject.GetComponent<Ball>().VisitedToMerge;
            }
            if (collider.CompareTag(ball.tag) && collider.gameObject != ball && !_ballVisitedForMerge)
            PutSimilarConnectedBallsInList(collider.gameObject);
            else
            continue;
        }

        _connectedSimilarBalls.Add(ball);
        //Debug.Log("ball added to list");
        ReorderListArrangement(_connectedSimilarBalls);
    }

    void ReorderListArrangement(List<GameObject> connectedSimilarBallsList)
    {
        int listBallTagNumber = int.Parse(connectedSimilarBallsList[0].gameObject.tag);
        int resultingBallNumber = Mathf.Min((int)(listBallTagNumber * Math.Pow(2, connectedSimilarBallsList.Count-1)), 2048);
        for (int i = 0; i < connectedSimilarBallsList.Count; i++)
        {
            Collider2D[] neighborsColliders = Physics2D.OverlapCircleAll(connectedSimilarBallsList[i].transform.position, BallCollisionRadius);
            
            foreach (Collider2D collider in neighborsColliders)
            {
                
                if (collider.gameObject.CompareTag(resultingBallNumber.ToString()))
                {
                    GameObject aux = connectedSimilarBallsList[0];
                    connectedSimilarBallsList[0] = connectedSimilarBallsList[i];
                    connectedSimilarBallsList[i] = aux;
                    break;
                }
            }
        }
    }

    IEnumerator MergeConnectedBalls(int ballTagNumber)
    {
        List<Coroutine> coroutines = new List<Coroutine>();
        
        if (_connectedSimilarBalls.Count > 0)
        {
            GameObject lastBallInConnectedBalls = _connectedSimilarBalls[0];

            foreach (GameObject ball in _connectedSimilarBalls)
            {   
                Coroutine coroutine = StartCoroutine(MoveCoroutine(ball, lastBallInConnectedBalls.transform.position, 0.3f));
                coroutines.Add(coroutine);
            }

            foreach (Coroutine coroutine in coroutines)
            {
                yield return coroutine;
            }

            foreach (GameObject ball in _connectedSimilarBalls)
            {
                ball.SetActive(false);
                ball.transform.SetParent(_objectPooler.transform);
                ball.GetComponent<Ball>().VisitedToMerge = false;
            }
                _popSoundShouldPlay = true;
                double connectedBallsAmount =_connectedSimilarBalls.Count;
                int resultingBallTag = Mathf.Min((int)(ballTagNumber * Math.Pow(2, connectedBallsAmount-1)), 2048);
                _resultingBall = _objectPooler.GetPooledBallByTag(resultingBallTag);
                _objectPooler.ActivateAndSetPooledObjectPosition(_resultingBall, lastBallInConnectedBalls.transform.position, 1f);
                _resultingBall.transform.SetParent(BallSpawner.Instance.BallContainer.transform);
                _connectedSimilarBalls.Clear();
                yield return new WaitForEndOfFrame();
            }
            }

        private IEnumerator MoveCoroutine(GameObject obj, Vector3 targetPosition, float duration)
        {
            GameObject particleSystem = _objectPooler.GetParticleSystem();
            _objectPooler.ActivateAndSetPooledObjectPosition(particleSystem, obj.transform.position, 1f);
            particleSystem.GetComponent<ParticleSystem>().Play();

            float timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(timeElapsed / duration);
                obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, t);
                yield return null;
        }
        obj.transform.position = targetPosition;
    }
    
}

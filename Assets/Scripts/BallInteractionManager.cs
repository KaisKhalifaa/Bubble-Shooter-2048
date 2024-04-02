
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class BallInteractionManager : MonoBehaviour
{   
        public static BallInteractionManager Instance { get; private set; }

    [SerializeField] BallShooter _ballShooter;
     GameObject _ball;
    [SerializeField] Transform objectPoolGameObject;
    bool _ballVisited = true;
    [SerializeField] List<GameObject> _connectedBalls = new List<GameObject>();
    static bool _coroutineIsFinished = false, _checker;

    static GameObject _resultingBall;
    
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
    }
    void Update()
    {
        if (_ballShooter.BallArrived)
        {
            _ball = _ballShooter.BallToShoot;
            StartCoroutine(BallAnimationRepeater(_ball));
            _ballShooter.BallArrived = false;
        }
        
    }
    
    IEnumerator BallAnimationRepeater(GameObject ball)
    {
        if (HasSimilarNeighbor(ball))
        {
            PutSimilarConnectedBallsInList(ball);
            yield return StartCoroutine(AnimateConnectedBalls(int.Parse(ball.tag)));
            StartCoroutine(BallAnimationRepeater(_resultingBall));
        }
    }

    bool HasSimilarNeighbor(GameObject ball) 
    {
        float ballCollisionRadius = ball.GetComponent<CircleCollider2D>().radius + 0.4f;
        var neighborsSimilarNumberColliders = Physics2D.OverlapCircleAll(ball.transform.position, ballCollisionRadius).ToList().FindAll(x => x.CompareTag(ball.tag) && x.gameObject != ball);

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
        float ballCollisionRadius = ball.GetComponent<CircleCollider2D>().radius + 0.4f;
        Collider2D[] neighborsColliders = Physics2D.OverlapCircleAll(ball.transform.position, ballCollisionRadius);
        Ball ballComponent = ball.GetComponent<Ball>();

        ballComponent.Visited = true;

        foreach (Collider2D collider in neighborsColliders)
        {
            if (collider.gameObject.GetComponent<Ball>() != null)
            {
            _ballVisited = collider.gameObject.GetComponent<Ball>().Visited;
            }
            if (collider.CompareTag(ball.tag) && collider.gameObject != ball && !_ballVisited)
            // bool ballVisited = collider.gameObject.GetComponent<Ball>().Visited;
            // if (!ballVisited)
            PutSimilarConnectedBallsInList(collider.gameObject);
            else
            continue;
        }

        _connectedBalls.Add(ball);
        //Debug.Log("ball added to list");
    }

    IEnumerator AnimateConnectedBalls(int ballTagNumber)
    {
        _coroutineIsFinished = false;
        List<Coroutine> coroutines = new List<Coroutine>();
        GameObject lastBallInConnectedBalls = _connectedBalls[0];
        // Start all coroutines simultaneously
        foreach (GameObject ball in _connectedBalls)
        {   
            Coroutine coroutine = StartCoroutine(MoveCoroutine(ball, lastBallInConnectedBalls.transform.position, 0.3f));
            coroutines.Add(coroutine);//el wa9t eli nhottou supposé lwakt ta l transition f 7arket l balls!!!!!!
        }

        // Wait for all coroutines to finish
        foreach (Coroutine coroutine in coroutines)
        {
            yield return coroutine;
        }

        // After all coroutines have finished, perform additional actions
        foreach (GameObject ball in _connectedBalls)
        {
            ball.SetActive(false);
            ball.transform.SetParent(objectPoolGameObject);
            ball.GetComponent<Ball>().Visited = false;
        }

            //look for the ball to replace the previously connected Balls
            double connectedBallsAmount =_connectedBalls.Count;
            int resultingBallTag = (int)(ballTagNumber * Math.Pow(2, connectedBallsAmount-1));
            _resultingBall = ObjectPool._instance.GetPooledBallByTag(resultingBallTag);
            ObjectPool._instance.ActivateAndSetPooledObjectPosition(_resultingBall, lastBallInConnectedBalls.transform.position, 1f);
            //resultingBall.transform.SetParent(BallContainer.transform);

            // Clear the list
            _connectedBalls.Clear();
            _coroutineIsFinished = true;
        }

    private IEnumerator MoveCoroutine(GameObject obj, Vector3 targetPosition, float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, t);
            yield return null;
        }
        obj.transform.position = targetPosition; // Ensure final position is exact
    }
}

 // IEnumerator AnimateConnectedBalls()
    // {
    //     for (int i = 0; i<= _connectedBalls.Count-1; i++)
    //     {
    //         yield return StartCoroutine(MoveCoroutine(_connectedBalls[i], _connectedBalls[0].transform.position, 0.2f));
    //         _connectedBalls[i].SetActive(false);
    //         _connectedBalls[i].transform.SetParent(objectPoolGameObject);
    //         _connectedBalls[i].GetComponent<Ball>().Visited = false;
    //     }
    //     _connectedBalls.Clear();
    // }
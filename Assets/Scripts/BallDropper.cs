using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropper : MonoBehaviour
{
    public static BallDropper Instance { get; private set; }
    BallSpawner _ballSpawner;
    BallInteractionManager _ballInteractionManager;

    [SerializeField] List<GameObject> _ballsInContainer = new List<GameObject>();
    public List<GameObject> BallsInContainer {get{ return _ballsInContainer;}}
    [SerializeField] List<GameObject> _ballsConnectedToCeiling = new List<GameObject>();
    public List<GameObject> BallsAttachedToCeiling = new List<GameObject>();

    bool _ballVisitedToDrop;
    bool _finishedConnectingBalls = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }
    void Start()
    {
        _ballSpawner = BallSpawner.Instance;
        _ballInteractionManager = BallInteractionManager.Instance;
    }
    void Update()
    {
        UpdateBallsInContainerList();

        if (_ballInteractionManager.BallMergingIsFinished)
        {
            StartCoroutine(HandleBallMergingFinish());
        }
    }

    IEnumerator HandleBallMergingFinish()
    {
        _ballInteractionManager.BallMergingIsFinished = false;
        ResetBallsConnectedToCeiling();

        for (int i = 0; i < 6; i++)
        {
            if (!BallsAttachedToCeiling[i].GetComponent<Ball>().VisitedToDrop)
            {
                PutConnectedBallsInList(BallsAttachedToCeiling[i]);
            }
        }
        yield return new WaitUntil(() => _finishedConnectingBalls);
        //yield return new WaitForEndOfFrame(); // Wait for end of frame to ensure the above operations are completed
        DropMidairBalls();
    }
    void UpdateBallsInContainerList()
    {
        if (BallShooter.Instance.BallArrived || _ballSpawner.BallRowSpawned || _ballInteractionManager.BallMergingIsFinished)
        {
            _ballsInContainer.Clear();
            for  (int i = 0; i<_ballSpawner.BallContainer.transform.childCount; i++)
            {
                GameObject ball = _ballSpawner.BallContainer.transform.GetChild(i).gameObject;
                if (ball.activeSelf)
                {
                    _ballsInContainer.Add(ball);
                }
            }
        }
    }

    void DropMidairBalls()
    {    
        foreach (GameObject ball in _ballsInContainer)
        {
            bool isConnectedToCeiling = ball.GetComponent<Ball>().VisitedToDrop;
            if(!isConnectedToCeiling)
            {
                Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
                ballRb.bodyType = RigidbodyType2D.Dynamic;
                ballRb.gravityScale = 2f;
                ball.GetComponent<Ball>().dropped = true;

            }
        }

        
    }
    void ResetBallsConnectedToCeiling()
    {
        foreach (GameObject ball in _ballsConnectedToCeiling)
        {
            ball.GetComponent<Ball>().VisitedToDrop = false;
        }
        _ballsConnectedToCeiling.Clear();
    }
    void PutConnectedBallsInList(GameObject ball)
    {
        
            //List<GameObject> similarSurroundingBalls = new List<GameObject>();
            if(ball.TryGetComponent(out CircleCollider2D collider2D))
            {
            _finishedConnectingBalls = false;
            float ballCollisionRadius = collider2D.radius + 0.1f;
            Collider2D[] neighborsColliders = Physics2D.OverlapCircleAll(ball.transform.position, ballCollisionRadius);
            Ball ballComponent = ball.GetComponent<Ball>();

            ballComponent.VisitedToDrop = true;

            foreach (Collider2D collider in neighborsColliders)
            {
                if (collider.gameObject.GetComponent<Ball>() != null)
                {
                _ballVisitedToDrop = collider.gameObject.GetComponent<Ball>().VisitedToDrop;
                }
                if (!_ballVisitedToDrop && !collider.CompareTag("ceiling") && !collider.CompareTag("border") && !collider.CompareTag("detachTrigger") && collider.gameObject != ball)
                PutConnectedBallsInList(collider.gameObject);
                else
                continue;
            }
            _ballsConnectedToCeiling.Add(ball);
            _finishedConnectingBalls = true;
            }
            
    }
}

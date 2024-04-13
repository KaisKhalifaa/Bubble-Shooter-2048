using System.Collections;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner Instance { get; private set; }

    BallShooter _ballShooter;
    ObjectPools _objectPooler;
    BallInteractionManager _ballInteractionManager;
    public GameObject BallContainer, BallShooterContainer;
    [SerializeField] Transform _referencePoint;
    [SerializeField] int _startingBlockNumberOfRows = 2;
    GameObject _ballToShoot, _nextBallToShoot; // the ball that will be shot is balltoshoot, the nextballtoshoot is the small ball beside it
    GameObject  _instantiatedBall;
    Vector3 _ballToShootPosition, _nextBallToShootPosition;
    static Vector3 _targetSpawnPosition;
    public bool StartingBlockSpawned = false, BallRowSpawned = false;
    float _ballSpawnOffset, _rowHorizontalOffset, _ballContainerMoveDistance, _timeDelayBetweenSpawningStartRows;
    int _ballQuantityPerRow;
    bool _rowToSpawnIsToTheRightSide = false;

    public int StartingNumberOfRows { private get{return _startingBlockNumberOfRows;} set{_startingBlockNumberOfRows = value;} }
    public GameObject BallToShoot {get { return _ballToShoot; } set { _ballToShoot = value;}}
    public GameObject NextBallToShoot {get { return _nextBallToShoot; } set { _nextBallToShoot = value;}}
    
    void Awake()
    {   

        if (Instance == null)
        {
            Instance = this;
        }

        _ballToShootPosition = new Vector3 (0f, -3.14f, 0f);
        _nextBallToShootPosition = new Vector3 (-0.88f, -3.14f, 0f);
        
        _timeDelayBetweenSpawningStartRows = 0.125f;

        _ballContainerMoveDistance = 0.612f;
        
        _rowHorizontalOffset = 0.36f;
        _ballSpawnOffset = 0.72f;
        _ballQuantityPerRow = 6;
        _targetSpawnPosition = _referencePoint.position;
        
    }

    void Start()
    {
        _ballShooter = BallShooter.Instance;
        _objectPooler = ObjectPools.Instance;
        _ballInteractionManager = BallInteractionManager.Instance;
        StartCoroutine(SpawnBallStartingBlock());
        InstantiateFirstBallToShoot();
        SpawnBallsToShoot();
    }

    void InstantiateFirstBallToShoot()
    {
        _nextBallToShoot = _objectPooler.GetRandomPooledBall();
        _objectPooler.ActivateAndSetPooledObjectPosition(_nextBallToShoot, _nextBallToShootPosition, 0.7f);
        _nextBallToShoot.transform.SetParent(BallShooterContainer.transform);
    }

    void Update()
    {

        if (_ballInteractionManager.RowShouldSpawn || SpecialEffects.Instance.BlockEmpty)
        {
            SpawnBallRow();
            _ballInteractionManager.RowShouldSpawn = false;
        }

        if (_ballShooter.BallIsShot)
        {
            _ballShooter.BallIsShot = false; // checks if the ballToshoot is already shot to assign the balltoshoot the nextballtoshoot
            StartCoroutine(WaitAndSpawnBallsToShoot(0.2f));
        }

    }
    IEnumerator WaitAndSpawnBallsToShoot(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        SpawnBallsToShoot();
    }

    void SpawnBallRow()
    {   
        BallDropper.Instance.BallsAttachedToCeiling.Clear();
        if(!_rowToSpawnIsToTheRightSide)
        {
            _targetSpawnPosition = new Vector3 (_targetSpawnPosition.x + _rowHorizontalOffset, _targetSpawnPosition.y,0);
        }
        else
        {
            _targetSpawnPosition = new Vector3 (_targetSpawnPosition.x, _targetSpawnPosition.y,0);
        }

        for (int i = 0; i < _ballQuantityPerRow; i++)
        {
            _instantiatedBall = _objectPooler.GetRandomPooledBall();
            _objectPooler.ActivateAndSetPooledObjectPosition(_instantiatedBall, _targetSpawnPosition, 1);
            _targetSpawnPosition = new Vector3(_targetSpawnPosition.x + _ballSpawnOffset, _referencePoint.position.y,0);
            _instantiatedBall.transform.SetParent(BallContainer.transform);
            BallDropper.Instance.BallsAttachedToCeiling.Add(_instantiatedBall);

            //Debug.Log(_targetSpawnPosition);
        }

        _rowToSpawnIsToTheRightSide = !_rowToSpawnIsToTheRightSide;
        //Debug.Log(_rowToSpawnIsToTheRightSide);
        _targetSpawnPosition = _referencePoint.position;
        StartCoroutine(MoveDownBallContainer());
        SpecialEffects.Instance.BlockEmpty = false;
        BallRowSpawned = true;
    }

    IEnumerator SpawnBallStartingBlock()
    {
        if (!StartingBlockSpawned){
        for (int i = 0; i < _startingBlockNumberOfRows ; i++)
        {
            SpawnBallRow();
            yield return new WaitForSeconds(_timeDelayBetweenSpawningStartRows);
        }    
        StartingBlockSpawned = true;
        }
    }

        public void SpawnBallsToShoot()
    {
        _ballToShoot = _nextBallToShoot;
        _ballToShoot.transform.position = _ballToShootPosition;
        _ballToShoot.transform.localScale = new Vector3 (1, 1, 1);
        //_ballToShoot.GetComponent<CircleCollider2D>().enabled = false;

        _nextBallToShoot = _objectPooler.GetRandomPooledBall();
        _objectPooler.ActivateAndSetPooledObjectPosition(_nextBallToShoot, _nextBallToShootPosition, 0.7f);
        _nextBallToShoot.transform.SetParent(BallShooterContainer.transform);
        _nextBallToShoot.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        _nextBallToShoot.GetComponent<CircleCollider2D>().enabled = false;
    }

    IEnumerator MoveDownBallContainer()
    {
        Vector3 startPosition = BallContainer.transform.position;
        Vector3 targetPosition = startPosition - Vector3.up * _ballContainerMoveDistance;
        float elapsedTime = 0f;

        while (elapsedTime < _timeDelayBetweenSpawningStartRows)
       {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / _timeDelayBetweenSpawningStartRows;
            BallContainer.transform.position = Vector3.Lerp(startPosition, targetPosition, percentageComplete);
            yield return null;
       }
        BallContainer.transform.position = targetPosition;
    }


}
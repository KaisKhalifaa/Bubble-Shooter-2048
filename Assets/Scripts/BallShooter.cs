using UnityEngine;

public class BallShooter : MonoBehaviour
{
    public static BallShooter Instance { get; private set; }

    [SerializeField] TouchManager _touchManager;
    [SerializeField] BallSpawner _ballSpawner;
    [SerializeField] BallInteractionManager _ballInteractionManager;
    [SerializeField] GameObject _ballToShoot, _nextBallToShoot;
    
    Vector3 _ballToShootPosition, _nextBallToShootPosition;
    public Vector2 _tapPosition;
    float _shotSpeed = 17f;
    [HideInInspector] public bool BallIsShot = false;
    [HideInInspector] public bool BallArrived = false;
    public GameObject BallToShoot {get { return _ballToShoot; }}
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _ballToShootPosition = new Vector3 (0f, -3.14f, 0f);
        _nextBallToShootPosition = new Vector3 (-0.88f, -3.14f, 0f);
    }
    void Start()
    {
        _ballInteractionManager = BallInteractionManager.Instance;
        _ballSpawner = BallSpawner.Instance;
    }
    void Update()
    {
        if (_touchManager.FingerReleased )
        {
            _touchManager.FingerReleased = false; // assigns the boolean responsible to shooting (fingerreleased) to false before shooting the ball
            ShootBall();
        }
        //_ballArrived = BallHasArrivedToBallBlock();
    }

    void ShootBall()
    {   
        _ballToShoot = _ballSpawner.BallToShoot; //takes the gameobject that the ballspawner has spawned
        _ballToShoot.GetComponent<CircleCollider2D>().enabled = true;
        Rigidbody2D rb = _ballToShoot.GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        //rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        Vector2 ballPosition = new Vector2(_ballToShoot.transform.position.x, _ballToShoot.transform.position.y);
        _tapPosition = _touchManager.TapPosition;
        Vector2 ballDirection = _tapPosition - ballPosition;
        ballDirection.Normalize();
        Vector2 velocity = ballDirection * _shotSpeed;
        rb.AddForce(velocity, ForceMode2D.Impulse);
        BallIsShot = true;
    }
    
}

using UnityEngine;

public class BallShooter : MonoBehaviour
{
    public static BallShooter Instance { get; private set; }

    [SerializeField] TouchManager _touchManager;
    [SerializeField] BallSpawner _ballSpawner;
    [SerializeField] GameObject _ballToShoot, _nextBallToShoot;
    
    Vector3 _ballToShootPosition, _nextBallToShootPosition;
    public Vector2 _tapPosition;
    float _shotSpeed = 17f;
    //bool _ballIsShot = false, _ballArrived = false;

    [HideInInspector] public bool BallIsShot = false;
    // {get { return _ballIsShot; } set { _ballIsShot = value;}}
    [HideInInspector] public bool BallArrived = false;
    //{get { return _ballArrived; } set { _ballArrived = value;}}
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
    void Update()
    {
        if (_touchManager.FingerReleased)
        {
            _touchManager.FingerReleased = false; // assigns the boolean responsible to shooting (fingerreleased) to false before shooting the ball
            ShootBall();
        }
        //_ballArrived = BallHasArrivedToBallBlock();
    }

    void ShootBall()
    {   
        _ballToShoot = _ballSpawner.BallToShoot; //takes the gameobject that the ballspawner has spawned
        Rigidbody2D rb = _ballToShoot.GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        //rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        Vector2 ballPosition = new Vector2(_ballToShoot.transform.position.x, _ballToShoot.transform.position.y);
        _tapPosition = _touchManager.TapPosition;
        Vector2 ballDirection = _tapPosition - ballPosition;
        ballDirection.Normalize();
        rb.AddForce(ballDirection * _shotSpeed, ForceMode2D.Impulse);
        BallIsShot = true;
    }

    void SnapBallToBlock()
    {

    }
    

    // public bool BallHasArrivedToBallBlock()
    //     {
    //         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _ballToShoot.GetComponent<CircleCollider2D>().radius);

    //         // Check if any colliders were found (excluding itself)
    //         if (colliders.Length > 0)
    //         {
    //             Debug.Log("ball Arrived!");
    //             return true;

    //         }
    //         else 
    //         {
    //             Debug.Log("nothing yet");
    //             return false;
    //         }
            
    //    }
    
}

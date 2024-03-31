
using UnityEngine;
using System.Linq;
public class BallInteractionManager : MonoBehaviour
{   
    [SerializeField] BallShooter _ballShooter;
    GameObject _ball;
    
    void Start()
    {
        _ballShooter = BallShooter.Instance;
        
    }
    void Update()
    {
        if (_ballShooter.BallArrived)
        {
            _ball = _ballShooter.BallToShoot;
            CheckSurroundingBallsNumbers(_ball,int.Parse(_ball.tag));
            _ballShooter.BallArrived = false;
        }
    }

    void CheckSurroundingBallsNumbers(GameObject ball,int number)
    {
        //List<GameObject> similarSurroundingBalls = new List<GameObject>();
        var colliders = Physics2D.OverlapCircleAll(ball.transform.position, ball.GetComponent<CircleCollider2D>().radius).ToList().FindAll(x=>
        x.CompareTag(ball.tag) && x.gameObject!=ball);

        foreach (Collider2D collider in colliders)
        {

            CheckSurroundingBallsNumbers(collider.gameObject, number*2); 
            // Add the ball to the list of surrounding balls
            //similarSurroundingBalls.Add(collider.gameObject);
        }
        Destroy(ball);
    }
}

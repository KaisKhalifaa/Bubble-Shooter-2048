using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] BallShooter _ballShooter;
    public bool Visited = false;

    void Start()
    {
        _ballShooter = BallShooter.Instance;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision occurred with: " + collision.gameObject.name);
        
        if (!collision.transform.CompareTag("border"))
        {
            _ballShooter.BallArrived = true;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.transform.SetParent(BallSpawner.Instance.BallContainer.transform);
        }
    }
}

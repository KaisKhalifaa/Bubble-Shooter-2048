using UnityEngine;

public class Ball : MonoBehaviour
{
    BallShooter _ballShooter;
    ObjectPools _objectPooler;
    public bool VisitedToMerge = false, IsAttachedToCeiling = false, VisitedToDrop = false, dropped = false;
    ParticleSystem _ballParticleSystem;

    void Start()
    {
        _ballParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        _objectPooler = ObjectPools.Instance;
        _ballShooter = BallShooter.Instance;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision occurred with: " + collision.gameObject.name);
        
        if (!collision.transform.CompareTag("border") && !collision.transform.CompareTag("ground"))
        {
            _ballShooter.BallArrived = true;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.transform.SetParent(BallSpawner.Instance.BallContainer.transform);
        }
        if (collision.transform.CompareTag("ground"))
        {
            
            GameObject particleSystem = _objectPooler.GetParticleSystem();
            _objectPooler.ActivateAndSetPooledObjectPosition(particleSystem, gameObject.transform.position, 1f);
            particleSystem.GetComponent<ParticleSystem>().Play();

            gameObject.SetActive(false);
            gameObject.transform.SetParent(ObjectPools.Instance.gameObject.transform);

        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ceiling"))
        {
            IsAttachedToCeiling = true;
        }
        if (other.CompareTag("detachTrigger"))
        {
            IsAttachedToCeiling = false;
        }

    }
}

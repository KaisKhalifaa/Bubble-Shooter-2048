using TMPro;
using UnityEngine;

public class SpecialEffects : MonoBehaviour
{
    public static SpecialEffects Instance { get; private set; }

    ObjectPools _objectPooler;
    BallInteractionManager _ballInteractionManager;
    public bool BlockEmpty = false;
    [SerializeField] TMP_Text _perfectNotification;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    void Start()
    {
        _objectPooler = ObjectPools.Instance;
        _ballInteractionManager = BallInteractionManager.Instance;
    }

    void Update()
    {
        PopSurroundingBalls();
        PerfectNotification();
    }
    void PopSurroundingBalls()
    {
        if(_ballInteractionManager.ResultingBall !=null && _ballInteractionManager.ResultingBall.activeSelf && _ballInteractionManager.ResultingBall.tag == "2048")
        {
            Collider2D[] neighborsColliders = Physics2D.OverlapCircleAll(_ballInteractionManager.ResultingBall.transform.position, _ballInteractionManager.BallCollisionRadius);
            foreach (Collider2D neighborCollider in neighborsColliders)
            {
                neighborCollider.gameObject.SetActive(false);
                neighborCollider.gameObject.transform.parent = _objectPooler.gameObject.transform;

                GameObject particleSystem = _objectPooler.GetParticleSystem();
                _objectPooler.ActivateAndSetPooledObjectPosition(particleSystem, neighborCollider.gameObject.transform.position, 1f);
                particleSystem.GetComponent<ParticleSystem>().Play();

            }
            _ballInteractionManager.ResultingBall.gameObject.SetActive(false);
            _ballInteractionManager.ResultingBall.gameObject.transform.parent = _objectPooler.transform;
        }
    }

    void PerfectNotification()
    {
        if (BallDropper.Instance.BallsInContainer.Count == 0 && BallSpawner.Instance.StartingBlockSpawned)
        {
            UITextEffects.Instance.FadeInOut(_perfectNotification);
            BlockEmpty = true;
        }
  
    }
}

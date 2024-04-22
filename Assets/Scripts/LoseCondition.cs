using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    [SerializeField] bool _gameOver;
    [SerializeField] Transform _ballContainerTransform;
    [SerializeField] GameObject _gameOverPanel, _touchManager;

    void Start()
    {
        _gameOver = false;
        
    }
    void Update()
    {
        if (_gameOver)
        {
            _gameOverPanel.SetActive(true);
            _touchManager.SetActive(false);
        }
    }
    void OnTriggerStay2D(Collider2D otherCollider)
    {
        Ball ball = otherCollider.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            if (otherCollider.gameObject.transform.parent == _ballContainerTransform && !ball.dropped)
            {
                _gameOver = true;
            }
        }
    }
}

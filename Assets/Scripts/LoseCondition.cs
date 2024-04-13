using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    public bool gameOver;
    public Transform ballContainerTransform;

    void OnTriggerStay2D(Collider2D otherCollider)
    {
        Ball ball = otherCollider.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            if (otherCollider.gameObject.transform.parent == ballContainerTransform && !ball.dropped)
            {
                gameOver = true;
            }
        }
    }
}

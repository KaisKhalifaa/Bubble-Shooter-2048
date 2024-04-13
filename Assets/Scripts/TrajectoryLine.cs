using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{    
    public LineRenderer lineRenderer;
    public Vector3 startPosition;
    public Vector3 endPosition;
    float lineLength = 3f;

    void Start()
    {
        startPosition = new Vector3 (0f,-3.15f,-0.1f);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        UpdateLine(TouchManager.Instance.TapPosition);
    }
    void Update()
    {
        UpdateLine(TouchManager.Instance.TapPosition);
    }
        void UpdateLine(Vector3 endPosition)
    {
        Vector3 direction = (endPosition - startPosition).normalized;
        Vector3 adjustedEndPosition = startPosition + direction * lineLength;
        lineRenderer.SetPosition(1, adjustedEndPosition);
    }
}

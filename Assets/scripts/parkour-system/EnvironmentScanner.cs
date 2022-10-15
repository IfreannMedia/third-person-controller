using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] private Vector3 forwardRayOffset = new Vector3(0, .25f, 0);
    [SerializeField] private float forwardRayLength = 0.8f;
    [SerializeField] private LayerMask obstacleLayer;
    public void ObstacleCheck()
    {
        Vector3 forwardOrigin = transform.position + forwardRayOffset;
        bool hitFound = Physics.Raycast(forwardOrigin, transform.forward, out RaycastHit hitInfo,
            forwardRayLength, obstacleLayer);
        Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength, hitFound ? Color.green : Color.red);
    }
}

using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] private Vector3 forwardRayOffset = new Vector3(0, .25f, 0);
    [SerializeField] private float forwardRayLength = 0.8f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float heightRayLength = 5f;
    [SerializeField] private float ledgeRayLength = 10f;
    [SerializeField] private float ledgeHeightThreshold = .75f;

    public ObstacleHitData ObstacleCheck()
    {
        ObstacleHitData hitData = new ObstacleHitData();
        Vector3 forwardOrigin = transform.position + forwardRayOffset;
        hitData.forwardHitFound = Physics.Raycast(forwardOrigin, transform.forward, out hitData.forwardHit,
            forwardRayLength, obstacleLayer);
        Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength, hitData.forwardHitFound ? Color.green : Color.red);

        if (hitData.forwardHitFound)
        {
            Vector3 heightOrigin = hitData.forwardHit.point + Vector3.up * heightRayLength;
            hitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitData.heightHit, heightRayLength, obstacleLayer);
            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, hitData.heightHitFound ? Color.green : Color.red);
        }

        return hitData;
    }

    public bool LedgeCheck(Vector3 moveDir, out LedgeData ledgeData)
    {
        ledgeData = new LedgeData();
        if (moveDir == Vector3.zero) return false;

        float originOffset = 0.5f;
        Vector3 origin = transform.position + moveDir * originOffset + Vector3.up;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, ledgeRayLength, obstacleLayer))
        {
            Debug.DrawRay(origin, Vector3.down * ledgeRayLength, Color.green);

            Vector3 surfaceRayOrigin = transform.position + moveDir - new Vector3(0, 0.1f, 0);

            if (Physics.Raycast(surfaceRayOrigin, -moveDir, out RaycastHit surfaceHit, 2, obstacleLayer))
            {
                float height = transform.position.y - hit.point.y;

                if (height > ledgeHeightThreshold)
                {
                    ledgeData.angle = Vector3.Angle(transform.forward, surfaceHit.normal);
                    ledgeData.height = height;
                    ledgeData.surfaceHit = surfaceHit;
                    return true;
                }
            }

            //float height = transform.position.y - hit.point.y;

            //if (height > ledgeHeightThreshold)
            //{
            //    return true;
            //}
        }

        return false;
    }
}

public struct ObstacleHitData
{
    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;
}

public struct LedgeData
{
    public float height;
    public float angle;
    public RaycastHit surfaceHit;
}

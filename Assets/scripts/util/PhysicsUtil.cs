using UnityEngine;
using System.Collections.Generic;

public class PhysicsUtil
{

    public static bool ThreeRaycasts(Vector3 origin, Vector3 dir, float spacing, Transform transform,
        out List<RaycastHit> hits, float distance, LayerMask layer, bool debug=false)
    {
        bool centreHitFound = Physics.Raycast(origin, Vector3.down, out RaycastHit centreHit, distance, layer);
        bool leftHitFound = Physics.Raycast(origin - transform.right * spacing, Vector3.down, out RaycastHit leftHit, distance, layer);
        bool rightHitFound = Physics.Raycast(origin + transform.right * spacing, Vector3.down, out RaycastHit rightHit, distance, layer);

        hits = new List<RaycastHit>() { centreHit, leftHit, rightHit };


        bool hitfound = centreHitFound || leftHitFound || rightHitFound;

        if(hitfound && debug)
        {
            Debug.DrawLine(origin, centreHit.point, Color.green);
            Debug.DrawLine(origin - transform.right * spacing, leftHit.point, Color.green);
            Debug.DrawLine(origin + transform.right * spacing, rightHit.point, Color.green);
        }

        return hitfound;
    }
}

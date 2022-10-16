using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/Custom Actions/New vault action")]
public class VaultAction : ParkourAction
{
    public override bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        if (!base.CheckIfPossible(hitData, player)) return false;

        Vector3 hitPointInLocalSpace = hitData.forwardHit.transform.InverseTransformPoint(hitData.forwardHit.point);

        // mirror if approaching from back and left, or front and right
        if (hitPointInLocalSpace.z < 0 && hitPointInLocalSpace.x < 0 || hitPointInLocalSpace.z > 0 && hitPointInLocalSpace.x > 0)
        {
            Mirror = true;
            matchBodyPart = AvatarTarget.RightHand;
        }
        else
        {
            Mirror = false;
            matchBodyPart = AvatarTarget.LeftHand;
        }

        return true;
    }
}

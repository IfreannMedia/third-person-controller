using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    [SerializeField] string animName;

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] bool rotateTowardsObstacle;

    public Quaternion TargetRotation { get; set; }

    public bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        float height = hitData.heightHit.point.y - player.transform.position.y;
        if (height < minHeight || height > maxHeight)
            return false;

        // if we should rotate towards the obstacle, then the target rotastion is the opposite of the normal of the hit object
        if (rotateTowardsObstacle)
            TargetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);
            
        
        return true;
    }

    public string AnimName => this.animName;
    public bool RotateTowards => this.rotateTowardsObstacle;

}

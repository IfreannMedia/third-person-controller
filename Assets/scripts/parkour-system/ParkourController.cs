using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    [SerializeField] List<ParkourAction> parkourActinos;

    bool inAction = false;
    EnvironmentScanner environmentScanner;
    Animator animator;
    PlayerController playerController;

    private void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButton("Jump") && !inAction)
        {
            ObstacleHitData hitData = environmentScanner.ObstacleCheck();
            if (hitData.forwardHitFound)
            {

                foreach (ParkourAction action in parkourActinos)
                {
                    if (action.CheckIfPossible(hitData, this.transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        break;
                    }
                }
            }
        }

    }

    IEnumerator DoParkourAction(ParkourAction action)
    {
        inAction = true;
        playerController.HasControl(false);
        // set up next animation
        animator.SetBool("mirror", action.Mirror);
        animator.CrossFade(action.AnimName, .02f);
        // wait for end of frame
        yield return null;
        // get info of animation that will play
        AnimatorStateInfo stateInfo = animator.GetNextAnimatorStateInfo(0);
        if (!stateInfo.IsName(action.AnimName))
        {
            Debug.LogError("incorrect animation name given: " + action.AnimName);
        }

        // wait for end of animation + smoothly rotate toward obstacle
        float timer = 0f;
        while (timer <= stateInfo.length)
        {
            timer += Time.deltaTime;

            // rotate player towards obstacle
            if (action.RotateTowards)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation, playerController.RotationSpeed);

            if (action.EnableTargetMatching)
                MatchTarget(action);

            if (animator.IsInTransition(0) && timer >= .5f)
                break;

            yield return null;
        }

        yield return new WaitForSeconds(action.PostActionDelay);

        // set inAction to false, we can listen again for user input
        inAction = false;
        playerController.HasControl(true);
    }

    void MatchTarget(ParkourAction action)
    {
        if (animator.isMatchingTarget) return;
        animator.MatchTarget(action.MatchPos, transform.rotation, action.MatchBodyPart, new MatchTargetWeightMask(action.MatchPosWeight, 0),
            action.MatchStartTime, action.MatchTargetTime);
    }
}
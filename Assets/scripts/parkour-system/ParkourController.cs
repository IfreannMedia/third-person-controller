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
        animator.CrossFade(action.AnimName, .2f);
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

            if (action.RotateTowards)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation, playerController.RotationSpeed);
            // rotate player towards obstacle
            yield return null;
        }
        // set inAction to false, we can listen again for user input
        inAction = false;
        playerController.HasControl(true);
    }
}
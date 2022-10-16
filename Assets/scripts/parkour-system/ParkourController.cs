using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
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
                StartCoroutine("DoParkourAction");
            }
        }

    }

    IEnumerator DoParkourAction()
    {
        inAction = true;
        playerController.HasControl(false);
        // set up next animation
        animator.CrossFade("stepUp", .2f);
        // wait for end of frame
        yield return null;
        // get info of animation that will play
        AnimatorStateInfo stateInfo = animator.GetNextAnimatorStateInfo(0);
        // wait for end of animation
        yield return new WaitForSeconds(stateInfo.length);
        // set inAction to false, we can listen again for user input
        inAction = false;
        playerController.HasControl(true);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{

    EnvironmentScanner environmentScanner;

    private void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
    }

    private void Update()
    {
        ObstacleHitData hitData = environmentScanner.ObstacleCheck();
        if (hitData.forwardHitFound)
        {
            Debug.Log("hit: " + hitData.forwardHit.transform.name);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    Transform target;

    void Update()
    {
        if (target)
        {
            transform.position = target.position;
        }
        else
        {
            Debug.LogWarning("FOLLOWCAMERA UPDATE: no target!");
        }
    }
}

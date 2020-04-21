using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (agent)
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }
        else
        {
            Debug.LogWarning("MOVER: Update: No agent!");
        }
        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            agent.destination = hit.point;
        }
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }
}

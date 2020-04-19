using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField]
    Transform target;
    NavMeshAgent agent;
    float agentSpeed;
    Ray lastRay;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agentSpeed = agent.speed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent && target)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                agent.destination = target.position;
                agent.speed = agentSpeed;
            }
        }
        else
        {
            Debug.LogWarning("MOVER: Update: No agent or target");
        }
        Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
    }
}

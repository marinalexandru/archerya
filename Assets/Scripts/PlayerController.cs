﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent Agent;
    private Camera Camera;
    // private List<Collider> Enemies;
    private GameObject Target;
    private PlayerAnnimationController PlayerAnnimationController;
    private ShooterController ShooterController;



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Camera = Camera.main;
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        PlayerAnnimationController = GetComponent<PlayerAnnimationController>();
        ShooterController = GetComponent<ShooterController>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void Update()
    {
        ShooterController.SetTarget(Target);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                MarkTargets(hit);
            }
        }

        if (AgentReachedDestination())
        {
            PlayerAnnimationController.AnimateIdle();
        }
    }

    private void MarkTargets(RaycastHit hit)
    {
        if (hit.transform.gameObject.tag.Equals("Enemy"))
        {
            Target = hit.transform.gameObject;
        }
        else
        {
            Target = null;
            Agent.SetDestination(hit.point);
            PlayerAnnimationController.AnimateMove();
        }

    }

    public bool AgentReachedDestination()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

}

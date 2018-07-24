﻿using UnityEngine;
using UnityEngine.AI;

public class ShooterController : MonoBehaviour
{
    [HeaderAttribute("3DParts")]
    public GameObject Weapon;
    public GameObject HitZone;
    public GameObject Projectile;
    [HeaderAttribute("Attack:")]
    public float Range = 5.0f;
    NavMeshAgent Agent;
    GameObject Target;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public void Start()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void Update()
    {
        if (Target != null)
        {
            if (Vector3.Distance(this.transform.position, Target.transform.position) <= Range)
            {
                // stop movement because we can attack
                Agent.ResetPath();
                RotateTowardsTarget();
            }
            else
            {
                // follow enemy
                Agent.SetDestination(Target.transform.position);
            }
        }
    }

    public void SetTarget(GameObject Target)
    {
        this.Target = Target;
    }

    public void ShootSpell()
    {
        ShootProjectile();
    }

    void RotateTowardsTarget()
    {
        Vector3 distanceVector = Target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(distanceVector);
        Quaternion stepRotation = Quaternion.Slerp(transform.rotation, targetRotation,
                            Constants.ANGULAR_LOCK_IN_SPEED * Time.deltaTime);
        stepRotation.eulerAngles.Set(0, stepRotation.eulerAngles.y, 0);
        transform.rotation = stepRotation;
    }

    void ShootProjectile()
    {
        GameObject projectileReference = Instantiate(Projectile, Weapon.transform.position, Weapon.transform.rotation);
        ShooterController gameCombatantController = Target.gameObject.GetComponent<ShooterController>();
        projectileReference.SendMessage("SetTarget", gameCombatantController.HitZone, SendMessageOptions.RequireReceiver);
        projectileReference.SendMessage("SetSpeed", 20, SendMessageOptions.RequireReceiver);

    }

    bool CanShootTarget()
    {
        if (Target != null && Vector3.Distance(this.transform.position, Target.transform.position) <= Range)
        {
            return true;
        }
        return false;
    }

}

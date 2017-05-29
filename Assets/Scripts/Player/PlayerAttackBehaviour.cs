﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerAttackBehaviour : StateMachineBehaviour
{

    private float InitialSpeed = 0f;
    public event OnProjectileAttackStarted OnProjectileAttackStartedEvent;
    public event OnProjectileFire OnProjectileFireEvent;
    public delegate void OnProjectileAttackStarted();
    public delegate void OnProjectileFire();
	private bool ActionDelegated = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitialSpeed = animator.speed;
        animator.speed = 1.5f;
        if (OnProjectileAttackStartedEvent != null)
        {
            OnProjectileAttackStartedEvent();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float normalisedDuration = stateInfo.normalizedTime % 1;
        if (normalisedDuration > 0.4f && !ActionDelegated)
        {
			ActionDelegated = true;
            if (OnProjectileFireEvent != null)
            {
                OnProjectileFireEvent();
            }
        }

        if (normalisedDuration <0.4f)
        {
            ActionDelegated = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = InitialSpeed;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}

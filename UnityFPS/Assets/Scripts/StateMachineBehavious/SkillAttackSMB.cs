using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.isShootingMagic = true;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

        if (m_TMonoBehavious.CheckForGrounded())
        {
            m_TMonoBehavious.SetMoveVector(Vector2.zero);
            m_TMonoBehavious.GroundedVerticalMovement();
        }
        else
        {
            m_TMonoBehavious.UpdateJump();
            m_TMonoBehavious.AirborneHorizonalMovement(true);
            m_TMonoBehavious.AirborneVerticalMovement();
        }
       
        
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.isShootingMagic = false;
    }
}

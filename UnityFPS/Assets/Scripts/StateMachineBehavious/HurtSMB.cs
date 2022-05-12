using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.SetMoveVector(m_TMonoBehavious.GetHurtDirection() * m_TMonoBehavious.hurtJumpSpeed);
        m_TMonoBehavious.StartFlickering();
    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_TMonoBehavious.IsFalling())
        {
            m_TMonoBehavious.CheckForGrounded();
           // m_TMonoBehavious.UpdateJump();
        }
        
        m_TMonoBehavious.AirborneVerticalMovement();
    }

}

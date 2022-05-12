using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_TMonoBehavious.SetMoveVector(m_TMonoBehavious.GetHurtDirection() * m_TMonoBehavious.hurtJumpSpeed);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.CheckForGrounded();
        m_TMonoBehavious.AirborneVerticalMovement();
    }
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.SetMoveVector(Vector2.zero);
    }
}

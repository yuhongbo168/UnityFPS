using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneMeleeAttackSMB : ScenceLinkSMB<Character>
{
    public string footDownEffect;
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStatePostEnter(animator, stateInfo, layerIndex);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        m_TMonoBehavious.UpdateJump();
        m_TMonoBehavious.AirborneHorizonalMovement(true);
        m_TMonoBehavious.AirborneVerticalMovement();
        m_TMonoBehavious.CheckForGrounded();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        if (m_TMonoBehavious.CheckForGrounded())
        {
            m_TMonoBehavious.ApplyFootDownEffect(footDownEffect);
            m_TMonoBehavious.canLunchjumpUpEffect = true;

        }

        m_TMonoBehavious.DisableMeleeAttack();
    }
}

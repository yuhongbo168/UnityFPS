using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackSMB : ScenceLinkSMB<Character>
{

   
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStatePostEnter(animator, stateInfo, layerIndex);

        
    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        m_TMonoBehavious.UpdateFacing();
        m_TMonoBehavious.CheckForGrounded();
        m_TMonoBehavious.GroundedHorizonalMovement(true,1f);
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStatePreExit(animator, stateInfo, layerIndex);
        m_TMonoBehavious.DisableMeleeAttack();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChianSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);
        m_TMonoBehavious.SetVerticalMovement(0);
    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_TMonoBehavious.SetVerticalMovement(0);
        m_TMonoBehavious.GroundedHorizonalMovement(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class OnChianSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);
        m_TMonoBehavious.SetVerticalMovement(0);
        m_TMonoBehavious.SetAirborneHorizonalMovement(0f);

    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.UpdateFacingSlid();

        m_TMonoBehavious.RunAtChain();
        m_TMonoBehavious.SetAirborneHorizonalMovement(0f);
        m_TMonoBehavious.SetVerticalMovement(0);


        if (m_TMonoBehavious.OnChainJump)
        {
           m_TMonoBehavious.SlidChian = false;
        }
        // m_TMonoBehavious.UpdateJump();

    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
         m_TMonoBehavious.UpdateJump();
    }
}

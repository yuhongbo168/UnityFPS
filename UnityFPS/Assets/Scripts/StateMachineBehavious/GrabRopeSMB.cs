using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRopeSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.UpdateFacing();
        //m_TMonoBehavious.UpdateJump();
        m_TMonoBehavious.AirborneHorizonalMovement(true);

        m_TMonoBehavious.CheckGrab();

        //m_TMonoBehavious.AirborneVerticalMovement();
        
        //m_TMonoBehavious.GrabRope();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_TMonoBehavious.UpdateFacing();
        m_TMonoBehavious.GroundedHorizonalMovement(true,0.2f);
        m_TMonoBehavious.GroundedVerticalMovement();
        m_TMonoBehavious.CheckForGrounded();

        m_TMonoBehavious.MovePushable();
    }
}

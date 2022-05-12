using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CrouchingSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_TMonoBehavious.UpdateFacing();
        m_TMonoBehavious.CheckForCrouching();
        m_TMonoBehavious.CheckForGrounded();
        m_TMonoBehavious.GroundedHorizonalMovement(false);
        if (m_TMonoBehavious.CheckForFallInput())
        {
            m_TMonoBehavious.MakePlatformFallthrough();
        }
        m_TMonoBehavious.GroundedVerticalMovement();
    }
}

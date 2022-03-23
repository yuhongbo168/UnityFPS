using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AirborneSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_TMonoBehavious.UpdateFacing();
        m_TMonoBehavious.UpdateJump();
        m_TMonoBehavious.AirborneVerticalMovement();
        m_TMonoBehavious.AirborneHorizonalMovement(true);
        m_TMonoBehavious.CheckForGrounded();
    }
}

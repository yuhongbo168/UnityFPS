using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locomationSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.TeleportToColliderBottom();
        m_TMonoBehavious.CanMeleeAttack = true;
    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.UpdateFacing();
        m_TMonoBehavious.GroundedHorizonalMovement(true);
        m_TMonoBehavious.GroundedVerticalMovement();
        m_TMonoBehavious.CheckForGrounded();
        m_TMonoBehavious.CheckForCrouching();
     

        if (m_TMonoBehavious.CheckSecounJump())
        {
            m_TMonoBehavious.SetVerticalMovement(0);
        }

       
        //if (m_TMonoBehavious.CheckForJumpInput())
        //{
        //    m_TMonoBehavious.SetVerticalMovement(m_TMonoBehavious.jumpSpeed);
        //}

    }
}



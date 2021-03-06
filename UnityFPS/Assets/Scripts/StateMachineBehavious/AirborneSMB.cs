using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AirborneSMB : ScenceLinkSMB<Character>
{
    
    public string footUpEffect;
    public string footDownEffect;

    private int jumpCount = 1;
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);
        //Debug.Log("StartJump");
        // m_TMonoBehavious.ApplyFootEffect(footUpEffect);
        
    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_TMonoBehavious.UpdateFacing();
        m_TMonoBehavious.UpdateJump();
        m_TMonoBehavious.AirborneVerticalMovement();
        m_TMonoBehavious.AirborneHorizonalMovement(true);


        if (m_TMonoBehavious.CheckForSeconJumpInut() && jumpCount > 0)
        {
            jumpCount--;
            //m_TMonoBehavious.SetVerticalMovement(0);
            m_TMonoBehavious.SetVerticalMovement(m_TMonoBehavious.jumpSpeed);
        }

        m_TMonoBehavious.CheckBounceJump();


        m_TMonoBehavious.CheckForGrounded();

        m_TMonoBehavious.FindRope();

       // m_TMonoBehavious.GrabRope();
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);
        if (m_TMonoBehavious.CheckForGrounded())
        {
            m_TMonoBehavious.ApplyFootDownEffect(footDownEffect);
            // Debug.Log("footDownEffect");
            m_TMonoBehavious.canLunchjumpUpEffect = true;
        }
       
        jumpCount = 1;
        m_TMonoBehavious.CheckStopChian();
       
    }
}

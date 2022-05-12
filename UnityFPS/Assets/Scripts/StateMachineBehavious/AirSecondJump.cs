﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSecondJump : ScenceLinkSMB<Character>
{

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        m_TMonoBehavious.SetVerticalMovement(0);
    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // m_TMonoBehavious.UpdateJump();
        

        m_TMonoBehavious.SecondBounceJumpVerticlMovment();    
     
        m_TMonoBehavious.CheckForGrounded();
        
    }
}

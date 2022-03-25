﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CrouchingSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_TMonoBehavious.UpdateFacing();
        m_TMonoBehavious.CheckForCrouching();
    }
}
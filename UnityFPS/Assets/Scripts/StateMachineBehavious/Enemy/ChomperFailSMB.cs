﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperFailSMB : ScenceLinkSMB<EnemyBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.UpdateJump();
    }
}

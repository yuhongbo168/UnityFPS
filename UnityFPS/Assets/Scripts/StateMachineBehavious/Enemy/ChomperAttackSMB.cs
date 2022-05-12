using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperAttackSMB : ScenceLinkSMB<EnemyBehaviour>
{

 
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        m_TMonoBehavious.SetHorizontalSpeed(0);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.GroundedVerticalMovement();
    }
}

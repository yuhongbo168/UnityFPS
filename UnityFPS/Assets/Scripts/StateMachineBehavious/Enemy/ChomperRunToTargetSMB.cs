using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperRunToTargetSMB : ScenceLinkSMB<EnemyBehaviour>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);

        m_TMonoBehavious.OrientToTarget();
        
     
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        m_TMonoBehavious.ChekTargetStillVisible();
        m_TMonoBehavious.CheckMeleeAttack();

        float amount = m_TMonoBehavious.speed * 2f;
        if (m_TMonoBehavious.CheckForObstacle(amount))
        {
            m_TMonoBehavious.ForgetTarget();
        }
        else
        {
            m_TMonoBehavious.SetHorizontalSpeed(amount);
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        m_TMonoBehavious.SetHorizontalSpeed(0);
    }
}

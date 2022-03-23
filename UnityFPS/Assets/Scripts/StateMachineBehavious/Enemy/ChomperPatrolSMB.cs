using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ChomperPatrolSMB : ScenceLinkSMB<EnemyBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float dist = m_TMonoBehavious.speed;
        if (m_TMonoBehavious.CheckForObstacle(dist))
        {
            m_TMonoBehavious.SetHorizontalSpeed(dist);
            Debug.Log("CheckForObstacle");
        }
        else
        {
            m_TMonoBehavious.SetHorizontalSpeed(dist);
        }
    }
}

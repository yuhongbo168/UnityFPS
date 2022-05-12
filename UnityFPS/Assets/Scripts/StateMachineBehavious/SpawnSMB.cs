using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSMB : ScenceLinkSMB<Character>
{


    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);
        m_TMonoBehavious.gameObject.SetActive(true);
    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        m_TMonoBehavious.CheckForGrounded();
        m_TMonoBehavious.GroundedHorizonalMovement(false);
        
      
    }
}

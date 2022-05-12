using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabUPSMB : ScenceLinkSMB<Character>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.GrapUP();
    }
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_TMonoBehavious.GrabRope();
        // m_TMonoBehavious.GrapUP();
        

    }
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);
        
    }
}

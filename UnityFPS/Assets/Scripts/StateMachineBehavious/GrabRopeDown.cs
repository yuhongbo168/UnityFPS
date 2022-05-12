using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRopeDown : ScenceLinkSMB<Character>
{

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        
    }
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_TMonoBehavious.GrabDown();
    }

}

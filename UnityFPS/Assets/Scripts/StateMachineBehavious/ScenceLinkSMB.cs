using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ScenceLinkSMB<TMonoBehavious> : SealedSMB
    where TMonoBehavious:MonoBehaviour
{
    protected TMonoBehavious m_TMonoBehavious;

    bool m_FirstFrameHappend;
    bool m_LastFrameHappened;

    public static void Initialise(Animator animation,TMonoBehavious newMono)
    {
        ScenceLinkSMB<TMonoBehavious>[] sceneLink = animation.GetBehaviours<ScenceLinkSMB<TMonoBehavious>>();

        for (int i = 0; i < sceneLink.Length; i++)
        {
            sceneLink[i].InternalInitialise(animation, newMono);
        }
    }
    protected void InternalInitialise(Animator animation,TMonoBehavious monoBehavious)
    {
        m_TMonoBehavious = monoBehavious;
        OnStart(animation);
    }
    // Start is called before the first frame update

    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_FirstFrameHappend = false;

        OnSLStateEnter(animator, stateInfo, layerIndex);
        OnSLStateEnter(animator, stateInfo, layerIndex, controller);
    }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (!animator.gameObject.activeSelf)
        {
            return;
        }

        

        if (animator.IsInTransition(layerIndex)&&animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash)
        {
            OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex);
            OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex, controller);
        }

        if (animator.IsInTransition(layerIndex) && !m_FirstFrameHappend)
        {
            m_FirstFrameHappend = true;

            OnSLStatePostEnter(animator, stateInfo, layerIndex);
            OnSLStatePostEnter(animator, stateInfo, layerIndex, controller);
        }

        if (!animator.IsInTransition(layerIndex) && m_FirstFrameHappend)
        {
            OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
            OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex,controller);
        }

        if (animator.IsInTransition(layerIndex) && !m_LastFrameHappened && m_FirstFrameHappend)
        {
            m_LastFrameHappened = true;
            OnSLStatePreExit(animator, stateInfo, layerIndex);
            OnSLStatePreExit(animator, stateInfo, layerIndex, controller);
        }

        if (!animator.IsInTransition(layerIndex) && !m_FirstFrameHappend)
        {
            m_FirstFrameHappend = true;
            OnSLStatePostEnter(animator, stateInfo, layerIndex);
            OnSLStatePostEnter(animator, stateInfo, layerIndex, controller);
        }

        if (animator.IsInTransition(layerIndex)&&animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash==stateInfo.fullPathHash)
        {
            OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex);
            OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex, controller);
        }

    }

    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_LastFrameHappened = false;

        OnSLStateExit(animator, stateInfo, layerIndex);
        OnSLStateExit(animator, stateInfo, layerIndex, controller);
    }

    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {

    }

    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {

    }

    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){ }
    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }



    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }
    public virtual void OnStart(Animator newAnimation)
    {

    }
}

public abstract class SealedSMB : StateMachineBehaviour
{
    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}

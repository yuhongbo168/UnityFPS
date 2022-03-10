using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ScenceLinkSMB<TMonoBehavious> : SealedSMB
    where TMonoBehavious:MonoBehaviour
{
    protected MonoBehaviour m_TMonoBehavious;


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
    void Start()
    {
        
    }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex, controller);
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

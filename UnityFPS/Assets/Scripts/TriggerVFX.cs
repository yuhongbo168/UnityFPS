using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerVFX : StateMachineBehaviour
{
    public string vfxName;
    public Vector3 offset = Vector3.zero;
    public bool attachToParent = false;
    public float startDelay = 0;
    public bool OnEnter = true;
    public bool OnExit = false;
    int m_VfxId;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnEnter)
        {
            
            Trigger(animator.transform);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnExit)
        {
            Trigger(animator.transform);
        }
    }
    void Trigger(Transform transform)
    {
        //Debug.Log(transform.position);
        var flip = false;
        // var spriteRender = transform.GetComponent<SpriteRenderer>();
        //         if (spriteRender)
        //         {
        //             flip = spriteRender.flipX;
        //         }

        flip = transform.localScale.x < 1;
        VFXController.Instance.Trigger(vfxName, offset, startDelay, flip, attachToParent ? transform : null);
    }
}

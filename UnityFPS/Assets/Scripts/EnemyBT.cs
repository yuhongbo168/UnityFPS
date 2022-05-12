using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBT : MonoBehaviour
{
    public EnymeBehavioursTest enymeBehaviour;
    RootA ai = BTA.Root();

    private void OnEnable()
    {
        ai.OpenBranch(

            BTA.If(()=>enymeBehaviour.Target!=null).OpenBranch
            (
             
                BTA.Call(enymeBehaviour.CheckTargetStillVisible),
                BTA.Call(enymeBehaviour.RememberTargetPos),
                BTA.Call(enymeBehaviour.Fire)
                
             ),

            BTA.If(()=>enymeBehaviour.Target==null).OpenBranch
            (
                BTA.Call(enymeBehaviour.StopFire),
                BTA.Call(enymeBehaviour.ScanTarget)
                
             )



            );
    }

    private void Update()
    {
        ai.Tick();
    }
}

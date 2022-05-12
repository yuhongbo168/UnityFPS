using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTest : MonoBehaviour
{
    Root01 m_Ai = BTtest.Root();
   
    public EnymeBehavioursTest enemyBehaviour;

    

    private void OnEnable()
    {
        m_Ai.OpenBranch(

            BTtest.If(()=>enemyBehaviour.Target!=null).OpenBranch
                (
                    BTtest.Call(enemyBehaviour.CheckTargetStillVisible),
                     BTtest.Call(enemyBehaviour.RememberTargetPos),
                     BTtest.Call(enemyBehaviour.Fire)

                ),
            

            BTtest.If(()=>enemyBehaviour.Target==null).OpenBranch
            (
                BTtest.Call(enemyBehaviour.StopFire),
                BTtest.Call(enemyBehaviour.ScanTarget)
                

             )

            );
    }

    public void Update()
    {

        m_Ai.Tick();

    }

}

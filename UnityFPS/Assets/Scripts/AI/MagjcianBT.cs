using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagjcianBT : MonoBehaviour
{
    Animator m_Animator;
    Damabeable m_Damageable;
    Root m_Ai = BT.Root();
    EnemyBehaviour m_EnemyBehaviour;

    private void OnEnable()
    {
        m_EnemyBehaviour = GetComponent<EnemyBehaviour>();
        m_Animator = GetComponentInChildren<Animator>();

        m_Ai.OpenBranch(

             BT.If(() => { return m_EnemyBehaviour.target != null; }).OpenBranch(
                 BT.Call(m_EnemyBehaviour.ChekTargetStillVisible),
                 BT.Call(m_EnemyBehaviour.OrientToTarget),
                 BT.Call(m_EnemyBehaviour.RemeberTargetPos),
                 BT.Trigger(m_Animator, "Shooting"),
                
                 BT.WaitForAnimatorState(m_Animator, "Attack")
               ),

             

             BT.If(() => { return m_EnemyBehaviour.target == null; }).OpenBranch(
                 BT.Call(m_EnemyBehaviour.ScanForPlayer)

                 )

            );
    }


    void Update()
    {
        m_Ai.Tick();
        
    }
}

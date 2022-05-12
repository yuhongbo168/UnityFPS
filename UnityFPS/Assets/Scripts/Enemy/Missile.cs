using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
   
    public float speed = 10f;
    public float angleStandard = 45.0f;
    private Vector3 distanceToTarget;
    private bool move = true;
    private Vector3 m_target;

    public float rotSpeed = 10f;
   
    void Start()
    {
        distanceToTarget = (m_target - transform.position).normalized;
        StartCoroutine(DirectShoot());
    }

    public void SetTarget(Vector3 pos)
    {

        m_target = pos;
    }

    IEnumerator DirectShoot()
    {
        while (true)
        {

            transform.position += distanceToTarget * speed * Time.deltaTime;
            yield return null;

        }
    }

    IEnumerator TraceShoot()
    {
        while(move)
        {
            Vector3 direction = (m_target - transform.position).normalized;

            float a = Vector3.Angle(transform.right, direction) / rotSpeed;

            transform.right = Vector3.Slerp(transform.right, direction, Time.deltaTime*2 ).normalized;

            //             if (a > 0.1f || a < -0.1f)
            //             {
            //                 transform.right = Vector3.Slerp(transform.right, direction, Time.deltaTime / a).normalized;
            //             }
            //             else
            //             {
            //                 speed += 2 * Time.deltaTime;
            //                 transform.right = Vector3.Slerp(transform.right, direction, 1).normalized;
            //             }

            //transform.position += transform.right * speed * Time.deltaTime;

            transform.position += transform.right * speed * Time.deltaTime;

            Vector3 targetPos = m_target;

            //transform.LookAt(targetPos);

            //             float angle = Mathf.Min(1, Vector3.Distance(this.transform.position, targetPos) / distanceToTarget) * angleStandard;
            //             this.transform.rotation = this.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            //             float currentDist = Vector3.Distance(transform.position, target.transform.position);
            //             if (currentDist<0.5f)
            //             {
            //                 yield break;
            //             }

            
            yield return null;
        }
    }

  
}

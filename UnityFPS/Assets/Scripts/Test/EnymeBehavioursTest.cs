using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class EnymeBehavioursTest : MonoBehaviour
{
    [Range(0,50)]
    public float MeleeRange = 10;
    [Range(0, 360)]
    public float angle = 10;
    public float spawnTimer=4f;

    public float viewDistance = 10;
    public float viewDerection;
    public float timeBeforTargetLost;
    private float m_TimeSinceLastTargetView;

    public Missile bullet;

    private GameObject m_target;
    private Vector3 m_TargetShootPosition;
    private float m_ShootTimer;
    private Coroutine m_ShootCoroutine;
    

    public GameObject Target
    {
        get { return m_target; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (m_TimeSinceLastTargetView > 0)
        {
            m_TimeSinceLastTargetView -= Time.deltaTime;
        }
        
    }

    private void FixedUpdate()
    {

        
    }

    public void  ScanTarget()
    {


        var dis = Character.PlayerCharacter.transform.position - transform.position;
    
        if (dis.sqrMagnitude > viewDistance * viewDistance)
        {
            return;
        }      

        Vector3 forward = Quaternion.Euler(0, 0, Mathf.Sign(Vector2.right.x) * viewDerection) * Vector3.right;

        float an = Vector3.Angle(forward, dis);

        if (an <= angle*0.5f)
        {
            m_target = Character.PlayerCharacter.gameObject;
            
        }

    }

    public void ForgetTarget()
    {
        m_target = null;
    }

    public void CheckTargetStillVisible()
    {
 
        if (m_target == null)
        {
            return;
        }
        
        var dis = Character.PlayerCharacter.transform.position - transform.position;

        //Debug.Log(dis.sqrMagnitude);

        if (dis.sqrMagnitude < viewDistance * viewDistance)
        {
            Vector3 forward = Quaternion.Euler(0, 0, Mathf.Sign(Vector2.right.x) * viewDerection) * Vector3.right;

            float an = Vector3.Angle(forward, dis);

            if ( an <= angle * 0.5f)
            {
                m_TimeSinceLastTargetView = timeBeforTargetLost;
            }
        }

        if (m_TimeSinceLastTargetView <= 0)
        {
            ForgetTarget();
        }

        Debug.Log(Target);

    }

    public void RememberTargetPos()
    {
        if (m_target == null)
        {
            return;
        }

        m_TargetShootPosition = m_target.transform.position;
    }

    public void SpawnButtle()
    {
        if (m_target)
        {
            Debug.Log("Fire");
            Missile instance = Instantiate(bullet, transform.position, Quaternion.identity);
            instance.SetTarget(m_target.transform.position);
        }

    }

    public void Fire()
    {
   
          m_ShootCoroutine=StartCoroutine(shooting());
        
    }

    public void StopFire()
    {
        if (m_ShootCoroutine!=null)
        {
            StopCoroutine(m_ShootCoroutine);
        }
    }

    IEnumerator shooting()
    {
        while (true)
        {
            if (Time.time > m_ShootTimer)
            {
                SpawnButtle();
                m_ShootTimer = Time.time + spawnTimer;
            }
            yield return null;
        }
        
    }

    public void OnDrawGizmosSelected()
    {
        Vector3 forward = Quaternion.Euler(0, 0, viewDerection) * Vector3.right;
        var endpoint =transform.position + (Quaternion.Euler(0, 0, angle * 0.5f) * forward);

        Color handlerColorScan = new Color(0, 1, 0, 0.1f);
        Handles.color = handlerColorScan;
        Handles.DrawSolidArc(transform.position, -Vector3.forward,(endpoint-transform.position).normalized , angle, viewDistance);


        Color handleColor = new Color(1, 0, 0, 0.1f);
        Handles.color = handleColor;
        Handles.DrawSolidDisc(transform.position, Vector3.forward, MeleeRange);
    }
}

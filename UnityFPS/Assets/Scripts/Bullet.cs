using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    public bool destroyWhenOutOfView = true;
    public bool spriteOriginallyFacesLeft;

    [HideInInspector]
    public Camera mianCamera;
    public float timeBeforAutodestruct = -1.0f;

    protected SpriteRenderer m_SpriteRenderer;

    public BulletObject bulletPoolObject;

    const float k_OffScreentError = 0.01f;

    protected float m_Timer;
    // Start is called before the first frame update


    private void OnEnable()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Timer = 0.0f;
    }
    private void FixedUpdate()
    {
        Vector3 screenPoint = mianCamera.WorldToViewportPoint(transform.position);

        bool onScreen = screenPoint.z > 0 && screenPoint.x > -k_OffScreentError && screenPoint.x < 1 + k_OffScreentError &&
            screenPoint.y > -k_OffScreentError && screenPoint.y < 1 + k_OffScreentError;

        if (!onScreen)
        {
            ReturnToPool();
        }

        if (timeBeforAutodestruct>0)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer>timeBeforAutodestruct)
            {
                bulletPoolObject.ReturnToPool();
            }
        }
    }

    public void ReturnToPool()
    {
        bulletPoolObject.ReturnToPool();
    }

    public void OnHitDamageable(Damager origin,Damabeable damageable)
    {

    }

    public void OnHitNonDamageable(Damager origin)
    {

    }

    protected void FindSurface(Collider2D collider)
    {
        Vector3 forward = spriteOriginallyFacesLeft ? Vector3.left : Vector3.right;
        if (m_SpriteRenderer.flipX)
        {
            forward.x = -forward.x;
        }
    }



}

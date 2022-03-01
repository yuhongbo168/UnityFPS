using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    public bool destroyWhenOutOfView = true;

    public Camera mianCamera;

    public BulletObject bulletPoolObject;

    const float k_OffScreentError = 0.01f;
    // Start is called before the first frame update
 

    private void FixedUpdate()
    {
        Vector3 screenPoint = mianCamera.WorldToViewportPoint(transform.position);

        bool onScreen = screenPoint.z > 0 && screenPoint.x > -k_OffScreentError && screenPoint.x < 1 + k_OffScreentError &&
            screenPoint.y > -k_OffScreentError && screenPoint.y < 1 + k_OffScreentError;

        if (!onScreen)
        {
            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        bulletPoolObject.ReturnToPool();
    }

}

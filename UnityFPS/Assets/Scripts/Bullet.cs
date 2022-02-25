using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    public bool destroyWhenOutOfView = true;

    [HideInInspector]
    public Camera mianCamera;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 screenPoint = mianCamera.WorldToViewportPoint(transform.position);
        Debug.Log(screenPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

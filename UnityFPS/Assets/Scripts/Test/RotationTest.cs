using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public Transform aimTransformRight;
    public Transform refObject;
    //public Transform targetPos;
    public Transform handle;

    public Transform targetPos;
    public Transform originPos;



    private Transform aim;
    private Vector3 forward = new Vector3(1f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        aim = aimTransformRight;
    }

    void HanldeAiming()
    {

        

        Vector3 targetDirection;
        //aimTransform.
        targetDirection = (refObject.position - aimTransformRight.position).normalized;

        

        if (transform.position.x - aim.position.x>0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            
        }
        else
        {
            transform.localScale = Vector3.one;
        }
        //aimTransform.lo

        handle.up = targetDirection;

        //float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

       // handle.localEulerAngles = new Vector3(0, 0, angle);


       // Debug.Log(angle);
    }
    // Update is called once per frame
    void Update()
    {
        originPos = targetPos;
        // Vector3 targetPos = Input.mousePosition;

        Vector3 mouseToWorldPos = Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z) );

        Vector3 targetDirection = (refObject.position - mouseToWorldPos ).normalized;

        handle.up = targetDirection;

        Debug.Log(mouseToWorldPos);

       
    }
}

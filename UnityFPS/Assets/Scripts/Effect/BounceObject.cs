using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BounceObject : MonoBehaviour
{
    public float bounceSpeed = 4f;
    protected CharacterController Pc;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var chareater = collision.gameObject.GetComponent<Character>();
            Pc = collision.gameObject.GetComponent<CharacterController>();
            Rigidbody2D rd = collision.gameObject.GetComponent<Rigidbody2D>();

            if (chareater != null)
            {

                Debug.Log("NormalJump");
                chareater.SetMoveVector(transform.up * bounceSpeed);

            }

        }
    }



    //private void 

    // Start is called before the first frame update

}

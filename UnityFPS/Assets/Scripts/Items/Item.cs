using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Item : MonoBehaviour
{
    public GameObject item;
    public float Distance = 3f;
    public int spawnCount = 1;
    public float timer=0.5f;
    public float randomRange = 2f;

    private bool isDid;
    private Rigidbody2D m_Rigidbody2d;

    private void Awake()
    {
//         m_Rigidbody2d = GetComponent<Rigidbody2D>();
//         m_Rigidbody2d.gravityScale = 0;
        isDid = false;
    }
    public void FixedUpdate()
    {
        if (isDid)
        {
//             timer -=Time.deltaTime;
// 
//             if (timer<=0)
//             {
//                 if (Vector3.Distance(Character.PlayerCharacter.transform.position, transform.position) < Distance)
//                 {
//                     /*this.GetComponent<Rigidbody2D>().gravityScale = 0;*/
// 
//                     if (Vector3.Distance(Character.PlayerCharacter.transform.position, transform.position) < Distance*2f)
//                     {
//                         transform.position = Vector2.Lerp(transform.position, Character.PlayerCharacter.transform.position, 6 * Time.deltaTime);
//                     }
// 
//                     transform.position = Vector2.Lerp(transform.position, Character.PlayerCharacter.transform.position, 2 * Time.deltaTime);
//                     
//                 }
//             }
//            
        }

    }
    public void SpawnItem(Vector3 pos)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newItem = Instantiate(item, new Vector3(pos.x + Random.Range(-randomRange, randomRange), pos.y, 0f), Quaternion.identity);
            //newItem.GetComponent<Rigidbody2D>().gravityScale = 1f;
            newItem.GetComponent<Item>().timer = 0.5f;
            newItem.GetComponent<Item>().isDid = true;
        }
        
       
        //newItem.GetComponent<Rigidbody2D>().AddForce(newVector);
        //newItem.GetComponent<Rigidbody2D>().gravityScale = 0f;
        
    }

   
}

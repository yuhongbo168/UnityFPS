using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject endObject;
    public GameObject prefabe;
    public int NodesCount = 10;
    public float distanceFromChainEnd = 0.6f;

    GameObject lastNode;

    Rigidbody2D hook;

    public List<GameObject> Nodes = new List<GameObject>();

    LineRenderer m_LineRenderer;



    private void Awake()
    {
       /* hook = GetComponent<Rigidbody2D>();*/
        m_LineRenderer = GetComponent<LineRenderer>();
      

       // Nodes.Add(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
       // lastNode = transform.gameObject;

       // CreateRope();
    }



    private void Update()
    {      

       CreateLineRender();

    }

    void CreateRope()
    {
//         Rigidbody2D previousRB = hook;
// 
//         
// 
//         for (int i = 0; i < NodesCount; i++)
//         {
//             Vector2 pos = hook.transform.position;
//             pos = pos + Vector2.down * distanceFromChainEnd * i;
// 
//             GameObject go = (GameObject)Instantiate(prefabe, pos, Quaternion.identity);
// 
//             Nodes.Add(go);
// 
//             go.transform.SetParent(transform);
// 
//             HingeJoint2D joint = go.GetComponent<HingeJoint2D>();
//             joint.connectedBody = previousRB;
// 
//             if (i < NodesCount-1)
//             {
//                 previousRB = go.GetComponent<Rigidbody2D>();
//             }
//             else
//             {
//                 ConnectRopeEnd(go.GetComponent<Rigidbody2D>());
//             }
//             
//         }

        
    }

 
    public void SubtractLinks()
    {
//         var node = Nodes[0];
//         Nodes.Remove(node);
//         //CreateRope();
    }
    void CreateLineRender()
    {
        m_LineRenderer.positionCount = Nodes.Count;

       

        for (int i = 0; i < Nodes.Count; i++)
        {
            m_LineRenderer.SetPosition(i, Nodes[i].transform.position);
        }

         //m_LineRenderer.SetPosition(i, endObject.transform.position);

    }
   

//     void ConnectRopeEnd(Rigidbody2D endRB)
//     {
//         HingeJoint2D joint = endObject.AddComponent<HingeJoint2D>();
//         joint.autoConfigureConnectedAnchor = false;
//         joint.connectedBody = endRB;
//         joint.connectedAnchor = new Vector2(0f, -.6f);
//     }



}

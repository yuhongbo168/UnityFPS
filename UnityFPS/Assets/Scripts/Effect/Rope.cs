using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Rope : MonoBehaviour
{
    public GameObject endObject;
    public GameObject prefabe;
    public int NodesCount = 10;
    public float distanceFromChainEnd = 0.6f;

    GameObject lastNode;

    Rigidbody2D hook;

    public List<GameObject> Nodes = new List<GameObject>();

    public LineRenderer m_LineRenderer;

    public LineRenderer GetLineRender
    {
        get { return m_LineRenderer; }
    }

    private void Awake()
    {
        /* hook = GetComponent<Rigidbody2D>();*/
        GetReferences();


       // Nodes.Add(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // lastNode = transform.gameObject;

        // CreateRope();
        CreateLineRender();
    }

    private void Update()
    {      

       

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

    public void GetReferences()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }
 
    public void SubtractLinks()
    {
//         var node = Nodes[0];
//         Nodes.Remove(node);
//         //CreateRope();
    }
   public void CreateLineRender()
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

#if UNITY_EDITOR
[CustomEditor(typeof(Rope))]
public class RopeEditor:Editor
{

    protected Rope m_Rope;
    protected List<GameObject> m_Nodes;
    LineRenderer m_LineRenderer;
    private void OnEnable()
    {
        m_Rope = target as Rope;
        m_Rope.GetReferences();

        m_Nodes = m_Rope.Nodes;
        m_LineRenderer = m_Rope.GetLineRender;


        m_Rope.m_LineRenderer.positionCount = m_Nodes.Count;

        for (int i = 0; i < m_Nodes.Count; i++)
        {
            m_Rope.m_LineRenderer.SetPosition(i, m_Nodes[i].transform.position);
        }

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        m_Rope.CreateLineRender();


        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(m_Rope, "Modify m_Rope");
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
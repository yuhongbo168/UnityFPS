using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChainSide : MonoBehaviour
{
    public GameObject chainHead;
    public GameObject chainEnd;

    public float chainDistance=0.1f;

    public List<GameObject> nodes;

    LineRenderer m_lineRendere;

    private void Awake()
    {
        m_lineRendere = GetComponent<LineRenderer>();

       


    }
    // Start is called before the first frame update
    void Start()
    {

        CreateLineRendere();
    }

    void CreateLineRendere()
    {
        var derection = (chainEnd.transform.position - chainHead.transform.position).normalized;
        float distance = Vector3.Distance(chainHead.transform.position, chainEnd.transform.position);

        int chainCount = Mathf.FloorToInt(distance / chainDistance);    

        for (int i = 0; i < chainCount; i++)
        {
            var newGameObejct = new GameObject();
            var object01 = Instantiate(newGameObejct);

            newGameObejct.AddComponent<CircleCollider2D>();
            newGameObejct.GetComponent<CircleCollider2D>().radius = 0.2f;

            newGameObejct.transform.position = transform.position + derection * chainDistance * i;
            newGameObejct.transform.SetParent(transform);
            nodes.Add(object01);

        }

        m_lineRendere.positionCount = nodes.Count + 1;

        for (int i = 0; i < nodes.Count; i++)
        {
            m_lineRendere.SetPosition(i, nodes[i].transform.position);
        }

        m_lineRendere.SetPosition(nodes.Count, chainEnd.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChainSide))]
public class ChianEditor:Editor
{
    protected ChainSide m_ChainSide;

    //     protected float m_chainDistance;
    //     protected GameObject chainEnd;

    protected SerializedProperty m_ChainDistance;
    protected SerializedProperty m_ChainEnd;


    private void OnEnable()
    {
        m_ChainDistance = serializedObject.FindProperty("chainDistance");
        m_ChainEnd = serializedObject.FindProperty("chainEnd");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();





        serializedObject.ApplyModifiedProperties();
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChainSide : MonoBehaviour
{
    public GameObject chainHead;
    public Transform chainEnd;

    public float chainDistance=0.1f;

    public List<GameObject> nodes;

    protected LineRenderer m_lineRendere;

    protected Vector3 m_ChainEndPos;

    private void Awake()
    {
        SetReferences();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_ChainEndPos = chainEnd.position;
        CreateLineRendere();
    }

    public void SetReferences()
    {
        m_lineRendere = GetComponent<LineRenderer>();
    }

    void CreateLineRendere()
    {
       
        var derection = (chainEnd.transform.position - chainHead.transform.position).normalized;
        float distance = Vector3.Distance(chainHead.transform.position, chainEnd.transform.position);

        int chainCount = Mathf.FloorToInt(distance / chainDistance);    

        for (int i = 0; i < chainCount; i++)
        {
            var newGameObejct = new GameObject("Chain_"+ i);

            newGameObejct.AddComponent<ChainCollision>();
            newGameObejct.AddComponent<CircleCollider2D>();
            newGameObejct.GetComponent<CircleCollider2D>().radius = 0.2f;
            newGameObejct.GetComponent<CircleCollider2D>().isTrigger = true;

            newGameObejct.transform.position = transform.position + derection * chainDistance * i;
            newGameObejct.transform.SetParent(transform);
            nodes.Add(newGameObejct);

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
       // CreateLineRendere();
    }
}

public class ChainCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            Character cheracter = collision.GetComponent<Character>();
            if (cheracter!=null)
            {              
                var chainParent = transform.parent.GetComponent<ChainSide>();

                int index = chainParent.nodes.IndexOf(this.gameObject);

                if (index+2> chainParent.nodes.Count)
                {
                    return;
                }
                // cheracter.PC.capsuleCollider.enabled = false;
                if (cheracter.chainside != null&& cheracter.chainside!=this)
                {
                    for (int i = 0; i < cheracter.chainside.nodes.Count; i++)
                    {
                        cheracter.chainside.nodes[i].GetComponent<CircleCollider2D>().enabled = true;
                    }
                }


                cheracter.chainside = chainParent;

                cheracter.CheckStartChain(index);

                cheracter.SlidChian = true;
            }
            
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {

    }

}


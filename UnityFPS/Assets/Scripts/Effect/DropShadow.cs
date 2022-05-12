using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadow : MonoBehaviour
{
    public GameObject origin;
    public LayerMask grounded;
    public float offset = -0.2f;
    public float maxHeight = 3f;

    private ContactFilter2D m_ContactFilter;
    private Vector3 m_OriginalSize;
    private SpriteRenderer m_SpriteRenderer;
    private RaycastHit2D[] m_Hit = new RaycastHit2D[3];

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_ContactFilter.layerMask = grounded;
        m_ContactFilter.useTriggers = false;
        m_ContactFilter.useLayerMask = true;

        m_OriginalSize = transform.localScale;

    }


    private void LateUpdate()
    {
        int count = Physics2D.Raycast(origin.transform.position + Vector3.up * 0.5f, Vector3.down, m_ContactFilter, m_Hit);

        if (count>0)
        {
            m_SpriteRenderer.gameObject.SetActive(true);
            transform.position = m_Hit[0].point;

            float height = Vector3.Magnitude(origin.transform.position - transform.position);
            float ratio = Mathf.Clamp(1 - height / (maxHeight * maxHeight), 0f, 1f);

            transform.localScale = m_OriginalSize * ratio;
        }
        else
        {
            m_SpriteRenderer.gameObject.SetActive(false);
        }

        
    }

}

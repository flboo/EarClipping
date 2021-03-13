
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HuluExport : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ObjParent;
    [SerializeField]
    private GameObject m_ObjHulu;

    [SerializeField]
    private Transform m_expodPos;
    [SerializeField]
    private Explodable m_Explodable;

    void Awake()
    {
        m_Explodable = GameObject.Instantiate(Resources.Load("Prefabs/HuLuBroken") as GameObject).GetComponent<Explodable>();
        m_Explodable.m_ObjParents = m_ObjParent;

        m_Explodable.fragmentInEditor();
        m_Explodable.transform.SetParent(this.transform);
        m_Explodable.transform.localPosition = Vector3.zero;
        m_Explodable.transform.eulerAngles = Vector3.zero;
        m_Explodable.transform.localScale = Vector3.one;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleExplode();
        }
    }

    public void HandleExplode()
    {
        m_ObjHulu.SetActive(false);
        m_Explodable.explode();
        StartCoroutine(waitAndExplode());
    }

    private IEnumerator waitAndExplode()
    {
        yield return new WaitForFixedUpdate();
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < m_Explodable.fragments.Count; i++)
        {
            dir = m_Explodable.fragments[i].transform.position - m_expodPos.position;
            dir.z = 0;
            Rigidbody2D rigidbody2D = m_Explodable.fragments[i].transform.GetComponent<Rigidbody2D>();
            rigidbody2D.AddForce(dir * 50);
            rigidbody2D.gravityScale = 40;
        }

    }

}


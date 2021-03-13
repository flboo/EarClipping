using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test01 : MonoBehaviour
{

    public Transform[] m_TestPos;

    void Start()
    {
        Vector3 AB = m_TestPos[1].position - m_TestPos[0].position;
        Vector3 BC = m_TestPos[2].position - m_TestPos[1].position;
        Debug.LogError(Vector3.Cross(BC, AB));
        Debug.LogError(Vector3.Cross(AB, BC));


        bool IaasIn = IsIn(m_TestPos[0].position, m_TestPos[0].position, m_TestPos[1].position, m_TestPos[2].position);
        Debug.LogError("IaasInIaasIn  " + IaasIn);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsIn(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 pa = p - a;
        Vector3 pb = p - b;
        Vector3 pc = p - c;

        Vector3 t1 = Vector3.Cross(pa, pb);
        Vector3 t2 = Vector3.Cross(pb, pc);
        Vector3 t3 = Vector3.Cross(pc, pa);

        bool isIn2 = t1.y >= 0 && t2.y >= 0 && t3.y >= 0 || t1.y <= 0 && t2.y <= 0 && t3.y <= 0;

        return isIn2;
    }

}

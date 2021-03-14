using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolygonTool;

public class Lines : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < transform.childCount - 1)
            {
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
            }
            else
            {
                Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
            }

        }

        Gizmos.color = Color.black;

    }

}

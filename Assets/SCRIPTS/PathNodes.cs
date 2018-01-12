using System.Collections.Generic;
using UnityEngine;

public class PathNodes : MonoBehaviour {

    public Color lineColor;
    private List<Transform> nodes = new List<Transform>();

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        Transform[] PathTransforms = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        for(int i = 0; i < PathTransforms.Length; i++)
        {
            if (PathTransforms[i] != transform)
            {
                nodes.Add(PathTransforms[i]);
            }
        }
        for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 CurrentNode = nodes[i].position;
            Vector3 PrevNode = Vector3.zero;

            if(i > 0)
            {
                PrevNode = nodes[i - 1].position;
            } else if(i == 0 && nodes.Count > 1)
            {
                PrevNode = nodes[nodes.Count - 1].position;
            }
            Gizmos.DrawLine(PrevNode, CurrentNode);
            Gizmos.DrawSphere(CurrentNode, 0.2f);
        }

    }
}

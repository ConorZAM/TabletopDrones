using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class measure : MonoBehaviour
{
    public Transform otherPoint;
    public float distance;

    private void OnDrawGizmos()
    {
        distance = Vector3.Distance(transform.position, otherPoint.position);
    }
}

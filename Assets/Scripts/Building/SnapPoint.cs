using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    [SerializeField] private GameObject snapPoint;

    public Transform GetSnapPoint()
    {
        return snapPoint.transform;
    }
}

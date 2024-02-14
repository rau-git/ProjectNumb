using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    [SerializeField] private GameObject snapPoint;

    public Transform GetSnapPoint()
    {
        return snapPoint.transform;
    }
}

using FishNet.Object;
using UnityEngine;

public class TreeRandomiser : NetworkBehaviour
{
    [SerializeField] private Vector2 scaleRange;
    
    [Server]
    void Start()
    {
        gameObject.transform.localScale += new Vector3(0, Random.Range(scaleRange.x, scaleRange.y), 0);
    }
}

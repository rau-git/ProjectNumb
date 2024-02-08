using FishNet.Object;
using UnityEngine;

public class BuildLocationController : NetworkBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private Transform raycastFireOrigin;
    private Collider collider;
    private Transform transformToSend;
    private PlayerBuild playerBuild;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        playerBuild = GetComponentInParent<PlayerBuild>();
    }

    private void Start()
    {
        transformToSend = transform;
    }

    private void FixedUpdate()
    {
        PositionHandler();
    }

    private void PositionHandler()
    {
        if (!Physics.Raycast(raycastFireOrigin.position, raycastFireOrigin.forward, out var hit, maxDistance)) return;
        
        gameObject.transform.position = hit.point;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FoundationSnapPoint"))
        {
            Debug.Log($"collision successful with {other.gameObject.name}");
            playerBuild.SetGhostTransform(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FoundationSnapPoint"))
        {
            playerBuild.SetGhostTransform(transform);
        }
    }
}

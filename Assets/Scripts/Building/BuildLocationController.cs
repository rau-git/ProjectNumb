using FishNet.Object;
using UnityEngine;

public class BuildLocationController : NetworkBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private float snapRadius;
    [SerializeField] private Transform raycastFireOrigin;
    private PlayerBuild playerBuild;

    private void Awake()
    {
        playerBuild = GetComponentInParent<PlayerBuild>();
    }

    private void FixedUpdate()
    {
        PositionHandler();
        SnapCheck();
    }

    private void SnapCheck()
    {
        var closestDistance = snapRadius;
        Vector3 maxSpherePosition = raycastFireOrigin.position + raycastFireOrigin.forward * maxDistance;
        
        if(Physics.Raycast(raycastFireOrigin.position, raycastFireOrigin.forward, out var hit, maxDistance))
        {
            maxSpherePosition = hit.point;
        }
        
        Collider[] colliders = Physics.OverlapSphere(maxSpherePosition, snapRadius, 1 << LayerMask.NameToLayer("BuildingSnap"), QueryTriggerInteraction.Collide);

        if (colliders.Length <= 0)
        {
            playerBuild.SetGhostTransform(transform);
            return;
        }

        foreach (var collider in colliders)
        {
            if (Vector3.Distance(hit.point, collider.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(hit.point, collider.transform.position);

                if(collider.gameObject.CompareTag(playerBuild.canSnapTo))
                {
                    playerBuild.SetGhostTransform(collider.transform);
                }
                else
                {
                    playerBuild.SetGhostTransform(transform);
                }
            }
        }
    }

    private void PositionHandler()
    {
        if (!Physics.Raycast(raycastFireOrigin.position, raycastFireOrigin.forward, out var hit, maxDistance)) return;
        
        gameObject.transform.position = hit.point;
    }
}

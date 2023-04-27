using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    private Material _material;
    
    [SyncVar(OnChange = nameof(on_health))]
    private Color _color;

    private void Awake()
    {
        _material = GetComponentInChildren<MeshRenderer>().material;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        ChangeColor();
    }

    private void on_health(Color previous, Color next, bool asServer)
    {
        if (!IsClient) return;

        _material.color = next;
    }

    [ServerRpc]
    private void ChangeColor()
    {
        if (!IsServer) return;
        
        _color = Random.ColorHSV(0, 255, 0, 255, 0, 255);
    }
}

using FishNet.Object;
using UnityEngine;

public class WorldObjectRandomiser : NetworkBehaviour
{
    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        
        var randomScale = Random.Range(-0.1f, 0.1f);
        gameObject.transform.localScale += new Vector3(randomScale, randomScale, randomScale);
        gameObject.transform.Rotate(transform.up, Random.Range(0, 360));
    }
}

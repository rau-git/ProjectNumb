using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : NetworkBehaviour
{
    /*
     * Another class that isn't going to be fully fleshed out initially
     * the final attack class needs to verify what type of primary fire
     * should be called based on what the player is holding
     * or taking context from where the player is, what menu they are in etc.
     *
     * Additionally the server needs to authenticate all hits if cheating is to be avoided
     * and projectiles may be better feeling for a survival game, but we need to prototype
     * first and actually getting a working product for hand-in not a polished one
     */

    [SerializeField]
    private float _damage;
    [SerializeField] 
    private GameObject _raycastOrigin;

    private PlayerIngameControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerIngameControls();
        _playerControls.Enable();
    }

    private void Update()
    {
        OnPrimaryFire();
    }
    
    private void OnPrimaryFire()
    {
        if (!_playerControls.PlayerControls.PrimaryFire.WasPressedThisFrame()) return;

        ShootRPC(_raycastOrigin.transform.position, _raycastOrigin.transform.forward);
    }
    
    [ServerRpc]
    private void ShootRPC(Vector3 _originOfAttack, Vector3 _directionOfAttack)
    {
        if (Physics.Raycast(_originOfAttack, _directionOfAttack, out var hit) &&
            hit.transform.TryGetComponent(out HealthComponent health))

            health.OnDamage(_damage);
    }
}
using FishNet.Object;
using UnityEngine;

public class Attack : NetworkBehaviour
{
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
        if (Physics.Raycast(_originOfAttack, _directionOfAttack, out var hit) && hit.transform.TryGetComponent(out IDamageable damageable))
        {
            damageable.OnDamage(_damage, gameObject);
        }
    }
}
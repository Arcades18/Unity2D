using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;

    [SerializeField] private LayerMask mask;
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        if( _camera == null)
        {
            Debug.Log("PlayerShoot : No Camera Reference");
            this.enabled = false;
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    [Client]
    private void Shoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(_camera.transform.position,_camera.transform.forward,out _hit ,weapon.range,mask))
        {
            if(_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name);
            }
        }
    }
    [Command]
    private void CmdPlayerShot(string _ID)
    {
        Debug.Log(_ID + "has been shot.");
    }
}

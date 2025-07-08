using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] componentToDisble;

    [SerializeField] private string remoteLayerName = "RemotePlayer";

    private Camera sceneCamera;
    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null )
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
        RegisterPLayer();
    }
    private void RegisterPLayer()
    {
        string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }
    private void DisableComponents()
    {
        for (int i = 0; i < componentToDisble.Length; i++)
        {
            componentToDisble[i].enabled = false;
        }
    }
    private void OnDisable()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}

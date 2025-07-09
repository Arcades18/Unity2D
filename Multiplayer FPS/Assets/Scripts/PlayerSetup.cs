using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;
[RequireComponent(typeof(Player))]
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
        GetComponent<Player>().SetUp();
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID,_player);
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
        GameManager.UnregisterPlayer(transform.name);
    }
}

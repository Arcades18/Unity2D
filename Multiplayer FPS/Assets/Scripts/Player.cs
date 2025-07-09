using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

public class Player : NetworkBehaviour
{
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField] private int maxHealth = 100;
    [SyncVar] private int currentHealth;

    [SerializeField] private Behaviour[] diableOnDeath;
    private bool[] wasEnabled;

    //private void Update()
    //{
    //    if (!isLocalPlayer)
    //    {
    //        return;
    //    }
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        RpcTakeDamage(9999);
    //    }
    //}
    public void SetUp()
    {
        wasEnabled = new bool[diableOnDeath.Length];
        for(int i = 0;i < wasEnabled.Length;i++)
        {
            wasEnabled[i] = diableOnDeath[i].enabled;
        }
        SetDefaults();
    }
    [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= amount;
        Debug.Log(transform.name + "now has" +  currentHealth + "Health");
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        for(int i = 0; i < diableOnDeath.Length; i++)
        {
            diableOnDeath[i].enabled = false;
        }
        Collider _collider = GetComponent<Collider>();
        if(_collider != null)
        {
            _collider.enabled = false;
        }
        Debug.Log(transform.name + " is Dead!");

        StartCoroutine(Respawn());
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log(transform.name + " is Respawned");
    }
    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        for(int i = 0; i < diableOnDeath.Length; i++)
        {
            diableOnDeath[i].enabled = wasEnabled[i];
        }
        Collider _collider = GetComponent<Collider>();
        if(_collider != null)
        {
            _collider.enabled = true;
        }
    }
}

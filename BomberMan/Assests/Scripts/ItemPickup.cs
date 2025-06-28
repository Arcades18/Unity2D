using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
    }
    public ItemType Type;

    public void OnItemPickUp(GameObject player)
    {
        switch (Type) 
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;
            
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().explostionRadius++;
                break;
            
            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().Speed++;
                break;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickUp(other.gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    public enum Type 
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        StarMan,
    }
    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.instance.AddCoin(GameManager.instance.coins + 1);
                break;

            case Type.ExtraLife:
                GameManager.instance.AddLive(GameManager.instance.lives + 1);
                break;

            case Type.MagicMushroom:
                player.GetComponent<Player>().grow();
                break;

            case Type.StarMan:
                player.GetComponent<Player>().StarPower();
                break;
        }
        Destroy(gameObject);
    }
}

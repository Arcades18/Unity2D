using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab;
    public KeyCode Inputkey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombRemaining = 0;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionTime = 1f;
    public int explostionRadius = 1;

    [Header("Destructible")]
    public Destructible destructiblePrefab;
    public Tilemap destructibleTiles;

    private void OnEnable()
    {
        bombRemaining = bombAmount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(Inputkey) && bombRemaining > 0)
        {
            StartCoroutine(placeBomb());
        }
    }

    private IEnumerator placeBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab , position, Quaternion.identity);
        explosion.SetActiveRender(explosion.start);
        explosion.DestroyAfter(explosionTime);   
        Explode(position , Vector2.up , explostionRadius);
        Explode(position, Vector2.down, explostionRadius);
        Explode(position, Vector2.left, explostionRadius);
        Explode(position, Vector2.right, explostionRadius);


        Destroy(bomb);
        bombRemaining++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("bomb"))
        {
            other.isTrigger = false;
        }
    }

    private void Explode(Vector2 position , Vector2 direction  , int length)
    {
        if(length == 0)
        {
            return;
        }

        position += direction;

        if(Physics2D.OverlapBox(position , Vector2.one / 2f , 0f, explosionLayerMask))
        {
            destrutible(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRender(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionTime);
        Explode(position , direction , length - 1);
    }

    private void destrutible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if(tile != null)
        {
            Instantiate(destructiblePrefab , position ,Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }
     public void AddBomb()
    {
        bombAmount++;
        bombRemaining++;
    }
}

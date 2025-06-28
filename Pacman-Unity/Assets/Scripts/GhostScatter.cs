using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : Ghost_Behaviour
{
    private void OnDisable()
    {
        this.ghost.chase.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();
        if(node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            int RandomIndex = Random.Range(0,node.availableDirections.Count);
            if (node.availableDirections[RandomIndex] == -this.ghost.movement.direction && node.availableDirections.Count > 1)
            {
                RandomIndex++;
                if(RandomIndex >= node.availableDirections.Count)
                {
                    RandomIndex = 0;
                }
            }
            this.ghost.movement.SetDirection(node.availableDirections[RandomIndex]);
        }
    }
}

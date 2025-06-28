using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : Ghost_Behaviour
{
    private void OnDisable()
    {
        this.ghost.scatter.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();
        if(node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            Vector2 Direction = Vector2.zero;
            float minDistance = float.MaxValue;
            foreach(Vector2 availabledirection in node.availableDirections)
            {
                Vector3 newDirection = this.transform.position + new Vector3(availabledirection.x, availabledirection.y, 0.0f);
                float distance = (this.ghost.target.position - newDirection).sqrMagnitude;
                
                if(distance < minDistance)
                {
                    Direction = availabledirection;
                    minDistance = distance;
                }
            }
            this.ghost.movement.SetDirection(Direction);
        }
    }
}

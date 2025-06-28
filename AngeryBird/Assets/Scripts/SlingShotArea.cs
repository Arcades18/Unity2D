using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask slingShotArea;
   public bool IsWithSlingShotArea()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputSystem.mousePosition);

        if (Physics2D.OverlapPoint(worldPosition,slingShotArea))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}

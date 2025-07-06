using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Player State")]
    public float moveSpeed = 5f;
    
    [Header("Layer Mask")]
    public LayerMask solidObjectLayer;
    public LayerMask longGrassLayer;

    public event Action OnEncounter;

    private bool isMoving = false;
    private Vector2 input;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0)
                input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector3 targetPos = transform.position + new Vector3(input.x, input.y, 0);
                if (isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
        CheckForEncounter();
    }
    private bool isWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos,0.2f,solidObjectLayer) != null)
        {
            return false;
        } 
        return true;
    }
    private void CheckForEncounter()
    {
        if(Physics2D.OverlapCircle(this.transform.position,0.2f,longGrassLayer) != null)
        {
            if(UnityEngine.Random.Range(1,101) <= 10)
            {
                animator.SetBool("isMoving", false);
                OnEncounter();
            }
        }
    }
}


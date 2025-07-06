using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    public float height = 6.5f;
    public float underGroundHeight = -9.5f;
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        Vector3 cameraPosition = this.transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
        this.transform.position = cameraPosition;
    }

    public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? underGroundHeight : height;
        this.transform.position = cameraPosition;
    }
}

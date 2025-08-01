using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (connection != null && other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(enterKeyCode))
            {
                StartCoroutine(enterAnimation(other.transform));
            }
        }
    }
    private IEnumerator enterAnimation(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        Vector3 enteredPosition = transform.position + enterDirection;
        Vector3 enteredScale = Vector3.one * 0.5f;

        yield return Move(player, enteredPosition, enteredScale);
        yield return new WaitForSeconds(1f);

        bool underGround = connection.position.y < 0;
        Camera.main.GetComponent<SideScrolling>().SetUnderground(underGround);

        if (exitDirection != Vector3.zero)
        {
            player.position = connection.position - exitDirection;
            yield return Move(player,connection.position + exitDirection,Vector3.one);
        }
        else
        {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }
        player.GetComponent<PlayerMovement>().enabled = true;
    }
    private IEnumerator Move(Transform player ,Vector3 EndPosition,Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 1f;

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, EndPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            
            elapsed += Time.deltaTime;

            yield return null;
        }
        player.position = EndPosition;
        player.localScale = endScale;
    }
}

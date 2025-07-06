using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.AddCoin(GameManager.instance.coins + 1);
        StartCoroutine(Animate());
    }
    private IEnumerator Animate()
    {

        Vector3 startingPosiiton = transform.localPosition;
        Vector3 animationPosition = startingPosiiton + Vector3.up * 2f;

        yield return Move(startingPosiiton, animationPosition);
        yield return Move(animationPosition, startingPosiiton);

        Destroy(gameObject);

    }
    private IEnumerator Move(Vector3 starting, Vector3 ending)
    {
        float elapsed = 0.0f;
        float duration = 0.25f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(starting, ending, t);
            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = ending;
    }
}

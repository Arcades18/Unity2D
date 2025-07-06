using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject healt;

    public void SetHp(float hpNormalize)
    {
        healt.transform.localScale = new Vector2(hpNormalize, 1f);
    }
    public IEnumerator SetHpSmooth(float newHp)
    {
        float currentHp = healt.transform.localScale.x;
        float changeAmount = currentHp - newHp;
        while(currentHp - newHp > Mathf.Epsilon)
        {
            currentHp -= changeAmount * Time.deltaTime;
            healt.transform.localScale = new Vector3(currentHp, 1f);
            yield return null;
        }
        healt.transform.localScale = new Vector3(newHp, 1f);
    }
}

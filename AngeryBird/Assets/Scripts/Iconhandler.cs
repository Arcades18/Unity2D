using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Iconhandler : MonoBehaviour
{
    [SerializeField] private Image[] icon;
    [SerializeField] private Color usedColor;

    public void UsedShot(int shotNumber)
    {
        for (int i = 0; i < icon.Length; i++)
        {
            if(shotNumber == i + 1)
            {
                icon[i].color = usedColor;
                return;
            }
        }
    }
}

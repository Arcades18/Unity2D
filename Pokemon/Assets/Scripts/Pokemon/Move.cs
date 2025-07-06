using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase Base;
    public int Pp;

    public Move(MoveBase pBase)
    {
        Base = pBase;
        Pp = pBase.Pp;
    }
}

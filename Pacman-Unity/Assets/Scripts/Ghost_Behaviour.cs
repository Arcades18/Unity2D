using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost_Behaviour : MonoBehaviour
{
    public Ghost ghost { get; private set; }
    public float duration;

    private void Awake()
    {
        this.ghost = GetComponent<Ghost>();
    }
    private void Start()
    {
        this.enabled = true;
    }

    public void Enable()
    {
        Enable(this.duration);
    }
    public virtual void Enable(float duration)
    {
        this.enabled = true;
        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }
    public virtual void Disable()
    {
        this.enabled = false;
        CancelInvoke();
    }


}

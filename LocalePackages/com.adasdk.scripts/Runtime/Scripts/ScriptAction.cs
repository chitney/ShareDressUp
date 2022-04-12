using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptAction : MonoBehaviour
{
    public const string Name = "Directory/Name";
    public virtual string Description() { return "Description"; }

    [HideInInspector]
    public PlayerScript Owner;

    private bool isComplete;
    public bool IsComplete { get => isComplete; private set => isComplete = value; }

    [HideInInspector]
    public bool IsStarted = false;

    public virtual void OnStart() {
        IsStarted = true;
    }

    public virtual void OnEnd() {
        IsComplete = true;
        Owner.OnActionEnd(this);
    }

}

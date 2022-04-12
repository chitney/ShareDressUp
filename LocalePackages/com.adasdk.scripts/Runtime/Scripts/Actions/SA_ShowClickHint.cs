using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_ShowClickHint : ScriptAction
{
    public new const string Name = "Hints/ShowClick";

    public override string Description()
    {
        return "Show click hint";
    }

    public Vector3 Target;

    public override void OnStart()
    {
        HintsController.ShowClickHint(Target);
        base.OnStart();
        OnEnd();
    }
}

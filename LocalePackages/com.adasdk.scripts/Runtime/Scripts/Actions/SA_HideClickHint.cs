using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_HideClickHint : ScriptAction
{
    public new const string Name = "Hints/HideClick";

    public override string Description()
    {
        return "Hide click hint";
    }

    public override void OnStart()
    {
        HintsController.StopClickHint();
        base.OnStart();
        OnEnd();
    }
}
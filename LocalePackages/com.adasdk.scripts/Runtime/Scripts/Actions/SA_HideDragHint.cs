using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_HideDragHint : ScriptAction
{
    public new const string Name = "Hints/HideDrag";

    public override string Description()
    {
        return "Hide hint";
    }

    public override void OnStart()
    {
        base.OnStart();
        HintsController.StopDragHint();
        OnEnd();
    }
}

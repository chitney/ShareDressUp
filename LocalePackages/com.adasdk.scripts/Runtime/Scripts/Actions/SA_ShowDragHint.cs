using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_ShowDragHint : ScriptAction
{
    public new const string Name = "Hints/ShowDrag";

    public override string Description() {

        //string f = From ? From.name : "None";
        //string t = To ? To.name : "None";

        return "Show hint";//+ f + " to " + t;
    }

    public Vector3 From;
    public Vector3 To;

    public override void OnStart()
    {
        HintsController.ShowDragHint(From, To);
        base.OnStart();
        OnEnd();
    }
}

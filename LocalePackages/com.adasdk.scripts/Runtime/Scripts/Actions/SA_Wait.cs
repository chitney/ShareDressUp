using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_Wait : ScriptAction
{
    public new const string Name = "Tools/Wait";

    public override string Description()
    {
        return "Wait for " + Time + " sec ";
    }
    public float Time = 1f;

    public override void OnStart()
    {
        base.OnStart();
        Invoke("OnEnd", Time);
    }

}

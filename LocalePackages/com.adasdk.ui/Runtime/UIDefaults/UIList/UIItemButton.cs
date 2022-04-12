using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemButton : UIItemImage
{
    Action btnAction;

    public void Init(Sprite icon, Action btnAction)
    {
        this.btnAction = btnAction;
        base.Init(icon);
    }

    public virtual void _UI_OnClick()
    {
        btnAction?.Invoke();
    }
}

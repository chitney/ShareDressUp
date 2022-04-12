using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemCheckBox : UIItemButton
{
    [SerializeField]
    private UIModeSwitcher modeSwitcher;

    public void Init(Sprite icon, System.Action btnAction, int mode)
    {
        base.Init(icon, btnAction);
        SetMode(mode);
    }

    public void SetMode(int mode)
    {
        modeSwitcher.Mode = mode;
    }
   
}

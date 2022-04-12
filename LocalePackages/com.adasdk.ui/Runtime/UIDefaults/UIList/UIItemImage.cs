using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemImage : UIItem
{
    [SerializeField]
    private Image img;

    public Image Image { get => img; set => img = value; }

    public virtual void Init(Sprite icon)
    {
        img.sprite = icon;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemRawImage : UIItem
{
    [SerializeField]
    private RawImage img;

    public RawImage Image { get => img; set => img = value; }

    public virtual void Init(Texture2D tex)
    {
        img.texture = tex;
    }
}

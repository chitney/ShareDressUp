using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class ColorableGraphics : MonoBehaviour
{
    [SerializeField]
    private Graphic graphic;
    [SerializeField]
    private ThemenType Type;

    public ThemenType ThemenType
    {
        get => Type;
    }

    public Color Color
    {
        set
        {
            if (graphic != null)             
                graphic.color = value;

#if UNITY_EDITOR
            if (graphic == null)
                Debug.Log(transform.parent + " " + name);
#endif
        }
    }
}

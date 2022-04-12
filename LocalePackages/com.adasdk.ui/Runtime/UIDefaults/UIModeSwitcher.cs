using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeSwitcher : MonoBehaviour
{
    [SerializeField]
    private int mode = 0;

    public int Mode
    {
        get { return mode; }
        set
        {
            mode = value;
            Refresh();
        }
    }

    public bool SetMode(string _modeName)
    {
        int ind = ModeIndex(_modeName);
        if (ind < 0) return false;

        Mode = ind;
        return true;
    }

    public int ModeIndex(string _modeName)
    {
        for (int i = 0; i < ModeObjects.Length; i++)
            if (ModeObjects[i].ModeName == _modeName)
                return i;
        return -1;
    }

    [Serializable]
    public class SpriteSwap
    {
        public Image Target;
        public Sprite Sprite;
    }

    [Serializable]
    public class ColorSwap
    {
        public Graphic Target;
        public Color Color = UnityEngine.Color.white;
    }

    [Serializable]
    public class MaterialSwap
    {
        public Graphic Target;
        public Material Material = null;
    }

    [Serializable]
    public class PositionSwap
    {
        public Transform Target;
        public Vector3 Position = Vector3.zero;
    }

    [Serializable]
    public class ObjectArray
    {
        public string ModeName = string.Empty;
        public GameObject[] Objects = new GameObject[0];
        public SpriteSwap[] Sprites = new SpriteSwap[0];
        public ColorSwap[] Colors = new ColorSwap[0];
        public MaterialSwap[] Materials = new MaterialSwap[0];
        public PositionSwap[] Positions = new PositionSwap[0];

        public void ApplyObjects(bool _isEnabled)
        {
            for (int j = Objects.Length - 1; j >= 0; j--)
            {
                GameObject obj = Objects[j];
                if (obj != null)
                    obj.SetActive(_isEnabled);
            }
        }

        public void ApplySprites()
        {
            for (int j = Sprites.Length - 1; j >= 0; j--)
            {
                var data = Sprites[j];
                if (data.Target != null)
                    data.Target.sprite = data.Sprite;
            }
        }

        public void ApplyColors()
        {
            for (int j = Colors.Length - 1; j >= 0; j--)
            {
                var data = Colors[j];
                if (data.Target != null)
                    data.Target.color = data.Color;
            }
        }

        public void ApplyMaterials()
        {
            for (int j = Materials.Length - 1; j >= 0; j--)
            {
                var data = Materials[j];
                if (data.Target != null)
                    data.Target.material = data.Material;
            }
        }

        public void ApplyPositions()
        {
            for (int j = Positions.Length - 1; j >= 0; j--)
            {
                var data = Positions[j];
                if (data.Target != null)
                    data.Target.localPosition = data.Position;
            }
        }
    }

    public ObjectArray[] ModeObjects = new ObjectArray[0];

    [SerializeField, HideInInspector]
    private ObjectArray DefaultState = new ObjectArray();

    void OnValidate()
    {
        if (DefaultState == null)
            DefaultState = new ObjectArray();

        List<GameObject> list = new List<GameObject>();
        List<SpriteSwap> defaultSprites = new List<SpriteSwap>();
        List<ColorSwap> defaultColors = new List<ColorSwap>();

        for (int i = ModeObjects.Length - 1; i >= 0; i--)
        {
            ObjectArray swapData = ModeObjects[i];
            for (int j = swapData.Objects.Length - 1; j >= 0; j--)
            {
                GameObject obj = swapData.Objects[j];
                if (obj != null && !list.Contains(obj))
                    list.Add(obj);
            }

            for (int j = swapData.Sprites.Length - 1; j >= 0; j--)
            {
                SpriteSwap spriteSwap = swapData.Sprites[j];
                if (spriteSwap.Target == null || defaultSprites.Find(_t => _t.Target == spriteSwap.Target) != null)
                    continue;
                defaultSprites.Add(new SpriteSwap() { Target = spriteSwap.Target, Sprite = spriteSwap.Target.sprite });
            }

            for (int j = swapData.Colors.Length - 1; j >= 0; j--)
            {
                ColorSwap colorSwap = swapData.Colors[j];
                if (colorSwap.Target == null || defaultColors.Find(_t => _t.Target == colorSwap.Target) != null)
                    continue;
                defaultColors.Add(new ColorSwap() { Target = colorSwap.Target, Color = Color.white });
            }
        }

        DefaultState.Objects = list.ToArray();
        DefaultState.Sprites = defaultSprites.ToArray();
        DefaultState.Colors = defaultColors.ToArray();
    }

    public void Refresh()
    {
        if (Mode >= ModeObjects.Length || Mode < 0)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Mode " + Mode + " is not specified.");
            return;
#endif
        }

        DefaultState.ApplyObjects(false);
        DefaultState.ApplySprites();
        DefaultState.ApplyColors();
        DefaultState.ApplyMaterials();
        DefaultState.ApplyPositions();

        ObjectArray swapData = ModeObjects[Mode];
        swapData.ApplyObjects(true);
        swapData.ApplySprites();
        swapData.ApplyColors();
        swapData.ApplyMaterials();
        swapData.ApplyPositions();
    }
}


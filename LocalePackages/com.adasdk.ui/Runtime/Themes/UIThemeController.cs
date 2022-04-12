using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ThemenType { bg, text, image }

[Serializable]
public class ThemenColors
{
    public ThemenType type;
    public Color color;
}

[Serializable]
public class Theme
{
    public string name;
    public ThemenColors[] Colors;
}

[ExecuteInEditMode]
public class UIThemeController : Controller, ISerializable
{
    /// <summary>
    /// ссылки на все окрашиваемы части интерфейса
    /// </summary>
    [SerializeField]
    private ColorableGraphics[] GraphicsList;

    [SerializeField]
    private List<Theme> Themes;

    private Theme CurrentTheme;
    private int CurrentIndex = 0;

    private Dictionary<ThemenType, Color> ColorsDictionary = new Dictionary<ThemenType, Color>();

    public static UIThemeController Instance => Controllers.Get<UIThemeController>();


    public JSONObject Serialize()
    {
        JSONObject obj = new JSONObject();
        obj["t"] = CurrentIndex;
        return obj;
    }


    public string Name()
    {
        return "thm";
    }

    public void Deserialize(JSONObject obj)
    {
        int CurrentIndex = obj["t"];
        if (CurrentIndex < 0 || CurrentIndex >= Themes.Count)
            CurrentIndex = 0;
        CurrentTheme = Themes[CurrentIndex];
    }

    public void SwitchTheme()
    {
        CurrentIndex++;
        if (CurrentIndex >= Themes.Count)
            CurrentIndex = 0;
        CurrentTheme = Themes[CurrentIndex];
        ColorsUpdate();
        //GoogleTools.LogEvent("SwitchTheme");
    }

    private void ColorsUpdate()
    {
        if (CurrentTheme == null)
            return;


        ColorsDictionary.Clear();

        for (int i = CurrentTheme.Colors.Length - 1; i >= 0; i--)
        {
            if (!ColorsDictionary.ContainsKey(CurrentTheme.Colors[i].type))
                ColorsDictionary.Add(CurrentTheme.Colors[i].type, CurrentTheme.Colors[i].color);
        }


        for (int i = GraphicsList.Length - 1; i >= 0; i--)
        {
            ColorableGraphics graphic = GraphicsList[i];
            graphic.Color = ColorsDictionary[graphic.ThemenType];
        }
    }

    protected void Start()
    {
        ColorsUpdate();
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void OnEnable()
    {
        GraphicsList = Resources.FindObjectsOfTypeAll<ColorableGraphics>();
    }
#endif
}

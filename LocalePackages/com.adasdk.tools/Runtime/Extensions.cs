using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public static class Extensions
{ }

public static class TextAssetExtensions
{
    public static List<List<int>> ParseToInt(this TextAsset ta)
    {
        List<List<int>> table = new List<List<int>>();
        List<int> tempLine = new List<int>();
        string[] lines = ta.text.Split(new char[] { '\r', '\n' });
        foreach (string line in lines)
        {
            if (!line.Equals(""))
            {
                tempLine.Clear();
                string[] _l = line.Split('\t');
                foreach (string row in _l)
                {
                    row.Replace(" ", "");
                    int res = 0;
                    int.TryParse(row, out res);
                    tempLine.Add(res);
                }

                table.Add(new List<int>(tempLine));
            }

        }
        return table;
    }
}

public static class StringExtension
{
    public static bool CheckString(string pattern, params string[] args)
    {
        var characters = new Regex(pattern);
        for (int i = args.Length - 1; i >= 0; i--)
        {
            string s = args[i];
            if (s.Length > 0 && !characters.IsMatch(s))
            {
                return false;
            }
        }
        return true;
    }


}

public static class Texture2DExtensions
{

    public static Texture2D GetTexture(this Texture2D source, Rect rect)
    {
        int x = Mathf.FloorToInt(rect.x);
        int y = Mathf.FloorToInt(rect.y);
        int width = Mathf.FloorToInt(rect.width);
        int height = Mathf.FloorToInt(rect.height);

        Color[] pix = source.GetPixels(x, y, width, height);
        Texture2D destTex = new Texture2D(width, height);
        destTex.SetPixels(pix);
        destTex.Apply();

        return destTex;
    }
    public static Texture2D DeCompress(this Texture2D source, Rect rect)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    (int)rect.width,
                     (int)rect.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}

public static class ListExtension
{
    public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
    {
        // exit if positions are equal or outside array
        if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= list.Count) || (0 > newIndex) ||
            (newIndex >= list.Count)) return;
        T tmp = list[oldIndex];
        // local variables
        int i;
        // move element down and shift other elements up
        if (oldIndex < newIndex)
        {
            for (i = oldIndex; i < newIndex; i++)
            {
                list[i] = list[i + 1];
            }
        }
        // move element up and shift other elements down
        else
        {
            for (i = oldIndex; i > newIndex; i--)
            {
                list[i] = list[i - 1];
            }
        }
        // put element from position 1 to destination
        list[newIndex] = tmp;
    }

    public static void Move<T>(this List<T> list, T item, int newIndex)
    {
        if (item != null)
        {
            var oldIndex = list.IndexOf(item);
            list.Move(oldIndex, newIndex);
        }
    }

    public static T Get<T>(this IList<T> _list, int _index, T _def = default(T))
    {
        if (_list == null || (uint)_index >= (uint)_list.Count)
            return _def;
        return _list[_index];
    }

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void AddIfNotExists<T>(this List<T> list, T value)
    {
        CheckListIsNull(list);
        if (!list.Contains(value))
            list.Add(value);
    }

    public static void AddRangeIfNotExists<T>(this List<T> list, List<T> listValue)
    {
        foreach (var value in listValue)
            if (!list.Contains(value))
                list.Add(value);
    }

    public static void UpdateValue<T>(this List<T> list, T value, T newValue)
    {
        CheckListAndValueIsNull(list, value);
        CheckValueIsNull(newValue);
        var index = list.IndexOf(value);
        list[index] = newValue;
    }

    public static void DeleteIfExists<T>(this List<T> list, T value)
    {
        CheckListAndValueIsNull(list, value);
        if (list.Contains(value))
            list.Remove(value);
    }

    public static bool AreValuesEmpty<T>(this List<T> list)
    {
        CheckListIsNull(list);
        return list.FindAll(x => x == null).Count > 0;
    }

    private static void CheckListAndValueIsNull<T>(this List<T> list, T value)
    {
        CheckListIsNull(list);
        CheckValueIsNull(value);
    }

    private static void CheckValueIsNull<T>(T value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
    }

    private static void CheckListIsNull<T>(this IList<T> list)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
    }
}

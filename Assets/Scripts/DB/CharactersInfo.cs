using System;
using UnityEngine;

[Serializable]
public class Variant
{
    public Sprite icon;
    public Sprite images;

    public Variant(Sprite icon, Sprite images)
    {
        this.icon = icon;
        this.images = images;
    }
}

[Serializable]
public class CharachtersItem
{
    public string name;
    public Sprite Icon;
    public Variant[] Variants;
    public Category category;
    public bool isDefault = false;
    public SiblingPosition SiblingPosition;
}

public enum Category {
    body    = 0,
    eyes    = 1,
    lips    = 2,
    brows   = 3,
    hairs   = 4, 
    tops    = 5,
    bottoms = 6,
    upper   = 7,
    shoes   = 8,
    other   = 9,
    Beards  = 10,
    necklace    = 11,
    bracelet    = 12
}

public enum SiblingPosition
{
    BeforeLast,
    First,
    Second, 
    Last
}

public class CharactersInfo : DBItem
{
    public string Name => name;
    public Sprite Icon;
    public CharachtersItem[] Items;
}

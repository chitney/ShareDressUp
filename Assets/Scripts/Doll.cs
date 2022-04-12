using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doll : MonoBehaviour
{
    [SerializeField]
    private Image body;
    [SerializeField]
    private UIList images;

    public static System.Action OnItemRemoved;

    private Dictionary<Category, UIItemButton> inGameVariants = new Dictionary<Category, UIItemButton>();

    public void Init(CharactersInfo character)
    {
        body.sprite = character.Items[0].Variants[0].images;
        images.Clear();
        inGameVariants.Clear();

        for (int i = 0; i < character.Items.Length; i++)
        {
            CharachtersItem item = character.Items[i];
            if (item.isDefault)
                InitItem(item, 0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="category">index of item in db array</param>
    /// <param name="variant"></param>
    public void InitItem(CharachtersItem item, int variant)
    {
        if (item.category == Category.body)
        {
            body.sprite = item.Variants[variant].images;
            return;
        }

        UIItemButton uIItemImage;

        if (inGameVariants.ContainsKey(item.category))
        {
            uIItemImage = inGameVariants[item.category];
        }
        else
        {
            uIItemImage = images.GetNext() as UIItemButton;
            inGameVariants.Add(item.category, uIItemImage);

            uIItemImage.name = item.name;

            switch (item.SiblingPosition)
            {
                case SiblingPosition.First:
                    uIItemImage.transform.SetAsFirstSibling();
                    break;
                case SiblingPosition.Second:
                    uIItemImage.transform.SetSiblingIndex(1);
                    break;
                case SiblingPosition.BeforeLast:
                    uIItemImage.transform.SetSiblingIndex(MaxSiblingIndex - 1);
                    break;
                case SiblingPosition.Last:
                    uIItemImage.transform.SetAsLastSibling();
                    break;
            }
        }

        Sprite img = item.Variants[variant].images;

        if (item.isDefault)
        {
            uIItemImage.Image.raycastTarget = false;
            uIItemImage.Init(img);
        }
        else
        {
            uIItemImage.Image.raycastTarget = true;
            uIItemImage.Init(img, () => Remove(item.category));
        }
    }

    /*
    SiblingIndex 
    0 - body
    1,2,3 - eyes, brows, lips
    last (childCount) - hairs
    */

    private int MaxSiblingIndex => transform.childCount - 1;

    public bool IsItemInGame(Category category)
    {
        return inGameVariants.ContainsKey(category);
    }

    public void Remove(Category category)
    {
        if (IsItemInGame(category))
        {
            UIItemButton item = inGameVariants[category];
            images.Remove(item);
            inGameVariants.Remove(category);
            OnItemRemoved?.Invoke();
        }
    }

    /// <summary>
    /// Set for categories item sibling index +1
    /// (1, MaxSiblingIndex-1]
    /// </summary>
    public void Up(Category category)
    {
        if (IsItemInGame(category))
        {
            UIItemButton item = inGameVariants[category];
            int currentIndex = item.transform.GetSiblingIndex();
            if (currentIndex < MaxSiblingIndex - 1)
                item.transform.SetSiblingIndex(currentIndex + 1);
            
        }
    }

    /// <summary>
    /// Set for categories item sibling index -1
    /// (1, MaxSiblingIndex-1]
    /// </summary>
    public void Down(Category category)
    {
        if (IsItemInGame(category))
        {
            UIItemButton item = inGameVariants[category];
            int currentIndex = item.transform.GetSiblingIndex();
            if (currentIndex > 2)
                item.transform.SetSiblingIndex(currentIndex - 1);
            
        }
    }

    public void CopyImages(Doll exampleDoll)
    {
        images.Clear(); 
        UIItemButton uIItemImage;
        foreach (UIItemButton exampleitem in exampleDoll.images)
        {
            if (exampleitem.gameObject.activeSelf)
            {
                uIItemImage = images.GetNext() as UIItemButton;
                uIItemImage.Image.sprite = exampleitem.Image.sprite;
                uIItemImage.transform.SetSiblingIndex(exampleitem.transform.GetSiblingIndex());
            }
        }
        body.sprite = exampleDoll.body.sprite;
    }
}

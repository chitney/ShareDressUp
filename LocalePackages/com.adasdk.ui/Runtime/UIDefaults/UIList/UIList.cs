using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIList : MonoBehaviour, IEnumerable<UIItem>
{
    [SerializeField]
    private bool SortDisableItems = true;
    [SerializeField] 
    private UIItem prefab;

    private List<UIItem> items = new List<UIItem>();

    System.Comparison<UIItem> comparison = (i1, i2) => i2.gameObject.activeSelf.CompareTo(i1.gameObject.activeSelf);
    public UIItem this[int _index]
    {
        get 
        {
            if (SortDisableItems)
                items.Sort(comparison);

            UIItem item;
            if (items.Count > _index)
            {
                item = items[_index];
                item.gameObject.SetActive(true);
            }
            else
                item = Create();

            if (item != null)
                item.Index = _index;

            return item;
        }
    }

    /// <summary>
    /// count of active obj
    /// </summary>
    public int Count
    {
        get { return items.FindAll(e => e.gameObject.activeSelf == true).Count; }
    }

    /// <summary>
    /// hide all of items
    /// </summary>
    public void Clear()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            UIItem item = items[i];
            item.gameObject.SetActive(false);
        }
        ResetIndexes(); 
    }

    /// <summary>
    /// reset indexes
    /// </summary>
    public void ResetIndexes()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            UIItem item = items[i];
            item.Index = items.IndexOf(item);
        }
    }

    public UIItem Create()
    {
        UIItem item = prefab.gameObject.activeSelf ? Instantiate(prefab, prefab.transform.position, Quaternion.identity, this.transform) : prefab;
        items.Add(item);
        item.gameObject.SetActive(true);
        return item;
    }

    public void Remove(UIItem item)
    {
        item.gameObject.SetActive(false);
        ResetIndexes();
    }

    public UIItem GetNext()
    {
        UIItem item = items.Find(i => !i.gameObject.activeSelf);
        if (item != null)
        {
            item.gameObject.SetActive(true);
            return item;
        }
        return this[Count + 1];
    }

    public int IndexOf(UIItem item)
    {
        return items.IndexOf(item);
    }

    public UIItem Get(int index)
    {
        UIItem item = GetNext();
        if (item.Index != index)
        {
            items.Move(item, index);
            item.transform.SetSiblingIndex(index+1);
        }
            
        ResetIndexes();
        return item;
    }

    public IEnumerator<UIItem> GetEnumerator()
    {
        for (int i = 0; i < items.Count; i++)
            yield return items[i];

        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

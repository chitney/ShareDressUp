using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Ext{
    public static int Count(this List<Item> items, ItemsInfo info)
    {
        Item current = items.Find(i => i.Id == info.id);
        if (current!=null)
        {
            return current.Count;
        }
        return 0;
    }
}

[Serializable]
public class Item
{
    public static Action<int, int> OnItemUpdate;

    [DBItem(typeof(ItemsDatabase)), SerializeField]
    private int id;
    private int count;

    public Item(int id, int count)
    {
        this.id = id;
        this.Count = count;
    }

    public int Count
    {
        get => count;
        set
        {
            int diff = count - value;
            count = value;
            if (count < 0) count = 0;

            OnItemUpdate?.Invoke(id, diff);
        }
    }

    public int Id
    {
        get => id;
    }

    public ItemsInfo Info { get => ItemsDatabase.Instance.GetItemById(id); }
}

public class PlayerInfoController : Controller, ISerializable
{
    public static PlayerInfoController Instance => Controllers.Get<PlayerInfoController>();
    [NonSerialized]
    private int level = 0;

    #region Level

    public int Level => level;

    private int LevelCount
    {
        get
        {
            return LevelItem.Count;
        }
    }

    [DBItem(typeof(ItemsDatabase)), SerializeField]
    private int LevelItemId;

    private Item LevelItem
    {
        get
        {
            if (levelItem == null)
                levelItem = Items.Find(i => i.Id == LevelItemId);
            return levelItem;
        }
        set
        {
            levelItem = value;
        }
    }

    public Item levelItem;

    public void LevelUp()
    {
        LevelItem.Count++;
        level = LevelCount;
        //GoogleTools.LogLevelUp(level);
    }

    public void SetLevel(int count)
    {
        LevelItem.Count = count;
        level = LevelCount;
    }

    #endregion

    #region Score
    int[] records = new int[0];
    private List<int> gp_recordsList = new List<int>();
    private List<int> recordsList = new List<int>();

    public List<int> Scores
    {
        get
        {
            return recordsList;
        }
    }

    public int MaxScore
    {
        get
        {
            if (recordsList.Count > 0)
                return recordsList[0];
            return 0;
        }
    }

    public static void AddScore(int score)
    {
        Instance.addScore(score);
    }

    public static void AddHighScore(int score)
    {
        Instance.addHighScore(score);
    }

    private void addHighScore(int score)
    {
        if (!gp_recordsList.Contains(score))
        {
            gp_recordsList.Add(score);
            gp_recordsList.Sort();
            gp_recordsList.Reverse();
        }
    }


    private void addScore(int score)
    {
        if (!recordsList.Contains(score) && score != 0)
        {
            recordsList.Add(score);
            recordsList.Sort();
            recordsList.Reverse();
            recordsList.RemoveAll(s => s <= 0);
            records = recordsList.ToArray();
        }
    }

    private void LoadScore()
    {
        Array.Sort(records);
        Array.Reverse(records);
        foreach (int rec in records)
            if (!recordsList.Contains(rec) && rec != 0)
                recordsList.Add(rec);
        if (recordsList.Count > 20)
        {
            recordsList.RemoveRange(21, recordsList.Count - 20);
        }
    }

    #endregion Score

    #region Save

    int lastScore = 0;
    public int LastScore => lastScore;

    public void Save(int _score)
    {
        lastScore = _score;
    }

    public void ClearSaves()
    {
        lastScore = 0;
    }

    public bool HaveSave
    {
        get
        {
            return lastScore > 0;
        }
    }

   

    #endregion

    #region Items
    [NonSerialized]
    public List<Item> Items = new List<Item>();

    public void AddItems(int _id, int _count)
    {
        var item = Items.Find(i => i.Id == _id);
        if (item != null)
        {
            item.Count += _count;
        }
        else Items.Add(new Item(_id, _count));
    }

    /// <summary>
    /// add default count
    /// </summary>
    /// <param name="_id"></param>
    public void AddItems(int _id)
    {
        ItemsInfo item = ItemsDatabase.Instance.GetItemById(_id);
        if (item != null)
        {
            AddItems(item.id, item.DefaultCount);
        }
    }

    public void RemoveItems(int _id, int _count)
    {
        var item = Items.Find(i => i.Id == _id);
        if (item != null)
        {
            item.Count -= _count;
        }
        else AddItems(_id, 0);
    }

    #endregion Items

    public JSONObject Serialize()
    {
        JSONObject obj = new JSONObject();
        recordsList.RemoveAll(s => s == 0);
        JSONArray recArray = new JSONArray();
        foreach (var rec in recordsList)
        {
            recArray.Add(rec);
        }
        obj.Add("rec", recArray);

        JSONArray ItemArray = new JSONArray();
        foreach (var item in Items)
        {
            JSONObject itemObj = new JSONObject();
            itemObj["id"] = item.Id;
            itemObj["c"] = item.Count;

            recArray.Add(itemObj);
        }

        obj.Add("itm", ItemArray);
        obj["s"] = lastScore;
        return obj;
    }


    public void Deserialize(JSONObject obj)
    {
        if (Items.Count != ItemsDatabase.Instance.Items.Count)
        {
            InitItems();
        }

        if (obj == null)
            return;

        JSONArray recArray = obj["rec"] as JSONArray;
        recordsList = new List<int>();
        if (recArray!=null)
            foreach (JSONNode _obj in recArray)
            {
                recordsList.Add(_obj);
            }

        lastScore = obj["s"];
        LoadScore();

        JSONArray ItemArray = obj["itm"] as JSONArray;
        if (ItemArray!=null)
            foreach (JSONNode _obj in ItemArray)
            {
                int id = _obj["id"];
                int cnt = _obj["c"];
                Item item = Items.Find(i => i.Id == id);
                if (item!=null)
                    item.Count = cnt;
            }

        level = LevelCount;
    }

    public string Name()
    {
        return "plinc";
    }


    public void InitItems()
    {
        Items.Clear();
        foreach (ItemsInfo itemInfo in ItemsDatabase.Instance.Items)
        {
            Items.Add(new Item(itemInfo.id, itemInfo.DefaultCount));
        }

        LevelItem = Items.Find(i => i.Id == LevelItemId);
    }

}

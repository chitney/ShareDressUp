using UnityEngine;

[CreateAssetMenu(fileName = "ItemsDatabase", menuName = "ScriptableObjects/DB/ItemsDatabase")]
public class ItemsDatabase : DBBase
{
    public static ItemsDatabase Instance { get { return DBManager.Get<ItemsDatabase>(); } }
    public ItemsInfo GetItemById(int id)
    {
        return Items.Find(i => i.id == id) as ItemsInfo;
    }
}

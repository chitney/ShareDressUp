using UnityEngine;

[System.Serializable]
public class Bachground
{
    public Sprite icon;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "CharactersDatabase", menuName = "ScriptableObjects/DB/ CharactersDatabase")]
public class CharactersDatabase : DBBase
{
    public static CharactersDatabase Instance { get { return DBManager.Get<CharactersDatabase>(); } }

    public Bachground[] Bachgrounds;
    
    public CharactersInfo GetItemById(int id)
    {
        return Items.Find(i => i.id == id) as CharactersInfo;
    }

    public CharactersInfo GetItemByName(string name)
    {
        return Items.Find(i => i.name == name) as CharactersInfo;
    }
#if UNITY_EDITOR
    public CharactersInfo CreateNewItem() {
        return CreateNewInstance<CharactersInfo>();
    }
#endif

}

using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerScriptsInfo
{
    private SriptDBItem info;
    private bool isDone;

    public PlayerScriptsInfo(JSONObject obj)
    {
        this.info = ScriptsDatabase.Instance.GetById(obj["id"]) as SriptDBItem;
        this.isDone = obj["d"];
    }

    public PlayerScriptsInfo(SriptDBItem info)
    {
        this.info = info;
        this.isDone = false;
    }

    public bool IsDone { get => isDone; private set => isDone = value; }

    public int Id => info.id;

    public SriptDBItem Info => info;

    public void SetDone()
    {
        IsDone = true;
    }

    public JSONObject Serialize()
    {
        JSONObject obj = new JSONObject();
        obj["id"] = Id;
        obj["d"] = IsDone;
        return obj;
    }


}

public class PlayerScriptsController : Controller
{
    private List<PlayerScriptsInfo> scripts = new List<PlayerScriptsInfo>();

    public static PlayerScriptsController Instance => Controllers.Get<PlayerScriptsController>();

    public override string Name()
    {
        return "scrc";
    }

    public override void Deserialize(JSONObject obj)
    {
        var dbItems = ScriptsDatabase.Instance.Items;
        scripts = new List<PlayerScriptsInfo>();

        if (obj!=null)
        {
            JSONArray array = (JSONArray)obj["a"];
            foreach (JSONObject json_scripts in array)
            {
                if (ScriptsDatabase.Instance.GetById(json_scripts["id"])!=null)                
                    scripts.Add(new PlayerScriptsInfo(json_scripts));
            }

            foreach (SriptDBItem dbItem in dbItems)
            {
                if (scripts.Find(i => i.Id == dbItem.id) == null)
                {
                    scripts.Add(new PlayerScriptsInfo(dbItem));
                }
            }
        }
        else
        {
            foreach (SriptDBItem dbItem in dbItems)
            {
                scripts.Add(new PlayerScriptsInfo(dbItem));
            }
        }
    }

    public override JSONObject Serialize()
    {
        JSONObject obj = new JSONObject();
        JSONArray array = new JSONArray();
        foreach (var script in scripts)
        {
            array.Add(script.Serialize());
        }
        obj.Add("a", array);
        return obj;

    }

    private void Start()
    {
        if (Controllers.IsDeserialized)
            StartScripts();
        else
            Controllers.OnDeserialize += StartScripts;
    }


    private void StartScripts()
    {
        Controllers.OnDeserialize -= StartScripts;
        var script = scripts.Find(s => !s.IsDone);
        if (script!=null)
        {
            Instantiate(script.Info.PlayerScriptPrefab, this.transform);
        }
    }

    public void SetScriptDone(PlayerScript item)
    {
        var script = scripts.Find(s => s.Info.id == item.Info.id);
        script.SetDone();
        Destroy(item.gameObject);
        StartScripts();
    }

}

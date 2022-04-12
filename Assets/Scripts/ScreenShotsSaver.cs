using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenShotsSaver : Controller, ISerializable
{
    private List<string> pictures = new List<string>();
    private List<Texture2D> textures = new List<Texture2D>();

    public static Action onPngRemoved;

    public List<Texture2D> Textures => textures;

    private static string FolderPath => Application.persistentDataPath + "/PNG/";
    private static string PngFullPath(string pName) => FolderPath+pName+".png";

    public static string PngName()
    {
        return string.Format("screen_{0}",System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public static ScreenShotsSaver Instance => Controllers.Get<ScreenShotsSaver>();
    public string Name()
    {
        return "csaver";
    }

    private bool Write(Texture2D screenShot)
    {
        try
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            byte[] bytes = screenShot.EncodeToPNG();
            string name = PngName();
            screenShot.name = name;
            System.IO.File.WriteAllBytes(PngFullPath(name), bytes);
            return true;
        }
        catch
        {
            HintsController.ShowText("error");
            //GoogleTools.LogEvent("Error_write");
            return false;
        }
    }

    private Texture2D Read(string pngName)
    {
        Texture2D tex = null;
        byte[] fileData;

        string filePath = PngFullPath(pngName);

        try
        {
            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(0, 0);
                tex.LoadImage(fileData);
                tex.name = pngName;
            }
        }
        catch {
            //GoogleTools.LogEvent("Error_read");
        }

        if (tex!=null)
        {
            textures.Add(tex);
            pictures.Add(pngName);
        }
        return tex;
    }

    Texture2D _texture;

    public void Save(Texture2D texture2D)
    {
        //GoogleTools.LogEvent("Save");
        if (Write(texture2D))
        {
            UnityADSController.Show("Interstitial_Android");
            _texture = Read(texture2D.name);
            UIPreview.Instance.Open(_texture);
        }
    }

    public void Remove(string pngName)
    {
        //GoogleTools.LogEvent("Remove");

        pictures.Remove(pngName);
        var t = textures.Find(t => t.name == pngName);
        textures.Remove(t);

        string filePath = PngFullPath(pngName);

        try
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            onPngRemoved?.Invoke();
        }
        catch {
            //GoogleTools.LogEvent("Error_remove");
        }
    }

    public void Share(string name)
    {
        //GoogleTools.LogEvent("Share");
        ShareTool.Share(PngFullPath(name));
    }


    public void Deserialize(JSONObject obj)
    {
        if (obj == null)
            return;

        JSONArray recArray = obj["png"] as JSONArray;
        pictures = new List<string>();
        textures = new List<Texture2D>();

        if (recArray != null)
            foreach (JSONNode _obj in recArray)
            {
                Read(_obj);
            }
    }

    public JSONObject Serialize()
    {
        JSONObject obj = new JSONObject();
        JSONArray pngArray = new JSONArray();
        foreach (var png in pictures)
        {
            pngArray.Add(png);
        }
        obj.Add("png", pngArray);

        return obj;
    }
}

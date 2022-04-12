using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serializer : MonoBehaviour
{
    public static bool IsDeserialized = false;
    public static Action OnDeserialize;

    private List<ISerializable> items = new List<ISerializable>();

    private Coroutine _saveCoroutine;

    private void OnDestroy()
    {
        if (_saveCoroutine != null)
            StopCoroutine(_saveCoroutine);
    }

    public void Init(Controller[] controllersArray)
    {
        items = new List<ISerializable>();
        IsDeserialized = false;

        for (int i = controllersArray.Length - 1; i >= 0; i--)
        {
            Controller controller = controllersArray[i];
            if (controller is ISerializable)
            {
                items.Add(controller as ISerializable);
            }
        }

#if UNITY_EDITOR
        Debug.Log("Serializable items count " + items.Count);

        CheckNames();
#endif

        Deserialize();

        if (_saveCoroutine != null)
            StopCoroutine(_saveCoroutine);

        _saveCoroutine = StartCoroutine(SaveCoroutine());
    }

    private IEnumerator SaveCoroutine()
    {
        while (gameObject.activeSelf)
        {
            Serialize();
            yield return null;
        }
        yield return null;
    }

    private void Deserialize()
    {
        string json = PlayerPrefs.GetString(name, string.Empty);
        JSONObject obj = json.Equals("") ? new JSONObject() : (JSONObject)JSONNode.Parse(json);
        foreach (ISerializable controller in items)
        {
            controller.Deserialize(obj[controller.Name()] as JSONObject);
        }
        OnDeserialize?.Invoke();
        IsDeserialized = true;

#if UNITY_EDITOR || DEBUG
        Debug.Log("Deserialize");
#endif
    }

    private void Serialize()
    {
        JSONObject obj = new JSONObject();

        foreach (ISerializable controller in items)
            obj[controller.Name()] = controller.Serialize();

        PlayerPrefs.SetString(name, obj.ToString());
    }


#if UNITY_EDITOR
    private void CheckNames()
    {
        List<string> names = new List<string>();
        foreach (var c in items)
            if (names.Contains(c.Name()))
            {
                Debug.LogError("Names dublicate "  + c.Name() + " "+ c.GetType());
            }
            else
            {
                names.Add(c.Name());
            }
    }
#endif
}

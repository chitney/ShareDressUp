using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Controllers : MonoBehaviour
{
    [SerializeField]
    private Controller[] ControllersArray;
    [SerializeField]
    private Serializer serializer;

    private Dictionary<Type, object> cachedControllers;

    public static Controllers Instance => instance;

    private static Controllers instance;

    private void InitDictionary()
    {
        cachedControllers = new Dictionary<System.Type, object>();
        foreach (Controller obj in ControllersArray)
            cachedControllers.Add(obj.GetType(), obj);
    }

    private T GetByType<T>()
    {
        Type type = typeof(T);
        if (cachedControllers == null)
            InitDictionary();
        try
        {
            if (cachedControllers.ContainsKey(type))
                return (T)cachedControllers[type];

            return default;
        }
        catch
        {
            Debug.LogError(type);
            return default;
        }
    }

    public static T Get<T>()
    {
        return Instance.GetByType<T>();
    }


    void Awake()
    {
        instance = this;

        serializer.Init(ControllersArray);

        for (int i = ControllersArray.Length - 1; i >= 0; i--)
        {
            Controller controller = ControllersArray[i];
            controller.Init();
        }
    }


#if UNITY_EDITOR && !RUN
    void OnEnable()
    {
        if (!Application.isPlaying)
        {
            Awake();
            ControllersArray = FindObjectsOfType<Controller>(true);
        }
    }
#endif
}
